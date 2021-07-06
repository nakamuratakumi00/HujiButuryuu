namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_hokan_rireki_seikyu", "HOKANR", c => c.Decimal(nullable: false, precision: 9, scale: 0));
            AddColumn("dbo.t_hokan_rireki_seikyu", "NIEKIKR", c => c.Decimal(nullable: false, precision: 9, scale: 0));
            AddColumn("dbo.t_hokan_rireki_seikyu_b", "HOKANKR", c => c.Decimal(nullable: false, precision: 9, scale: 0));
            AddColumn("dbo.t_hokan_rireki_seikyu_b", "NIEKIR", c => c.Decimal(nullable: false, precision: 9, scale: 0));
            AddColumn("dbo.t_hokan_seikyu", "HOKANKR", c => c.Decimal(nullable: false, precision: 9, scale: 0));
            AddColumn("dbo.t_hokan_seikyu", "NIEKIKR", c => c.Decimal(nullable: false, precision: 9, scale: 0));
            AddColumn("dbo.t_hokan_seikyu_b", "HOKANKR", c => c.Decimal(nullable: false, precision: 9, scale: 0));
            AddColumn("dbo.t_hokan_seikyu_b", "NIEKIKR", c => c.Decimal(nullable: false, precision: 9, scale: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_hokan_seikyu_b", "NIEKIKR");
            DropColumn("dbo.t_hokan_seikyu_b", "HOKANKR");
            DropColumn("dbo.t_hokan_seikyu", "NIEKIKR");
            DropColumn("dbo.t_hokan_seikyu", "HOKANKR");
            DropColumn("dbo.t_hokan_rireki_seikyu_b", "NIEKIR");
            DropColumn("dbo.t_hokan_rireki_seikyu_b", "HOKANKR");
            DropColumn("dbo.t_hokan_rireki_seikyu", "NIEKIKR");
            DropColumn("dbo.t_hokan_rireki_seikyu", "HOKANR");
        }
    }
}
