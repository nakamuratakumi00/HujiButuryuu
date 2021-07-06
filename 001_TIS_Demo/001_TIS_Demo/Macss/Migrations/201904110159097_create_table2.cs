namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.m_account", "sdek12", c => c.String(maxLength: 1));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.m_account", "sdek12", c => c.String(maxLength: 2));
        }
    }
}
