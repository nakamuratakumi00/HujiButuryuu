namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table21 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_use_status",
                c => new
                    {
                        session_id = c.String(nullable: false, maxLength: 100),
                        account_id = c.String(nullable: false, maxLength: 32),
                        title_name = c.String(maxLength: 128),
                        update_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.session_id, t.account_id });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.t_use_status");
        }
    }
}
