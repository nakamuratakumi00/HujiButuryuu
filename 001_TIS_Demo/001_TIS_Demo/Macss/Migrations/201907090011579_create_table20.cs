namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table20 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.m_hokan_rireki_bumon",
                c => new
                    {
                        MONTH = c.String(nullable: false, maxLength: 8),
                        KISYUA = c.String(nullable: false, maxLength: 2),
                        BASYO = c.String(nullable: false, maxLength: 2),
                        BMNCOD = c.String(maxLength: 4),
                        CRTCOD = c.String(maxLength: 8),
                        CRTYMD = c.DateTime(),
                        UPDCOD = c.String(maxLength: 8),
                        UPDYMD = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.MONTH, t.KISYUA, t.BASYO });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.m_hokan_rireki_bumon");
        }
    }
}
