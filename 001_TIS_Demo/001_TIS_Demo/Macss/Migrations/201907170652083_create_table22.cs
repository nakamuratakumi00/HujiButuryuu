namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_use_status", "action_name", c => c.String(maxLength: 32));
            AddColumn("dbo.t_use_status", "controller_name", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_use_status", "controller_name");
            DropColumn("dbo.t_use_status", "action_name");
        }
    }
}
