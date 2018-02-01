namespace CCubed_2012.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedDatabaseGeneratedOptionFromExecutionLog : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ExecutionLogModels");
            AlterColumn("dbo.ExecutionLogModels", "Id", c => c.Byte(nullable: false));
            AddPrimaryKey("dbo.ExecutionLogModels", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ExecutionLogModels");
            AlterColumn("dbo.ExecutionLogModels", "Id", c => c.Byte(nullable: false, identity: true));
            AddPrimaryKey("dbo.ExecutionLogModels", "Id");
        }
    }
}
