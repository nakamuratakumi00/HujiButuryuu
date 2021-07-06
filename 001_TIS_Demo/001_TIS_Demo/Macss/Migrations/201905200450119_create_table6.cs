namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_unsou_shuukatehai", "YUBFLG", c => c.String(nullable: false, maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_unsou_shuukatehai", "YUBFLG");
        }
    }
}
