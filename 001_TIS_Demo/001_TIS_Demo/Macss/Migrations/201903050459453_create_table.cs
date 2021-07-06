namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.m_unsou_jiscode",
                c => new
                    {
                        JISCOD = c.String(nullable: false, maxLength: 8),
                        JYUSY1 = c.String(nullable: false, maxLength: 10),
                        JYUSY2 = c.String(nullable: false, maxLength: 50),
                        JYUSY3 = c.String(nullable: false, maxLength: 50),
                        CRTCOD = c.String(nullable: false, maxLength: 8),
                        CRTYMD = c.DateTime(nullable: false),
                        UPDCOD = c.String(nullable: false, maxLength: 8),
                        UPDYMD = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JISCOD);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.m_unsou_jiscode");
        }
    }
}
