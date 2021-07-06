namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.m_account", "sdek12", c => c.String(maxLength: 2));
            AddColumn("dbo.m_account", "ctlfl1", c => c.String(maxLength: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.m_account", "ctlfl1");
            DropColumn("dbo.m_account", "sdek12");
        }
    }
}
