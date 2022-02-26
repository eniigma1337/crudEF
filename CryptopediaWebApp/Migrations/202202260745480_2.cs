namespace WebApplication3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Collections", "CollectionsYear", c => c.String());
            DropColumn("dbo.Collections", "CollectionsStandard");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Collections", "CollectionsStandard", c => c.String());
            DropColumn("dbo.Collections", "CollectionsYear");
        }
    }
}
