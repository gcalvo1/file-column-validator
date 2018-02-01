namespace CCubed_2012.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedExecutionLogIdFromByteToInt : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ExecutionLogModels");
            AlterColumn("dbo.ExecutionLogModels", "Id", c => c.Int(nullable: false, identity: true));
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
