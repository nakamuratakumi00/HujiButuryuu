namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.m_unsou_hinmei_koyuu", "DELFLG", c => c.String(nullable: false, maxLength: 1));
            AddColumn("dbo.m_unsou_todokesaki_koyuu", "DELFLG", c => c.String(nullable: false, maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.m_unsou_todokesaki_koyuu", "DELFLG");
            DropColumn("dbo.m_unsou_hinmei_koyuu", "DELFLG");
        }
    }
}
