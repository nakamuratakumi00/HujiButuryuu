namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table9 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.t_hokan_denpyokensu");
            AlterColumn("dbo.t_hokan_denpyokensu", "KEIYMD", c => c.DateTime());
            AlterColumn("dbo.t_hokan_denpyokensu", "INPYMD", c => c.DateTime(nullable: false));
            AddPrimaryKey("dbo.t_hokan_denpyokensu", new[] { "KISYUA", "KISYUB", "BASYO", "DENKUB", "INPYMD", "HINCOD" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.t_hokan_denpyokensu");
            AlterColumn("dbo.t_hokan_denpyokensu", "INPYMD", c => c.DateTime());
            AlterColumn("dbo.t_hokan_denpyokensu", "KEIYMD", c => c.DateTime(nullable: false));
            AddPrimaryKey("dbo.t_hokan_denpyokensu", new[] { "KISYUA", "KISYUB", "BASYO", "DENKUB", "KEIYMD", "HINCOD" });
        }
    }
}
