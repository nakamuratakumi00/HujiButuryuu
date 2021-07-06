namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table8 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.t_unsou_shuuka_tyuumonsho_tehai", "TDBNJ1");
            DropColumn("dbo.t_unsou_shuuka_tyuumonsho_tehai", "TDBNJ2");
            DropColumn("dbo.t_unsou_shuuka_tyuumonsho_tehai", "TDBNJ3");
        }
        
        public override void Down()
        {
            AddColumn("dbo.t_unsou_shuuka_tyuumonsho_tehai", "TDBNJ3", c => c.String(maxLength: 50));
            AddColumn("dbo.t_unsou_shuuka_tyuumonsho_tehai", "TDBNJ2", c => c.String(maxLength: 20));
            AddColumn("dbo.t_unsou_shuuka_tyuumonsho_tehai", "TDBNJ1", c => c.String(maxLength: 6));
        }
    }
}
