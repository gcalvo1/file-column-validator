namespace CCubed_2012.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateControlTable1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CCubedControlTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CheckType = c.String(nullable: false),
                        Client = c.String(nullable: false),
                        Project = c.String(nullable: false),
                        TemplateFilePath = c.String(nullable: false),
                        RawFilePath = c.String(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CCubedControlTables");
        }
    }
}
