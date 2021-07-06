namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.m_hokan_rireki_tanka", "OSYBCOD", c => c.String(maxLength: 2));
            AddColumn("dbo.m_hokan_rireki_tanka", "OHINNMK", c => c.String(maxLength: 20));
            AddColumn("dbo.m_hokan_rireki_tanka", "OKISYUA", c => c.String(maxLength: 2));
            AddColumn("dbo.m_hokan_rireki_tanka", "OKISYUB", c => c.String(maxLength: 6));
            AddColumn("dbo.m_hokan_rireki_tanka", "OFRIKAE", c => c.String(maxLength: 1));
            AddColumn("dbo.m_hokan_tanka", "OSYBCOD", c => c.String(maxLength: 2));
            AddColumn("dbo.m_hokan_tanka", "OHINNMK", c => c.String(maxLength: 20));
            AddColumn("dbo.m_hokan_tanka", "OKISYUA", c => c.String(maxLength: 2));
            AddColumn("dbo.m_hokan_tanka", "OKISYUB", c => c.String(maxLength: 6));
            AddColumn("dbo.m_hokan_tanka", "OFRIKAE", c => c.String(maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.m_hokan_tanka", "OFRIKAE");
            DropColumn("dbo.m_hokan_tanka", "OKISYUB");
            DropColumn("dbo.m_hokan_tanka", "OKISYUA");
            DropColumn("dbo.m_hokan_tanka", "OHINNMK");
            DropColumn("dbo.m_hokan_tanka", "OSYBCOD");
            DropColumn("dbo.m_hokan_rireki_tanka", "OFRIKAE");
            DropColumn("dbo.m_hokan_rireki_tanka", "OKISYUB");
            DropColumn("dbo.m_hokan_rireki_tanka", "OKISYUA");
            DropColumn("dbo.m_hokan_rireki_tanka", "OHINNMK");
            DropColumn("dbo.m_hokan_rireki_tanka", "OSYBCOD");
        }
    }
}
