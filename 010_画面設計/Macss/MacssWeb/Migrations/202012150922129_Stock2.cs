namespace MacssWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stock2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.dummy_t_stock", "warehouse_cd", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.dummy_t_stock", "warehouse_cd");
        }
    }
}
