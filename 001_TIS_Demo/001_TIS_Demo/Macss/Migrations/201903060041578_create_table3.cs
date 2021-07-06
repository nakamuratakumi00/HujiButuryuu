namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.m_unsou_postalcode");
            AddPrimaryKey("dbo.m_unsou_postalcode", new[] { "YUBINN", "JYUSY1", "JYUSY2", "JYKANA", "KENJY1", "KENJY2", "KENJY3" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.m_unsou_postalcode");
            AddPrimaryKey("dbo.m_unsou_postalcode", new[] { "YUBINN", "JYUSY1", "JYUSY2", "JYKANA" });
        }
    }
}
