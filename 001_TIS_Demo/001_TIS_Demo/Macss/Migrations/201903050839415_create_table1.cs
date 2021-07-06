namespace Macss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table1 : DbMigration
    {
        public override void Up()
        {

            CreateTable(
    "dbo.t_unsou_shuuka_tyuumonsho_tehai",
    c => new
    {
        SYUKNO = c.String(nullable: false, maxLength: 20),
        CDATE = c.String(nullable: false, maxLength: 6),
        SYKYMD = c.DateTime(),
        KISYU = c.String(maxLength: 8),
        KEIFNO = c.String(maxLength: 30),
        FSYKNO = c.String(maxLength: 20),
        SYBCOD = c.String(maxLength: 2),
        TOKCOD = c.String(maxLength: 9),
        SEICOD = c.String(maxLength: 9),
        HTYNAM = c.String(maxLength: 20),
        HTYKAH = c.String(maxLength: 10),
        TANCOD = c.String(maxLength: 8),
        TANNAM = c.String(maxLength: 10),
        HTYTEL = c.String(maxLength: 20),
        BASYO = c.String(maxLength: 20),
        TDKCOD = c.String(maxLength: 15),
        TDKYUB = c.String(maxLength: 10),
        TDKJYU = c.String(maxLength: 60),
        TDBNJ1 = c.String(maxLength: 6),
        TDBNJ2 = c.String(maxLength: 20),
        TDBNJ3 = c.String(maxLength: 50),
        TDKNAM = c.String(maxLength: 20),
        TDSNAM = c.String(maxLength: 20),
        TDBNAM = c.String(maxLength: 20),
        TDKTAN = c.String(maxLength: 20),
        TDKTEL = c.String(maxLength: 20),
        DHINCOD = c.String(maxLength: 15),
        DHINNAM = c.String(maxLength: 80),
        DSYUKSU = c.Decimal(precision: 11, scale: 0),
        TKJIKO = c.String(maxLength: 40),
        COMENT = c.String(maxLength: 20),
        UNSCOD = c.String(maxLength: 2),
        UNSCRS = c.String(maxLength: 2),
        SIRCOD = c.String(maxLength: 9),
        UNSKBN = c.String(maxLength: 5),
        DENKBN = c.String(maxLength: 2),
        DENMSU = c.Int(),
        UFUTAN = c.String(maxLength: 1),
        YUSONO = c.String(maxLength: 7),
        DENF = c.String(maxLength: 1),
        HKUYMD = c.DateTime(),
        HKUCOD = c.String(maxLength: 8),
        PCCOD = c.String(maxLength: 12),
        YUBFLG = c.String(maxLength: 1),
        MUKOUKBN = c.String(maxLength: 1),
        MUKOURIYUU = c.String(maxLength: 20),
        INSCOD = c.String(maxLength: 6),
        CRTCOD = c.String(maxLength: 8),
        CRTYMD = c.DateTime(),
        UPDCOD = c.String(maxLength: 8),
        UPDYMD = c.DateTime(),
    })
    .PrimaryKey(t => new { t.SYUKNO, t.CDATE });


        }

        public override void Down()
        {
        }
    }
}
