using CCubed_2012.Classes;
using CCubed_2012.Interfaces;
using CCubed_2012.Models;
using CCubed_2012.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace CCubed_2012.Controllers
{
    public class ValidationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IList<ILogger> _loggers;

        public ValidationController()
        {
            _loggers = new List<ILogger>();
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //return the view model with values selected from the database
        public ActionResult Index()
        {
            var viewModel = new ValidationFormViewModel()
            {
                Client =
                _context.CCubedControlTable
                    .Select(c => c.Client)
                    .Distinct().ToList(),
                Project =
                _context.CCubedControlTable
                    .Select(c => c.Project)
                    .Distinct().ToList(),
                CheckType =
                _context.CCubedControlTable
                    .Select(c => c.CheckType)
                    .Distinct().ToList()
            };
            return View(viewModel);
        }

        //When submit button is clicked, execute the below code in order to run validation
        [HttpPost]
        public ActionResult Result(ValidationFormViewModel viewModel)
        {
            //Add logging types for polymorphic logging to multiple destinations
            RegisterLogger(new DatabaseLogger());

            if (!ModelState.IsValid)
            {
                var formViewModel = new ValidationFormViewModel()
                {
                    ClientId = viewModel.ClientId,
                    ProjectId = viewModel.ProjectId,
                    CheckTypeId = viewModel.CheckTypeId
                };
                return View("Index", formViewModel);
            }

            var fileInfo =
              (from v in _context.CCubedControlTable
               where v.Client.Equals(viewModel.ClientId)
                     && v.Project.Equals(viewModel.ProjectId)
                     && v.CheckType.Equals(viewModel.CheckTypeId)
               select new {v.RawFilePath, v.TemplateFilePath, v.ColumnDelimiter }).FirstOrDefault();

            var resultViewModel = new ValidationResultViewModel() { Results = new List<ValidationResultModel>() };

            if (fileInfo != null)
            {
                var directoryInfo = new DirectoryInfo(fileInfo.TemplateFilePath);

                //Check to see if a template file exists. If not return No template file located to result view
                if (!directoryInfo.GetFiles("*.*").Any())
                {
                    var resultModel = new ValidationResultModel
                    {
                        IsValidated = "No Template File Located",
                        FileName = "Please Place A Template File Here: " + fileInfo.TemplateFilePath,
                        DiscrepancyColumns = ""
                    };

                    var executionLogRecord = InitializeLogModelObject(viewModel.ClientId, viewModel.ProjectId,
                        viewModel.CheckTypeId, "N/A", false, "No Template File Located", DateTime.Now);

                    foreach (var logger in _loggers)
                    {
                        logger.Log(executionLogRecord);
                    }

                    resultViewModel.Results.Add(resultModel);
                }
                else
                {

                    var templateFullFilePath = Directory.GetFiles(fileInfo.TemplateFilePath)[0];
                    var rawFullFilePaths = Directory.GetFiles(fileInfo.RawFilePath).ToList();
                    
                    //Check to see if template file is open
                    var templateFileInfo = new FileInfo(templateFullFilePath);

                    var fileManipulator = new FileManipulator();

                    if (fileManipulator.IsFileLocked(templateFileInfo))
                    {
                        var resultModel = new ValidationResultModel
                        {
                            IsValidated = "Template file is open in another program. Please close the file and try again.",
                            FileName = Path.GetFileName(templateFullFilePath),
                            DiscrepancyColumns = ""
                        };

                        var executionLogRecord = InitializeLogModelObject(viewModel.ClientId, viewModel.ProjectId,
                            viewModel.CheckTypeId, Path.GetFileName(templateFullFilePath), false, "Template File Open In Another Program", DateTime.Now);

                        foreach (var logger in _loggers)
                        {
                            logger.Log(executionLogRecord);
                        }

                        resultViewModel.Results.Add(resultModel);
                    }
                    else
                    {

                        foreach (var rawFullFilePath in rawFullFilePaths)
                        {
                            var resultModel = new ValidationResultModel();

                            //Check to see if raw file is open
                            var rawFileInfo = new FileInfo(rawFullFilePath);

                            if (fileManipulator.IsFileLocked(rawFileInfo))
                            {
                                resultModel.IsValidated =
                                    "Raw file is open in another program. Please close the file and try again.";
                                resultModel.FileName = Path.GetFileName(rawFullFilePath);
                                resultModel.DiscrepancyColumns = "";

                                var executionLogRecord = InitializeLogModelObject(viewModel.ClientId,
                                    viewModel.ProjectId,
                                    viewModel.CheckTypeId, Path.GetFileName(rawFullFilePath), false,
                                    "Raw File Open In Another Program", DateTime.Now);

                                foreach (var logger in _loggers)
                                {
                                    logger.Log(executionLogRecord);
                                }

                                resultViewModel.Results.Add(resultModel);
                            }
                            else
                            {

                                var columnChecker = new ColumnChecker(rawFullFilePath, templateFullFilePath);

                                var discrepancyString = columnChecker.ColumnDifference(1, fileInfo.ColumnDelimiter);

                                //if discrepancyString == "" then there were no discrepancies when validating column names/order
                                if (discrepancyString == "")
                                {
                                    resultModel.IsValidated = "Validated";
                                    resultModel.FileName = Path.GetFileName(rawFullFilePath);
                                    resultModel.DiscrepancyColumns = null;

                                    var executionLogRecord = InitializeLogModelObject(viewModel.ClientId,
                                        viewModel.ProjectId,
                                        viewModel.CheckTypeId, Path.GetFileName(rawFullFilePath), true, null,
                                        DateTime.Now);

                                    foreach (var logger in _loggers)
                                    {
                                        logger.Log(executionLogRecord);
                                    }
                                }

                                else
                                {
                                    resultModel.IsValidated = "Not Validated";
                                    resultModel.FileName = Path.GetFileName(rawFullFilePath);
                                    resultModel.DiscrepancyColumns = discrepancyString;

                                    var executionLogRecord = InitializeLogModelObject(viewModel.ClientId,
                                        viewModel.ProjectId,
                                        viewModel.CheckTypeId, Path.GetFileName(rawFullFilePath), false,
                                        discrepancyString,
                                        DateTime.Now);

                                    foreach (var logger in _loggers)
                                    {
                                        logger.Log(executionLogRecord);
                                    }

                                }

                                resultViewModel.Results.Add(resultModel);
                            }
                        }
                    }
                }
            }
            else
            {
                //return Content("Error with control table setup. Please contact a Developer");
                var resultModel = new ValidationResultModel
                {
                    IsValidated = "Incorrect Selection",
                    FileName = "Please Select Different Parameters",
                    DiscrepancyColumns = ""
                };

                var executionLogRecord = InitializeLogModelObject(viewModel.ClientId, viewModel.ProjectId,
                    viewModel.CheckTypeId, "N/A", false, "Invalid Parameter Selection", DateTime.Now);

                foreach (var logger in _loggers)
                {
                    logger.Log(executionLogRecord);
                }

                resultViewModel.Results.Add(resultModel);
            }

            return View(resultViewModel);

             
        }

        //Method to initialize a LogMode object with certain parameters
        public LogModel InitializeLogModelObject(string client, string project, string checkType,
            string fileName, bool isValidated, string discrepancyColumns, DateTime requestDate)
        {
            return new LogModel
            {
                Client = client,
                Project = project,
                CheckType = checkType,
                FileName = fileName,
                IsValidated = isValidated,
                DiscrepancyColumns = discrepancyColumns,
                RequestDate = requestDate
            };
        }

        //Add a logging type (logger) to pass to the Interface in order to make a polymorphic logging type decision
        public void RegisterLogger(ILogger logger)
        {
            _loggers.Add(logger);
        }

        //Used to get filtered list of projects based on client-side Client selection in dropdown list
        [HttpPost]
        public ActionResult GetProjects(string clientName)
        {
            IEnumerable<SelectListItem> projectList = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(clientName))
            {
                projectList = (from m in _context.CCubedControlTable where m.Client == clientName select m)
                    .AsEnumerable()
                    .Select(m => new SelectListItem() { Text = m.Project, Value = m.Project });
            }
            var result = Json(new SelectList(projectList, "Value", "Text"));
            return result;
        }

        //Used to get filtered list of checkTypes based on client-side Client and Project selection in dropdown lists
        [HttpPost]
        public ActionResult GetCheckTypes(string clientName, string projectName)
        {
            IEnumerable<SelectListItem> checkTypeList = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(clientName))
            {
                checkTypeList = (from m in _context.CCubedControlTable where m.Client == clientName && m.Project == projectName select m)
                    .AsEnumerable()
                    .Select(m => new SelectListItem() { Text = m.CheckType, Value = m.CheckType });
            }
            var result = Json(new SelectList(checkTypeList, "Value", "Text"));
            return result;
        }
    }
}