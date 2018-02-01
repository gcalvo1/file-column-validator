namespace CCubed_2012.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateControlTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO CCubedControlTables VALUES ('Column','Campbells','Finance','\\\\PSCETLU00106\\MECAnalytics\\Client\\Campbells\\Raw Data\\Mediatools\\Validation\\Template\\','\\PSCETLU00106\\MECAnalytics\\Client\\Campbells\\Raw Data\\Mediatools\\','George Calvo',getdate())");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM CCubedControlTables WHERE Id = 1");
        }
    }
}
