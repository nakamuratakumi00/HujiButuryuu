namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table14 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.t_password_history", new[] { "account_id" });
            DropIndex("dbo.t_password_history", new[] { "account_password" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.t_password_history", "account_password");
            CreateIndex("dbo.t_password_history", "account_id");
        }
    }
}
