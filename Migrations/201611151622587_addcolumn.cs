namespace CCubed_2012.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CCubedControlTableModels", "ColumnDelimiter", c => c.String(nullable: false));
 }
        
        public override void Down()
        {
            DropColumn("dbo.CCubedControlTableModels", "ColumnDelimiter");
        }
    }
}
