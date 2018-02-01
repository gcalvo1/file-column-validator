namespace CCubed_2012.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryingToGetLogIdentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
            DropTable("dbo.LogModels");
        }
    }
}
