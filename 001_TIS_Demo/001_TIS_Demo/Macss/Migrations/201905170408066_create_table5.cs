namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.m_torihikisaki", "FBTCDS", c => c.String(nullable: false, maxLength: 9));
        }
        
        public override void Down()
        {
            DropColumn("dbo.m_torihikisaki", "FBTCDS");
        }
    }
}
