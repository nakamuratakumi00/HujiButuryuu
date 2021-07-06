namespace MacssWeb.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Stock : DbMigration
    {
        public override void Up()
        {

            CreateTable(
                "dbo.dummy_t_stock",
                c => new
                {
                    id = c.Long(nullable: false, identity: true),
                    created_user = c.String(nullable: false),
                    created_at = c.DateTime(nullable: false),
                    updated_user = c.String(nullable: false),
                    updated_at = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.id);


        }

        public override void Down()
        {
            DropTable("dbo.dummy_t_stock");
        }
    }
}
