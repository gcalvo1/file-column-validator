namespace CCubed_2012.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class DiscrepColsNoLongerRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LogModels", "DiscrepancyColumns", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LogModels", "DiscrepancyColumns", c => c.String(nullable: false));
        }
    }
}
