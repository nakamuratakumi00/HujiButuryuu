namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.m_unsou_hinmei", "DELFLG", c => c.String(nullable: false, maxLength: 1));
            AddColumn("dbo.m_unsou_todokesaki", "DELFLG", c => c.String(nullable: false, maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.m_unsou_todokesaki", "DELFLG");
            DropColumn("dbo.m_unsou_hinmei", "DELFLG");
        }
    }
}
