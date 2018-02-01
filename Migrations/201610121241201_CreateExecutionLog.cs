namespace CCubed_2012.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateExecutionLog : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CCubedControlTables", newName: "CCubedControlTableModels");
            CreateTable(
                "dbo.ExecutionLogModels",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Client = c.String(nullable: false),
                        Project = c.String(nullable: false),
                        CheckType = c.String(nullable: false),
                        FileName = c.String(nullable: false),
                        IsValidated = c.Boolean(nullable: false),
                        DiscrepancyColumns = c.String(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExecutionLogModels");
            RenameTable(name: "dbo.CCubedControlTableModels", newName: "CCubedControlTables");
        }
    }
}
