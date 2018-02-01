using CCubed_2012.Interfaces;
using CCubed_2012.Models;


namespace CCubed_2012.Classes
{
    public class DatabaseLogger : ILogger
    {
        private readonly ApplicationDbContext _context;

        public DatabaseLogger()
        {
            _context = new ApplicationDbContext();
        }

        protected void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public void Log(LogModel logModel)
        {
           _context.LogModel.Add(logModel);

            _context.SaveChanges();
        }
    }
}