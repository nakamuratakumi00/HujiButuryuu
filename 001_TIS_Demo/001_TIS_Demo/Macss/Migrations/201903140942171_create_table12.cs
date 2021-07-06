namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table12 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.t_unsou_shuuka_tyuumonsho_tehai_kouho", newName: "t_unsou_shuuka_tyuumonsho_tehai_k");
            RenameTable(name: "dbo.t_unsou_shuuka_tyuumonsho_tehai_meisai_kouho", newName: "t_unsou_shuuka_tyuumonsho_tehai_mk");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.t_unsou_shuuka_tyuumonsho_tehai_mk", newName: "t_unsou_shuuka_tyuumonsho_tehai_meisai_kouho");
            RenameTable(name: "dbo.t_unsou_shuuka_tyuumonsho_tehai_k", newName: "t_unsou_shuuka_tyuumonsho_tehai_kouho");
        }
    }
}
