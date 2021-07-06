namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.m_account", "bumon_cd", c => c.String(nullable: false, maxLength: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.m_account", "bumon_cd", c => c.String(nullable: false, maxLength: 2));
        }
    }
}
