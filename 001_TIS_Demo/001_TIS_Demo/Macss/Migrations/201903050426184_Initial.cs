namespace Macss.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {

            CreateTable(
    "dbo.m_hokan_bumon",
    c => new
    {
        KISYUA = c.String(nullable: false, maxLength: 2),
        BASYO = c.String(nullable: false, maxLength: 2),
        BMNCOD = c.String(maxLength: 4),
        CRTCOD = c.String(maxLength: 8),
        CRTYMD = c.DateTime(),
        UPDCOD = c.String(maxLength: 8),
        UPDYMD = c.DateTime(),
    })
    .PrimaryKey(t => new { t.KISYUA, t.BASYO });

            CreateTable(
                "dbo.m_hokan_jouken",
                c => new
                {
                    SIKCOD = c.String(nullable: false, maxLength: 10),
                    JYOKEN = c.String(nullable: false, maxLength: 10),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.SIKCOD, t.JYOKEN });

            CreateTable(
                "dbo.m_hokan_keiyaku",
                c => new
                {
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISNAM = c.String(maxLength: 15),
                    SEITAI = c.String(maxLength: 1),
                    SEINAM = c.String(maxLength: 40),
                    TANBSY = c.String(maxLength: 15),
                    FBTCOD = c.String(maxLength: 9),
                    HOKFLG1 = c.String(maxLength: 1),
                    HOKFLG2 = c.String(maxLength: 1),
                    HOKFLG3 = c.String(maxLength: 1),
                    HOKFLG4 = c.String(maxLength: 1),
                    HOKFLG5 = c.String(maxLength: 1),
                    HNEBIR = c.Decimal(precision: 5, scale: 3),
                    HOKANT = c.Decimal(precision: 10, scale: 5),
                    NIEFLG1 = c.String(maxLength: 1),
                    NIEFLG2 = c.String(maxLength: 1),
                    NIEFLG3 = c.String(maxLength: 1),
                    NIEFLG4 = c.String(maxLength: 1),
                    NIEFLG5 = c.String(maxLength: 1),
                    NIEANT = c.Decimal(precision: 9, scale: 4),
                    NIEKYT = c.Decimal(precision: 9, scale: 4),
                    NNEBIR = c.Decimal(precision: 5, scale: 3),
                    OJYUKR = c.Decimal(precision: 5, scale: 3),
                    OSYJYR = c.Decimal(precision: 5, scale: 3),
                    HJYUKR = c.Decimal(precision: 5, scale: 3),
                    HSYJYR = c.Decimal(precision: 5, scale: 3),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => t.KISYUA);

            CreateTable(
                "dbo.m_hokan_rireki_jouken",
                c => new
                {
                    MONTH = c.String(nullable: false, maxLength: 8),
                    SIKCOD = c.String(nullable: false, maxLength: 10),
                    JYOKEN = c.String(nullable: false, maxLength: 10),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.MONTH, t.SIKCOD, t.JYOKEN });

            CreateTable(
                "dbo.m_hokan_rireki_keiyaku",
                c => new
                {
                    MONTH = c.String(nullable: false, maxLength: 8),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISNAM = c.String(maxLength: 15),
                    SEITAI = c.String(maxLength: 1),
                    SEINAM = c.String(maxLength: 40),
                    TANBSY = c.String(maxLength: 15),
                    FBTCOD = c.String(maxLength: 9),
                    HOKFLG1 = c.String(maxLength: 1),
                    HOKFLG2 = c.String(maxLength: 1),
                    HOKFLG3 = c.String(maxLength: 1),
                    HOKFLG4 = c.String(maxLength: 1),
                    HOKFLG5 = c.String(maxLength: 1),
                    HNEBIR = c.Decimal(precision: 5, scale: 3),
                    HOKANT = c.Decimal(precision: 10, scale: 5),
                    NIEFLG1 = c.String(maxLength: 1),
                    NIEFLG2 = c.String(maxLength: 1),
                    NIEFLG3 = c.String(maxLength: 1),
                    NIEFLG4 = c.String(maxLength: 1),
                    NIEFLG5 = c.String(maxLength: 1),
                    NIEANT = c.Decimal(precision: 9, scale: 4),
                    NIEKYT = c.Decimal(precision: 9, scale: 4),
                    NNEBIR = c.Decimal(precision: 5, scale: 3),
                    OJYUKR = c.Decimal(precision: 5, scale: 3),
                    OSYJYR = c.Decimal(precision: 5, scale: 3),
                    HJYUKR = c.Decimal(precision: 5, scale: 3),
                    HSYJYR = c.Decimal(precision: 5, scale: 3),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.MONTH, t.KISYUA });

            CreateTable(
                "dbo.m_hokan_rireki_seikyuusaki_change",
                c => new
                {
                    MONTH = c.String(nullable: false, maxLength: 8),
                    SEICOD = c.String(nullable: false, maxLength: 9),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    CHGCOD = c.String(maxLength: 9),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.MONTH, t.SEICOD, t.KISYUA });

            CreateTable(
                "dbo.m_hokan_rireki_tanka",
                c => new
                {
                    MONTH = c.String(nullable: false, maxLength: 8),
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    SYBCOD = c.String(maxLength: 2),
                    HINNMK = c.String(maxLength: 20),
                    KISYUA = c.String(maxLength: 2),
                    KISYUB = c.String(maxLength: 6),
                    TFULAG = c.String(maxLength: 1),
                    FRIKAE = c.String(maxLength: 1),
                    JYUKAR = c.Decimal(precision: 5, scale: 3),
                    SYUJYR = c.Decimal(precision: 5, scale: 3),
                    FPTANK = c.Decimal(precision: 10, scale: 2),
                    HOKANT = c.Decimal(precision: 10, scale: 5),
                    HOKYMD = c.DateTime(),
                    HOKTAN = c.String(maxLength: 8),
                    NIEKIT = c.Decimal(precision: 10, scale: 5),
                    NIEYMD = c.DateTime(),
                    NIETAN = c.String(maxLength: 8),
                    OFPTNK = c.Decimal(precision: 10, scale: 2),
                    OHOKAT = c.Decimal(precision: 10, scale: 5),
                    ONIEKT = c.Decimal(precision: 10, scale: 5),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.MONTH, t.HINCOD });

            CreateTable(
                "dbo.m_hokan_seihin",
                c => new
                {
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    HINNAM = c.String(maxLength: 80),
                    HINNMK = c.String(maxLength: 80),
                    KISYUA = c.String(maxLength: 2),
                    KISYUB = c.String(maxLength: 6),
                    SYBCOD = c.String(maxLength: 2),
                    FPTANK = c.Decimal(precision: 10, scale: 2),
                    MENTBI = c.DateTime(),
                    ZANTEI = c.String(maxLength: 1),
                    ZAIKSU = c.Decimal(precision: 11, scale: 2),
                    HOKCOD = c.String(maxLength: 2),
                    FRIKAE = c.String(maxLength: 1),
                    DTMOTO = c.String(maxLength: 1),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => t.HINCOD);

            CreateTable(
                "dbo.m_hokan_seikyuusaki_change",
                c => new
                {
                    SEICOD = c.String(nullable: false, maxLength: 9),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    CHGCOD = c.String(maxLength: 9),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.SEICOD, t.KISYUA });

            CreateTable(
                "dbo.m_hokan_tanka",
                c => new
                {
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    SYBCOD = c.String(maxLength: 2),
                    HINNMK = c.String(maxLength: 20),
                    KISYUA = c.String(maxLength: 2),
                    KISYUB = c.String(maxLength: 6),
                    TFULAG = c.String(maxLength: 1),
                    FRIKAE = c.String(maxLength: 1),
                    JYUKAR = c.Decimal(precision: 5, scale: 3),
                    SYUJYR = c.Decimal(precision: 5, scale: 3),
                    FPTANK = c.Decimal(precision: 10, scale: 2),
                    HOKANT = c.Decimal(precision: 10, scale: 5),
                    HOKYMD = c.DateTime(),
                    HOKTAN = c.String(maxLength: 8),
                    NIEKIT = c.Decimal(precision: 10, scale: 5),
                    NIEYMD = c.DateTime(),
                    NIETAN = c.String(maxLength: 8),
                    OFPTNK = c.Decimal(precision: 10, scale: 2),
                    OHOKAT = c.Decimal(precision: 10, scale: 5),
                    ONIEKT = c.Decimal(precision: 10, scale: 5),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => t.HINCOD);

            CreateTable(
                "dbo.t_hokan_denpyokensu",
                c => new
                {
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    BASYO = c.String(nullable: false, maxLength: 2),
                    DENKUB = c.String(nullable: false, maxLength: 2),
                    KEIYMD = c.DateTime(nullable: false),
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    DENSUU = c.Decimal(precision: 6, scale: 0),
                    SEIKYU = c.String(maxLength: 9),
                    INPYMD = c.DateTime(),
                    NSKOSU = c.Decimal(precision: 9, scale: 0),
                    EIGSOK = c.String(maxLength: 2),
                    DTMOTO = c.String(maxLength: 1),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.KISYUA, t.KISYUB, t.BASYO, t.DENKUB, t.KEIYMD, t.HINCOD });

            CreateTable(
                "dbo.t_hokan_denpyokensu_kurikosi",
                c => new
                {
                    KURYMD = c.String(nullable: false, maxLength: 8),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    BASYO = c.String(nullable: false, maxLength: 2),
                    DENKUB = c.String(nullable: false, maxLength: 2),
                    DENSUU = c.Decimal(precision: 6, scale: 0),
                    SEIKYU = c.String(maxLength: 9),
                    INPYMD = c.DateTime(),
                    KEIYMD = c.DateTime(),
                    HINCOD = c.String(maxLength: 8),
                    NSKOSU = c.Decimal(precision: 9, scale: 0),
                    EIGSOK = c.String(maxLength: 2),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.KURYMD, t.KISYUA, t.KISYUB, t.BASYO, t.DENKUB });

            CreateTable(
                "dbo.t_hokan_matujime_kanri",
                c => new
                {
                    MONTH = c.String(nullable: false, maxLength: 8),
                    STARTT = c.DateTime(),
                    ENDT = c.DateTime(),
                    STATUS = c.String(maxLength: 1),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => t.MONTH);

            CreateTable(
                "dbo.t_hokan_nyuushuuko",
                c => new
                {
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    HOKCOD = c.String(nullable: false, maxLength: 2),
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    DKBN = c.String(nullable: false, maxLength: 1),
                    ZANSUU = c.Decimal(precision: 9, scale: 0),
                    ZANKIN = c.Decimal(precision: 11, scale: 0),
                    SIKSUU = c.Decimal(precision: 9, scale: 0),
                    SIKKIN = c.Decimal(precision: 11, scale: 0),
                    KAISUU = c.Decimal(precision: 9, scale: 0),
                    KAIKIN = c.Decimal(precision: 11, scale: 0),
                    SYKSUU = c.Decimal(precision: 9, scale: 0),
                    SYKKIN = c.Decimal(precision: 11, scale: 0),
                    SEIKYU = c.String(maxLength: 9),
                    BAITAI = c.String(maxLength: 1),
                    TOUZAN = c.Decimal(precision: 9, scale: 0),
                    DTMOTO = c.String(maxLength: 1),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.KISYUA, t.KISYUB, t.HOKCOD, t.HINCOD, t.DKBN });

            CreateTable(
                "dbo.t_hokan_nyuushuuko_kurikosi",
                c => new
                {
                    KURYMD = c.String(nullable: false, maxLength: 8),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    HOKCOD = c.String(nullable: false, maxLength: 2),
                    ZANSUU = c.Decimal(precision: 9, scale: 0),
                    ZANKIN = c.Decimal(precision: 11, scale: 0),
                    SIKSUU = c.Decimal(precision: 9, scale: 0),
                    SIKKIN = c.Decimal(precision: 11, scale: 0),
                    KAISUU = c.Decimal(precision: 9, scale: 0),
                    KAIKIN = c.Decimal(precision: 11, scale: 0),
                    SYKSUU = c.Decimal(precision: 9, scale: 0),
                    SYKKIN = c.Decimal(precision: 11, scale: 0),
                    DKBN = c.String(maxLength: 1),
                    SEIKYU = c.String(maxLength: 9),
                    BAITAI = c.String(maxLength: 1),
                    TOUZAN = c.Decimal(precision: 9, scale: 0),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.KURYMD, t.KISYUA, t.KISYUB, t.HINCOD, t.HOKCOD });

            CreateTable(
                "dbo.t_hokan_rireki_denpyokensu",
                c => new
                {
                    MONTH = c.String(nullable: false, maxLength: 8),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    BASYO = c.String(nullable: false, maxLength: 2),
                    DENKUB = c.String(nullable: false, maxLength: 2),
                    KEIYMD = c.DateTime(nullable: false),
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    DENSUU = c.Decimal(precision: 6, scale: 0),
                    SEIKYU = c.String(maxLength: 9),
                    INPYMD = c.DateTime(),
                    NSKOSU = c.Decimal(precision: 9, scale: 0),
                    EIGSOK = c.String(maxLength: 2),
                    DTMOTO = c.String(maxLength: 1),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.MONTH, t.KISYUA, t.KISYUB, t.BASYO, t.DENKUB, t.KEIYMD, t.HINCOD });

            CreateTable(
                "dbo.t_hokan_rireki_denpyokensu_kurikosi",
                c => new
                {
                    MONTH = c.String(nullable: false, maxLength: 8),
                    KURYMD = c.String(nullable: false, maxLength: 8),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    BASYO = c.String(nullable: false, maxLength: 2),
                    DENKUB = c.String(nullable: false, maxLength: 2),
                    DENSUU = c.Decimal(precision: 6, scale: 0),
                    SEIKYU = c.String(maxLength: 9),
                    INPYMD = c.DateTime(),
                    KEIYMD = c.DateTime(),
                    HINCOD = c.String(maxLength: 8),
                    NSKOSU = c.Decimal(precision: 9, scale: 0),
                    EIGSOK = c.String(maxLength: 2),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.MONTH, t.KURYMD, t.KISYUA, t.KISYUB, t.BASYO, t.DENKUB });

            CreateTable(
                "dbo.t_hokan_rireki_nyuushuuko",
                c => new
                {
                    MONTH = c.String(nullable: false, maxLength: 8),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    HOKCOD = c.String(nullable: false, maxLength: 2),
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    DKBN = c.String(nullable: false, maxLength: 1),
                    ZANSUU = c.Decimal(precision: 9, scale: 0),
                    ZANKIN = c.Decimal(precision: 11, scale: 0),
                    SIKSUU = c.Decimal(precision: 9, scale: 0),
                    SIKKIN = c.Decimal(precision: 11, scale: 0),
                    KAISUU = c.Decimal(precision: 9, scale: 0),
                    KAIKIN = c.Decimal(precision: 11, scale: 0),
                    SYKSUU = c.Decimal(precision: 9, scale: 0),
                    SYKKIN = c.Decimal(precision: 11, scale: 0),
                    SEIKYU = c.String(maxLength: 9),
                    BAITAI = c.String(maxLength: 1),
                    TOUZAN = c.Decimal(precision: 9, scale: 0),
                    DTMOTO = c.String(maxLength: 1),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.MONTH, t.KISYUA, t.KISYUB, t.HOKCOD, t.HINCOD, t.DKBN });

            CreateTable(
                "dbo.t_hokan_rireki_nyuushuuko_kurikosi",
                c => new
                {
                    MONTH = c.String(nullable: false, maxLength: 8),
                    KURYMD = c.String(nullable: false, maxLength: 8),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    HOKCOD = c.String(nullable: false, maxLength: 2),
                    ZANSUU = c.Decimal(precision: 9, scale: 0),
                    ZANKIN = c.Decimal(precision: 11, scale: 0),
                    SIKSUU = c.Decimal(precision: 9, scale: 0),
                    SIKKIN = c.Decimal(precision: 11, scale: 0),
                    KAISUU = c.Decimal(precision: 9, scale: 0),
                    KAIKIN = c.Decimal(precision: 11, scale: 0),
                    SYKSUU = c.Decimal(precision: 9, scale: 0),
                    SYKKIN = c.Decimal(precision: 11, scale: 0),
                    DKBN = c.String(maxLength: 1),
                    SEIKYU = c.String(maxLength: 9),
                    BAITAI = c.String(maxLength: 1),
                    TOUZAN = c.Decimal(precision: 9, scale: 0),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.MONTH, t.KURYMD, t.KISYUA, t.KISYUB, t.HINCOD, t.HOKCOD });

            CreateTable(
                "dbo.t_hokan_rireki_seikyu",
                c => new
                {
                    MONTH = c.String(nullable: false, maxLength: 8),
                    SEICOD = c.String(nullable: false, maxLength: 9),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    KISNAM = c.String(nullable: false, maxLength: 15),
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    HOKCOD = c.String(nullable: false, maxLength: 2),
                    PCCOD = c.String(nullable: false, maxLength: 12),
                    HINNMK = c.String(nullable: false, maxLength: 20),
                    ZANSUU = c.Decimal(nullable: false, precision: 10, scale: 0),
                    NYUKSU = c.Decimal(nullable: false, precision: 10, scale: 0),
                    SYKSUU = c.Decimal(nullable: false, precision: 10, scale: 0),
                    SEKISU = c.Decimal(nullable: false, precision: 10, scale: 2),
                    HOKANT = c.Decimal(nullable: false, precision: 10, scale: 5),
                    HOKANK = c.Decimal(nullable: false, precision: 9, scale: 0),
                    ATUKAI = c.Decimal(nullable: false, precision: 10, scale: 0),
                    DENSUU = c.Decimal(nullable: false, precision: 8, scale: 0),
                    NIEKIT = c.Decimal(nullable: false, precision: 9, scale: 4),
                    NIEKIK = c.Decimal(nullable: false, precision: 9, scale: 0),
                    HOKFLG1 = c.String(nullable: false, maxLength: 1),
                    HOKFLG2 = c.String(nullable: false, maxLength: 1),
                    HOKFLG3 = c.String(nullable: false, maxLength: 1),
                    HOKFLG4 = c.String(nullable: false, maxLength: 1),
                    HOKFLG5 = c.String(nullable: false, maxLength: 1),
                    NIEFLG1 = c.String(nullable: false, maxLength: 1),
                    NIEFLG2 = c.String(nullable: false, maxLength: 1),
                    NIEFLG3 = c.String(nullable: false, maxLength: 1),
                    NIEFLG4 = c.String(nullable: false, maxLength: 1),
                    NIEFLG5 = c.String(nullable: false, maxLength: 1),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.MONTH, t.SEICOD, t.KISYUA, t.KISYUB, t.KISNAM, t.HINCOD, t.HOKCOD, t.PCCOD });

            CreateTable(
                "dbo.t_hokan_rireki_seikyu_b",
                c => new
                {
                    MONTH = c.String(nullable: false, maxLength: 8),
                    SEICOD = c.String(nullable: false, maxLength: 9),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    KISNAM = c.String(nullable: false, maxLength: 15),
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    HOKCOD = c.String(nullable: false, maxLength: 2),
                    PCCOD = c.String(nullable: false, maxLength: 12),
                    HINNMK = c.String(nullable: false, maxLength: 20),
                    ZANSUU = c.Decimal(nullable: false, precision: 10, scale: 0),
                    NYUKSU = c.Decimal(nullable: false, precision: 10, scale: 0),
                    SYKSUU = c.Decimal(nullable: false, precision: 10, scale: 0),
                    SEKISU = c.Decimal(nullable: false, precision: 10, scale: 2),
                    HOKANT = c.Decimal(nullable: false, precision: 10, scale: 5),
                    HOKANK = c.Decimal(nullable: false, precision: 9, scale: 0),
                    ATUKAI = c.Decimal(nullable: false, precision: 10, scale: 0),
                    DENSUU = c.Decimal(nullable: false, precision: 8, scale: 0),
                    NIEKIT = c.Decimal(nullable: false, precision: 9, scale: 4),
                    NIEKIK = c.Decimal(nullable: false, precision: 9, scale: 0),
                    HOKFLG1 = c.String(nullable: false, maxLength: 1),
                    HOKFLG2 = c.String(nullable: false, maxLength: 1),
                    HOKFLG3 = c.String(nullable: false, maxLength: 1),
                    HOKFLG4 = c.String(nullable: false, maxLength: 1),
                    HOKFLG5 = c.String(nullable: false, maxLength: 1),
                    NIEFLG1 = c.String(nullable: false, maxLength: 1),
                    NIEFLG2 = c.String(nullable: false, maxLength: 1),
                    NIEFLG3 = c.String(nullable: false, maxLength: 1),
                    NIEFLG4 = c.String(nullable: false, maxLength: 1),
                    NIEFLG5 = c.String(nullable: false, maxLength: 1),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.MONTH, t.SEICOD, t.KISYUA, t.KISYUB, t.KISNAM, t.HINCOD, t.HOKCOD, t.PCCOD });

            CreateTable(
                "dbo.t_hokan_rireki_seikyu_kyoten",
                c => new
                {
                    MONTH = c.String(nullable: false, maxLength: 8),
                    BASYO = c.String(nullable: false, maxLength: 2),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    BASNAM = c.String(maxLength: 20),
                    KISYBN = c.String(maxLength: 3),
                    SEICOD = c.String(maxLength: 9),
                    ZANSUU = c.Decimal(precision: 12, scale: 0),
                    ZANKIN = c.Decimal(precision: 12, scale: 0),
                    SIKSUU = c.Decimal(precision: 12, scale: 0),
                    SIKKIN = c.Decimal(precision: 12, scale: 0),
                    KAISUU = c.Decimal(precision: 12, scale: 0),
                    KAIKIN = c.Decimal(precision: 12, scale: 0),
                    NYUKSU = c.Decimal(precision: 12, scale: 0),
                    NYUKIN = c.Decimal(precision: 12, scale: 0),
                    SYKSUU = c.Decimal(precision: 12, scale: 0),
                    SYKKIN = c.Decimal(precision: 12, scale: 0),
                    ZAIKSU = c.Decimal(precision: 12, scale: 0),
                    ZAIKIN = c.Decimal(precision: 12, scale: 0),
                    DENSUU = c.Decimal(precision: 6, scale: 0),
                    DENSKY = c.Decimal(precision: 6, scale: 0),
                    HOKANK = c.Decimal(precision: 9, scale: 0),
                    NIEKIK = c.Decimal(precision: 9, scale: 0),
                    NIEKYJ = c.Decimal(precision: 9, scale: 0),
                    PCCODH = c.String(maxLength: 12),
                    PCCODN = c.String(maxLength: 12),
                    DATAYM = c.String(maxLength: 4),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.MONTH, t.BASYO, t.KISYUA, t.KISYUB });

            CreateTable(
                "dbo.t_hokan_seikyu",
                c => new
                {
                    SEICOD = c.String(nullable: false, maxLength: 9),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    KISNAM = c.String(nullable: false, maxLength: 15),
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    HOKCOD = c.String(nullable: false, maxLength: 2),
                    PCCOD = c.String(nullable: false, maxLength: 12),
                    HINNMK = c.String(nullable: false, maxLength: 20),
                    ZANSUU = c.Decimal(nullable: false, precision: 10, scale: 0),
                    NYUKSU = c.Decimal(nullable: false, precision: 10, scale: 0),
                    SYKSUU = c.Decimal(nullable: false, precision: 10, scale: 0),
                    SEKISU = c.Decimal(nullable: false, precision: 10, scale: 2),
                    HOKANT = c.Decimal(nullable: false, precision: 10, scale: 5),
                    HOKANK = c.Decimal(nullable: false, precision: 9, scale: 0),
                    ATUKAI = c.Decimal(nullable: false, precision: 10, scale: 0),
                    DENSUU = c.Decimal(nullable: false, precision: 8, scale: 0),
                    NIEKIT = c.Decimal(nullable: false, precision: 9, scale: 4),
                    NIEKIK = c.Decimal(nullable: false, precision: 9, scale: 0),
                    HOKFLG1 = c.String(nullable: false, maxLength: 1),
                    HOKFLG2 = c.String(nullable: false, maxLength: 1),
                    HOKFLG3 = c.String(nullable: false, maxLength: 1),
                    HOKFLG4 = c.String(nullable: false, maxLength: 1),
                    HOKFLG5 = c.String(nullable: false, maxLength: 1),
                    NIEFLG1 = c.String(nullable: false, maxLength: 1),
                    NIEFLG2 = c.String(nullable: false, maxLength: 1),
                    NIEFLG3 = c.String(nullable: false, maxLength: 1),
                    NIEFLG4 = c.String(nullable: false, maxLength: 1),
                    NIEFLG5 = c.String(nullable: false, maxLength: 1),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.SEICOD, t.KISYUA, t.KISYUB, t.KISNAM, t.HINCOD, t.HOKCOD, t.PCCOD });

            CreateTable(
                "dbo.t_hokan_seikyu_b",
                c => new
                {
                    SEICOD = c.String(nullable: false, maxLength: 9),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    KISNAM = c.String(nullable: false, maxLength: 15),
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    HOKCOD = c.String(nullable: false, maxLength: 2),
                    PCCOD = c.String(nullable: false, maxLength: 12),
                    HINNMK = c.String(nullable: false, maxLength: 20),
                    ZANSUU = c.Decimal(nullable: false, precision: 10, scale: 0),
                    NYUKSU = c.Decimal(nullable: false, precision: 10, scale: 0),
                    SYKSUU = c.Decimal(nullable: false, precision: 10, scale: 0),
                    SEKISU = c.Decimal(nullable: false, precision: 10, scale: 2),
                    HOKANT = c.Decimal(nullable: false, precision: 10, scale: 5),
                    HOKANK = c.Decimal(nullable: false, precision: 9, scale: 0),
                    ATUKAI = c.Decimal(nullable: false, precision: 10, scale: 0),
                    DENSUU = c.Decimal(nullable: false, precision: 8, scale: 0),
                    NIEKIT = c.Decimal(nullable: false, precision: 9, scale: 4),
                    NIEKIK = c.Decimal(nullable: false, precision: 9, scale: 0),
                    HOKFLG1 = c.String(nullable: false, maxLength: 1),
                    HOKFLG2 = c.String(nullable: false, maxLength: 1),
                    HOKFLG3 = c.String(nullable: false, maxLength: 1),
                    HOKFLG4 = c.String(nullable: false, maxLength: 1),
                    HOKFLG5 = c.String(nullable: false, maxLength: 1),
                    NIEFLG1 = c.String(nullable: false, maxLength: 1),
                    NIEFLG2 = c.String(nullable: false, maxLength: 1),
                    NIEFLG3 = c.String(nullable: false, maxLength: 1),
                    NIEFLG4 = c.String(nullable: false, maxLength: 1),
                    NIEFLG5 = c.String(nullable: false, maxLength: 1),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.SEICOD, t.KISYUA, t.KISYUB, t.KISNAM, t.HINCOD, t.HOKCOD, t.PCCOD });

            CreateTable(
                "dbo.t_hokan_seikyu_kyoten",
                c => new
                {
                    BASYO = c.String(nullable: false, maxLength: 2),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 3),
                    BASNAM = c.String(maxLength: 20),
                    KISYBN = c.String(maxLength: 3),
                    SEICOD = c.String(maxLength: 9),
                    ZANSUU = c.Decimal(precision: 12, scale: 0),
                    ZANKIN = c.Decimal(precision: 12, scale: 0),
                    SIKSUU = c.Decimal(precision: 12, scale: 0),
                    SIKKIN = c.Decimal(precision: 12, scale: 0),
                    KAISUU = c.Decimal(precision: 12, scale: 0),
                    KAIKIN = c.Decimal(precision: 12, scale: 0),
                    NYUKSU = c.Decimal(precision: 12, scale: 0),
                    NYUKIN = c.Decimal(precision: 12, scale: 0),
                    SYKSUU = c.Decimal(precision: 12, scale: 0),
                    SYKKIN = c.Decimal(precision: 12, scale: 0),
                    ZAIKSU = c.Decimal(precision: 12, scale: 0),
                    ZAIKIN = c.Decimal(precision: 12, scale: 0),
                    DENSUU = c.Decimal(precision: 6, scale: 0),
                    DENSKY = c.Decimal(precision: 6, scale: 0),
                    HOKANK = c.Decimal(precision: 9, scale: 0),
                    NIEKIK = c.Decimal(precision: 9, scale: 0),
                    NIEKYJ = c.Decimal(precision: 9, scale: 0),
                    PCCODH = c.String(maxLength: 12),
                    PCCODN = c.String(maxLength: 12),
                    DATAYM = c.String(maxLength: 4),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.BASYO, t.KISYUA, t.KISYUB });

            CreateTable(
                "dbo.w_hokan_denpyokensu",
                c => new
                {
                    ID = c.Decimal(nullable: false, precision: 11, scale: 0),
                    KISYUA = c.String(maxLength: 2),
                    KISYUB = c.String(maxLength: 3),
                    BASYO = c.String(maxLength: 2),
                    DENKUB = c.String(maxLength: 2),
                    DENSUU = c.Decimal(precision: 6, scale: 0),
                    SEIKYU = c.String(maxLength: 9),
                    INPYMD = c.DateTime(),
                    KEIYMD = c.DateTime(),
                    HINCOD = c.String(maxLength: 8),
                    NSKOSU = c.Decimal(precision: 9, scale: 0),
                    EIGSOK = c.String(maxLength: 2),
                    DTMOTO = c.String(maxLength: 1),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.w_hokan_nyuushuuko",
                c => new
                {
                    ID = c.Decimal(nullable: false, precision: 11, scale: 0),
                    KISYUA = c.String(maxLength: 2),
                    KISYUB = c.String(maxLength: 3),
                    HOKCOD = c.String(maxLength: 2),
                    HINCOD = c.String(maxLength: 8),
                    ZANSUU = c.Decimal(precision: 9, scale: 0),
                    ZANKIN = c.Decimal(precision: 11, scale: 0),
                    SIKSUU = c.Decimal(precision: 9, scale: 0),
                    SIKKIN = c.Decimal(precision: 11, scale: 0),
                    KAISUU = c.Decimal(precision: 9, scale: 0),
                    KAIKIN = c.Decimal(precision: 11, scale: 0),
                    SYKSUU = c.Decimal(precision: 9, scale: 0),
                    SYKKIN = c.Decimal(precision: 11, scale: 0),
                    DKBN = c.String(maxLength: 1),
                    SEIKYU = c.String(maxLength: 9),
                    BAITAI = c.String(maxLength: 1),
                    TOUZAN = c.Decimal(precision: 9, scale: 0),
                    DTMOTO = c.String(maxLength: 1),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.w_hokan_seihin",
                c => new
                {
                    ID = c.Decimal(nullable: false, precision: 11, scale: 0),
                    HINCOD = c.String(nullable: false, maxLength: 8),
                    HINNAM = c.String(maxLength: 80),
                    HINNMK = c.String(maxLength: 80),
                    KISYUA = c.String(maxLength: 2),
                    KISYUB = c.String(maxLength: 6),
                    SYBCOD = c.String(maxLength: 2),
                    FPTANK = c.Decimal(precision: 10, scale: 2),
                    MENTBI = c.DateTime(),
                    ZANTEI = c.String(maxLength: 1),
                    ZAIKSU = c.Decimal(precision: 11, scale: 2),
                    HOKCOD = c.String(maxLength: 2),
                    FRIKAE = c.String(maxLength: 1),
                    DTMOTO = c.String(maxLength: 1),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.ID, t.HINCOD });

            CreateTable(
                "dbo.m_unsou_hinmei",
                c => new
                {
                    HINCOD = c.String(nullable: false, maxLength: 15),
                    HINNAM = c.String(nullable: false, maxLength: 80),
                    HINNMK = c.String(nullable: false, maxLength: 80),
                    TORCOD = c.String(nullable: false, maxLength: 9),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 6),
                    DTMOTO = c.String(nullable: false, maxLength: 1),
                    TKCOD = c.String(nullable: false, maxLength: 5),
                    SYUBTU = c.String(nullable: false, maxLength: 2),
                    CTLFL1 = c.String(nullable: false, maxLength: 2),
                    KHINCD = c.String(nullable: false, maxLength: 15),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.HINCOD);

            CreateTable(
                "dbo.m_unsou_hinmei_koyuu",
                c => new
                {
                    HINCOD = c.String(nullable: false, maxLength: 15),
                    HINNAM = c.String(nullable: false, maxLength: 80),
                    HINNMK = c.String(nullable: false, maxLength: 80),
                    TORCOD = c.String(nullable: false, maxLength: 9),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYUB = c.String(nullable: false, maxLength: 6),
                    DTMOTO = c.String(nullable: false, maxLength: 1),
                    TKCOD = c.String(nullable: false, maxLength: 5),
                    SYUBTU = c.String(nullable: false, maxLength: 2),
                    CTLFL1 = c.String(nullable: false, maxLength: 2),
                    KHINCD = c.String(nullable: false, maxLength: 15),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.HINCOD);

            CreateTable(
                "dbo.m_unsou_postalcode",
                c => new
                {
                    YUBINN = c.String(nullable: false, maxLength: 8),
                    JYUSY1 = c.String(nullable: false, maxLength: 10),
                    JYUSY2 = c.String(nullable: false, maxLength: 40),
                    JYUSY3 = c.String(nullable: false, maxLength: 40),
                    JYKANA = c.String(nullable: false, maxLength: 50),
                    JISCOD = c.String(nullable: false, maxLength: 8),
                    KENJY1 = c.String(nullable: false, maxLength: 10),
                    KENJY2 = c.String(nullable: false, maxLength: 40),
                    KENJY3 = c.String(nullable: false, maxLength: 40),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.YUBINN, t.JYUSY1, t.JYUSY2 });

            CreateTable(
                "dbo.m_unsou_shuuka_tyuumonsho_pattern",
                c => new
                {
                    SYKNO2 = c.String(nullable: false, maxLength: 3),
                    INSCOD = c.String(nullable: false, maxLength: 6),
                    HAINAM = c.String(maxLength: 20),
                    MAISUU = c.Int(),
                    NOPRTF = c.String(maxLength: 1),
                    KIGENF = c.String(maxLength: 1),
                    HTYNAM = c.String(maxLength: 20),
                    KTYKHO = c.String(maxLength: 10),
                    TANCOD = c.String(maxLength: 8),
                    TANNAM = c.String(maxLength: 10),
                    KTYTEL = c.String(maxLength: 20),
                    BASYO = c.String(maxLength: 20),
                    UFUTAN = c.String(maxLength: 1),
                    KEIFNO = c.String(maxLength: 30),
                    TDKCOD = c.String(maxLength: 15),
                    TDKTEL = c.String(maxLength: 20),
                    JDKJYU = c.String(maxLength: 60),
                    TDKNAM = c.String(maxLength: 20),
                    TDSNAM = c.String(maxLength: 20),
                    TDBNAM = c.String(maxLength: 20),
                    TDKTAN = c.String(maxLength: 20),
                    HINCD1 = c.String(maxLength: 15),
                    HINCD2 = c.String(maxLength: 15),
                    HINCD3 = c.String(maxLength: 15),
                    HINCD4 = c.String(maxLength: 15),
                    HINCD5 = c.String(maxLength: 15),
                    HINNM1 = c.String(maxLength: 80),
                    HINNM2 = c.String(maxLength: 80),
                    HINNM3 = c.String(maxLength: 80),
                    HINNM4 = c.String(maxLength: 80),
                    HINNM5 = c.String(maxLength: 80),
                    SYKSU1 = c.Decimal(precision: 11, scale: 0),
                    SYKSU2 = c.Decimal(precision: 11, scale: 0),
                    SYKSU3 = c.Decimal(precision: 11, scale: 0),
                    SYKSU4 = c.Decimal(precision: 11, scale: 0),
                    SYKSU5 = c.Decimal(precision: 11, scale: 0),
                    TKJIKO = c.String(maxLength: 40),
                    TOKCOD = c.String(maxLength: 9),
                    UNSCOD = c.String(maxLength: 4),
                    UNSCRS = c.String(maxLength: 2),
                    SYBCOD = c.String(maxLength: 2),
                    TDKYUB = c.String(maxLength: 8),
                    DENKBN = c.String(maxLength: 2),
                    LSTFLG = c.String(maxLength: 1),
                    TDKHEN = c.String(maxLength: 1),
                    IRIMOT = c.String(maxLength: 13),
                    IRITEL = c.String(maxLength: 20),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.SYKNO2, t.INSCOD });

            CreateTable(
                "dbo.m_unsou_todokesaki",
                c => new
                {
                    TDKCOD = c.String(nullable: false, maxLength: 15),
                    TDKNAM = c.String(nullable: false, maxLength: 20),
                    TDKNMS = c.String(nullable: false, maxLength: 20),
                    TDKNMK = c.String(nullable: false, maxLength: 80),
                    TDBNAM = c.String(nullable: false, maxLength: 20),
                    TDKTAN = c.String(nullable: false, maxLength: 20),
                    JYUSYO = c.String(nullable: false, maxLength: 60),
                    TDKTEL = c.String(nullable: false, maxLength: 20),
                    SDEK01 = c.String(nullable: false, maxLength: 1),
                    SDEK02 = c.String(nullable: false, maxLength: 1),
                    SDEK03 = c.String(nullable: false, maxLength: 1),
                    SDEK04 = c.String(nullable: false, maxLength: 1),
                    SDEK05 = c.String(nullable: false, maxLength: 1),
                    SDEK06 = c.String(nullable: false, maxLength: 1),
                    SDEK07 = c.String(nullable: false, maxLength: 1),
                    SDEK08 = c.String(nullable: false, maxLength: 1),
                    SDEK09 = c.String(nullable: false, maxLength: 1),
                    SDEK10 = c.String(nullable: false, maxLength: 1),
                    SDEK11 = c.String(nullable: false, maxLength: 1),
                    SDEK12 = c.String(nullable: false, maxLength: 1),
                    SDEK13 = c.String(nullable: false, maxLength: 1),
                    SDEK14 = c.String(nullable: false, maxLength: 1),
                    SDEK15 = c.String(nullable: false, maxLength: 1),
                    TKJIKO = c.String(nullable: false, maxLength: 40),
                    DTMOTO = c.String(nullable: false, maxLength: 1),
                    YUBINN = c.String(nullable: false, maxLength: 10),
                    FAXNO = c.String(nullable: false, maxLength: 20),
                    KTDKCD = c.String(nullable: false, maxLength: 15),
                    KITDCD = c.String(nullable: false, maxLength: 15),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.TDKCOD);

            CreateTable(
                "dbo.m_unsou_todokesaki_koyuu",
                c => new
                {
                    TDKCOD = c.String(nullable: false, maxLength: 15),
                    TDKNAM = c.String(nullable: false, maxLength: 20),
                    TDKNMS = c.String(nullable: false, maxLength: 20),
                    TDKNMK = c.String(nullable: false, maxLength: 80),
                    TDBNAM = c.String(nullable: false, maxLength: 20),
                    TDKTAN = c.String(nullable: false, maxLength: 20),
                    JYUSYO = c.String(nullable: false, maxLength: 60),
                    TDKTEL = c.String(nullable: false, maxLength: 20),
                    SDEK01 = c.String(nullable: false, maxLength: 1),
                    SDEK02 = c.String(nullable: false, maxLength: 1),
                    SDEK03 = c.String(nullable: false, maxLength: 1),
                    SDEK04 = c.String(nullable: false, maxLength: 1),
                    SDEK05 = c.String(nullable: false, maxLength: 1),
                    SDEK06 = c.String(nullable: false, maxLength: 1),
                    SDEK07 = c.String(nullable: false, maxLength: 1),
                    SDEK08 = c.String(nullable: false, maxLength: 1),
                    SDEK09 = c.String(nullable: false, maxLength: 1),
                    SDEK10 = c.String(nullable: false, maxLength: 1),
                    SDEK11 = c.String(nullable: false, maxLength: 1),
                    SDEK12 = c.String(nullable: false, maxLength: 1),
                    SDEK13 = c.String(nullable: false, maxLength: 1),
                    SDEK14 = c.String(nullable: false, maxLength: 1),
                    SDEK15 = c.String(nullable: false, maxLength: 1),
                    TKJIKO = c.String(nullable: false, maxLength: 40),
                    DTMOTO = c.String(nullable: false, maxLength: 1),
                    YUBINN = c.String(nullable: false, maxLength: 10),
                    FAXNO = c.String(nullable: false, maxLength: 20),
                    KTDKCD = c.String(nullable: false, maxLength: 15),
                    KITDCD = c.String(nullable: false, maxLength: 15),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.TDKCOD);

            CreateTable(
                "dbo.m_unsou_unsouhouhou",
                c => new
                {
                    UNSCOD = c.String(nullable: false, maxLength: 2),
                    UNSNAM = c.String(nullable: false, maxLength: 20),
                    RYKNAM = c.String(nullable: false, maxLength: 6),
                    TORCOD = c.String(nullable: false, maxLength: 9),
                    UNSKBN = c.String(nullable: false, maxLength: 5),
                    SCTL02 = c.String(nullable: false, maxLength: 1),
                    SCTL03 = c.String(nullable: false, maxLength: 1),
                    SCTL06 = c.String(nullable: false, maxLength: 1),
                    SCTL08 = c.String(nullable: false, maxLength: 1),
                    PCTL05 = c.String(nullable: false, maxLength: 1),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.UNSCOD);

            CreateTable(
                "dbo.m_unsou_unsoukousu",
                c => new
                {
                    UNSCRS = c.String(nullable: false, maxLength: 2),
                    CRSNAM = c.String(nullable: false, maxLength: 20),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.UNSCRS);

            CreateTable(
                "dbo.m_unsou_unsoukubun",
                c => new
                {
                    UNSKBN = c.String(nullable: false, maxLength: 5),
                    UNSNAM = c.String(nullable: false, maxLength: 20),
                    JGYKBN = c.String(nullable: false, maxLength: 4),
                    JGYKB2 = c.String(nullable: false, maxLength: 4),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.UNSKBN);

            CreateTable(
                "dbo.t_unsou_shuuka_jiseki1",
                c => new
                {
                    SYUKNO = c.String(nullable: false, maxLength: 20),
                    DATAYM = c.String(nullable: false, maxLength: 4),
                    SYKYMD = c.DateTime(nullable: false),
                    KISYU = c.String(nullable: false, maxLength: 8),
                    KEIFNO = c.String(nullable: false, maxLength: 30),
                    FSYKNO = c.String(nullable: false, maxLength: 20),
                    TANCOD = c.String(nullable: false, maxLength: 8),
                    TANNAM = c.String(nullable: false, maxLength: 10),
                    TDKCOD = c.String(nullable: false, maxLength: 15),
                    TDKJYU = c.String(nullable: false, maxLength: 60),
                    TDKNAM = c.String(nullable: false, maxLength: 20),
                    TDSNAM = c.String(nullable: false, maxLength: 20),
                    TDBNAM = c.String(nullable: false, maxLength: 20),
                    TDKTAN = c.String(nullable: false, maxLength: 20),
                    TDKTEL = c.String(nullable: false, maxLength: 20),
                    TDKYUB = c.String(nullable: false, maxLength: 8),
                    HINCOD = c.String(nullable: false, maxLength: 15),
                    HINNAM = c.String(nullable: false, maxLength: 80),
                    UNSCOD = c.String(nullable: false, maxLength: 2),
                    UNSCRS = c.String(nullable: false, maxLength: 2),
                    SIRCOD = c.String(nullable: false, maxLength: 9),
                    SGENNO = c.String(nullable: false, maxLength: 20),
                    UNSKBN = c.String(nullable: false, maxLength: 5),
                    SYBCOD = c.String(nullable: false, maxLength: 2),
                    TOKCOD = c.String(nullable: false, maxLength: 9),
                    SEICOD = c.String(nullable: false, maxLength: 9),
                    DENKBN = c.String(nullable: false, maxLength: 2),
                    DENMSU = c.Int(nullable: false),
                    TKJIKO = c.String(nullable: false, maxLength: 40),
                    HOSOSU = c.Decimal(nullable: false, precision: 7, scale: 0),
                    NFDATE = c.DateTime(nullable: false),
                    DAIHNO = c.String(nullable: false, maxLength: 20),
                    DAIHNOYM = c.String(nullable: false, maxLength: 4),
                    JYURYO = c.Decimal(nullable: false, precision: 9, scale: 2),
                    HOSOS3 = c.Decimal(nullable: false, precision: 9, scale: 0),
                    JYURY3 = c.Decimal(nullable: false, precision: 9, scale: 2),
                    UFUTAN = c.String(nullable: false, maxLength: 1),
                    YUSONO = c.String(nullable: false, maxLength: 7),
                    CTLF19 = c.String(nullable: false, maxLength: 1),
                    CTLF28 = c.String(nullable: false, maxLength: 1),
                    CTLF29 = c.String(nullable: false, maxLength: 1),
                    MEHKBN = c.String(nullable: false, maxLength: 1),
                    JSKKBN = c.String(nullable: false, maxLength: 1),
                    TOCYMD = c.DateTime(nullable: false),
                    TAKSIZ = c.String(nullable: false, maxLength: 1),
                    SEIKYU = c.String(nullable: false, maxLength: 1),
                    GEPPOU = c.String(nullable: false, maxLength: 1),
                    PCCOD = c.String(nullable: false, maxLength: 12),
                    SGENN2 = c.String(nullable: false, maxLength: 20),
                    STOUCD = c.String(nullable: false, maxLength: 10),
                    KENCOD = c.String(nullable: false, maxLength: 2),
                    JISCOD = c.String(nullable: false, maxLength: 8),
                    TENSIR = c.String(nullable: false, maxLength: 9),
                    HATCOD = c.String(nullable: false, maxLength: 8),
                    SGEN35 = c.String(nullable: false, maxLength: 20),
                    ECOFLG = c.String(nullable: false, maxLength: 1),
                    SYUKSU = c.Decimal(nullable: false, precision: 11, scale: 0),
                    SYUTUF = c.String(nullable: false, maxLength: 1),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.SYUKNO, t.DATAYM });

            CreateTable(
                "dbo.t_unsou_shuuka_jiseki2",
                c => new
                {
                    SYUKNO = c.String(nullable: false, maxLength: 20),
                    DATAYM = c.String(nullable: false, maxLength: 4),
                    SYKYMD = c.DateTime(nullable: false),
                    KISYU = c.String(nullable: false, maxLength: 8),
                    KEIFNO = c.String(nullable: false, maxLength: 30),
                    FSYKNO = c.String(nullable: false, maxLength: 20),
                    TANCOD = c.String(nullable: false, maxLength: 8),
                    TANNAM = c.String(nullable: false, maxLength: 10),
                    TDKCOD = c.String(nullable: false, maxLength: 15),
                    TDKJYU = c.String(nullable: false, maxLength: 60),
                    TDKNAM = c.String(nullable: false, maxLength: 20),
                    TDSNAM = c.String(nullable: false, maxLength: 20),
                    TDBNAM = c.String(nullable: false, maxLength: 20),
                    TDKTAN = c.String(nullable: false, maxLength: 20),
                    TDKTEL = c.String(nullable: false, maxLength: 20),
                    TDKYUB = c.String(nullable: false, maxLength: 8),
                    HINCOD = c.String(nullable: false, maxLength: 15),
                    HINNAM = c.String(nullable: false, maxLength: 80),
                    UNSCOD = c.String(nullable: false, maxLength: 2),
                    UNSCRS = c.String(nullable: false, maxLength: 2),
                    SIRCOD = c.String(nullable: false, maxLength: 9),
                    SGENNO = c.String(nullable: false, maxLength: 20),
                    UNSKBN = c.String(nullable: false, maxLength: 5),
                    SYBCOD = c.String(nullable: false, maxLength: 2),
                    TOKCOD = c.String(nullable: false, maxLength: 9),
                    SEICOD = c.String(nullable: false, maxLength: 9),
                    DENKBN = c.String(nullable: false, maxLength: 2),
                    DENMSU = c.Int(nullable: false),
                    TKJIKO = c.String(nullable: false, maxLength: 40),
                    HOSOSU = c.Decimal(nullable: false, precision: 7, scale: 0),
                    NFDATE = c.DateTime(nullable: false),
                    DAIHNO = c.String(nullable: false, maxLength: 20),
                    DAIHNOYM = c.String(nullable: false, maxLength: 4),
                    JYURYO = c.Decimal(nullable: false, precision: 9, scale: 2),
                    HOSOS3 = c.Decimal(nullable: false, precision: 9, scale: 0),
                    JYURY3 = c.Decimal(nullable: false, precision: 9, scale: 2),
                    UFUTAN = c.String(nullable: false, maxLength: 1),
                    YUSONO = c.String(nullable: false, maxLength: 7),
                    CTLF19 = c.String(nullable: false, maxLength: 1),
                    CTLF28 = c.String(nullable: false, maxLength: 1),
                    CTLF29 = c.String(nullable: false, maxLength: 1),
                    MEHKBN = c.String(nullable: false, maxLength: 1),
                    JSKKBN = c.String(nullable: false, maxLength: 1),
                    TOCYMD = c.DateTime(nullable: false),
                    TAKSIZ = c.String(nullable: false, maxLength: 1),
                    SEIKYU = c.String(nullable: false, maxLength: 1),
                    GEPPOU = c.String(nullable: false, maxLength: 1),
                    PCCOD = c.String(nullable: false, maxLength: 12),
                    SGENN2 = c.String(nullable: false, maxLength: 20),
                    STOUCD = c.String(nullable: false, maxLength: 10),
                    KENCOD = c.String(nullable: false, maxLength: 2),
                    JISCOD = c.String(nullable: false, maxLength: 8),
                    TENSIR = c.String(nullable: false, maxLength: 9),
                    HATCOD = c.String(nullable: false, maxLength: 8),
                    SGEN35 = c.String(nullable: false, maxLength: 20),
                    ECOFLG = c.String(nullable: false, maxLength: 1),
                    SYUKSU = c.Decimal(nullable: false, precision: 11, scale: 0),
                    SYUTUF = c.String(nullable: false, maxLength: 1),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.SYUKNO, t.DATAYM });

            CreateTable(
                "dbo.t_unsou_shuukatehai",
                c => new
                {
                    SYUKNO = c.String(nullable: false, maxLength: 20),
                    CDATE = c.String(nullable: false, maxLength: 6),
                    SYKYMD = c.DateTime(nullable: false),
                    KISYU = c.String(nullable: false, maxLength: 8),
                    KEIFNO = c.String(nullable: false, maxLength: 30),
                    FSYKNO = c.String(nullable: false, maxLength: 20),
                    TANCOD = c.String(nullable: false, maxLength: 8),
                    TANNAM = c.String(nullable: false, maxLength: 10),
                    TDKCOD = c.String(nullable: false, maxLength: 15),
                    TDKYUB = c.String(nullable: false, maxLength: 10),
                    TDKJYU = c.String(nullable: false, maxLength: 60),
                    TDKNAM = c.String(nullable: false, maxLength: 20),
                    TDSNAM = c.String(nullable: false, maxLength: 20),
                    TDBNAM = c.String(nullable: false, maxLength: 20),
                    TDKTAN = c.String(nullable: false, maxLength: 20),
                    TDKTEL = c.String(nullable: false, maxLength: 20),
                    HINCOD = c.String(nullable: false, maxLength: 15),
                    HINNAM = c.String(nullable: false, maxLength: 80),
                    UNSCOD = c.String(nullable: false, maxLength: 2),
                    UNSCRS = c.String(nullable: false, maxLength: 2),
                    SIRCOD = c.String(nullable: false, maxLength: 9),
                    UNSKBN = c.String(nullable: false, maxLength: 5),
                    SYBCOD = c.String(nullable: false, maxLength: 2),
                    TOKCOD = c.String(nullable: false, maxLength: 9),
                    SEICOD = c.String(nullable: false, maxLength: 9),
                    DENKBN = c.String(nullable: false, maxLength: 2),
                    DENMSU = c.Int(nullable: false),
                    TKJIKO = c.String(nullable: false, maxLength: 40),
                    UFUTAN = c.String(nullable: false, maxLength: 1),
                    YUSONO = c.String(nullable: false, maxLength: 7),
                    DENF = c.String(nullable: false, maxLength: 1),
                    CTLF19 = c.String(nullable: false, maxLength: 1),
                    MEHKBN = c.String(nullable: false, maxLength: 1),
                    PCCOD = c.String(nullable: false, maxLength: 12),
                    KENCOD = c.String(nullable: false, maxLength: 2),
                    SIKCOD = c.String(nullable: false, maxLength: 4),
                    TDBNJ1 = c.String(nullable: false, maxLength: 6),
                    TDBNJ2 = c.String(nullable: false, maxLength: 20),
                    TDBNJ3 = c.String(nullable: false, maxLength: 50),
                    JISCOD = c.String(nullable: false, maxLength: 8),
                    TENSIR = c.String(nullable: false, maxLength: 9),
                    SYUKSU = c.Decimal(nullable: false, precision: 11, scale: 0),
                    MKCOD = c.String(nullable: false, maxLength: 1),
                    MKRIYU = c.String(nullable: false, maxLength: 20),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.SYUKNO, t.CDATE });


            CreateTable(
                "dbo.t_unsou_shuuka_tehai_all",
                c => new
                {
                    SYUKNO = c.String(nullable: false, maxLength: 20),
                    DATAYM = c.String(nullable: false, maxLength: 4),
                    SYKYMD = c.DateTime(nullable: false),
                    KISYU = c.String(nullable: false, maxLength: 8),
                    KEIFNO = c.String(nullable: false, maxLength: 30),
                    FSYKNO = c.String(nullable: false, maxLength: 20),
                    TANCOD = c.String(nullable: false, maxLength: 8),
                    TANNAM = c.String(nullable: false, maxLength: 10),
                    TDKCOD = c.String(nullable: false, maxLength: 15),
                    TDKJYU = c.String(nullable: false, maxLength: 60),
                    TDKNAM = c.String(nullable: false, maxLength: 20),
                    TDSNAM = c.String(nullable: false, maxLength: 20),
                    TDBNAM = c.String(nullable: false, maxLength: 20),
                    TDKTAN = c.String(nullable: false, maxLength: 20),
                    TDKTEL = c.String(nullable: false, maxLength: 20),
                    TDKYUB = c.String(nullable: false, maxLength: 8),
                    HINCOD = c.String(nullable: false, maxLength: 15),
                    HINNAM = c.String(nullable: false, maxLength: 80),
                    UNSCOD = c.String(nullable: false, maxLength: 2),
                    UNSCRS = c.String(nullable: false, maxLength: 2),
                    SIRCOD = c.String(nullable: false, maxLength: 9),
                    SGENNO = c.String(nullable: false, maxLength: 20),
                    UNSKBN = c.String(nullable: false, maxLength: 5),
                    SYBCOD = c.String(nullable: false, maxLength: 2),
                    TOKCOD = c.String(nullable: false, maxLength: 9),
                    SEICOD = c.String(nullable: false, maxLength: 9),
                    DENKBN = c.String(nullable: false, maxLength: 2),
                    DENMSU = c.Int(nullable: false),
                    TKJIKO = c.String(nullable: false, maxLength: 40),
                    HOSOSU = c.Decimal(nullable: false, precision: 7, scale: 0),
                    NFDATE = c.DateTime(nullable: false),
                    DAIHNO = c.String(nullable: false, maxLength: 20),
                    DAIHNOYM = c.String(nullable: false, maxLength: 4),
                    JYURYO = c.Decimal(nullable: false, precision: 9, scale: 2),
                    HOSOS3 = c.Decimal(nullable: false, precision: 9, scale: 0),
                    JYURY3 = c.Decimal(nullable: false, precision: 9, scale: 2),
                    UFUTAN = c.String(nullable: false, maxLength: 1),
                    YUSONO = c.String(nullable: false, maxLength: 7),
                    CTLF19 = c.String(nullable: false, maxLength: 1),
                    CTLF28 = c.String(nullable: false, maxLength: 1),
                    CTLF29 = c.String(nullable: false, maxLength: 1),
                    MEHKBN = c.String(nullable: false, maxLength: 1),
                    JSKKBN = c.String(nullable: false, maxLength: 1),
                    TOCYMD = c.DateTime(nullable: false),
                    TAKSIZ = c.String(nullable: false, maxLength: 1),
                    SEIKYU = c.String(nullable: false, maxLength: 1),
                    GEPPOU = c.String(nullable: false, maxLength: 1),
                    PCCOD = c.String(nullable: false, maxLength: 12),
                    SGENN2 = c.String(nullable: false, maxLength: 20),
                    STOUCD = c.String(nullable: false, maxLength: 10),
                    KENCOD = c.String(nullable: false, maxLength: 2),
                    JISCOD = c.String(nullable: false, maxLength: 8),
                    TENSIR = c.String(nullable: false, maxLength: 9),
                    HATCOD = c.String(nullable: false, maxLength: 8),
                    SGEN35 = c.String(nullable: false, maxLength: 20),
                    ECOFLG = c.String(nullable: false, maxLength: 1),
                    SYUKSU = c.Decimal(nullable: false, precision: 11, scale: 0),
                    SYUTUF = c.String(nullable: false, maxLength: 1),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.SYUKNO, t.DATAYM });

            CreateTable(
                "dbo.t_unsou_shuuka_tyuumonsho_tehai_meisai_kouho",
                c => new
                {
                    SYUKNO = c.String(nullable: false, maxLength: 20),
                    CDATE = c.String(nullable: false, maxLength: 6),
                    RENBAN = c.Int(nullable: false),
                    HINCOD = c.String(nullable: false, maxLength: 15),
                    HINNAM = c.String(nullable: false, maxLength: 80),
                    SYUKSU = c.Decimal(nullable: false, precision: 11, scale: 0),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.SYUKNO, t.CDATE, t.RENBAN });

            CreateTable(
                "dbo.t_unsou_shuuka_tyuumonsho_tehai_kouho",
                c => new
                {
                    SYUKNO = c.String(nullable: false, maxLength: 20),
                    CDATE = c.String(nullable: false, maxLength: 6),
                    SYKYMD = c.DateTime(nullable: false),
                    KISYU = c.String(nullable: false, maxLength: 8),
                    KEIFNO = c.String(nullable: false, maxLength: 30),
                    FSYKNO = c.String(nullable: false, maxLength: 20),
                    SYBCOD = c.String(nullable: false, maxLength: 2),
                    TOKCOD = c.String(nullable: false, maxLength: 9),
                    SEICOD = c.String(nullable: false, maxLength: 9),
                    HTYNAM = c.String(nullable: false, maxLength: 20),
                    HTYKAH = c.String(nullable: false, maxLength: 10),
                    TANCOD = c.String(nullable: false, maxLength: 8),
                    TANNAM = c.String(nullable: false, maxLength: 10),
                    HTYTEL = c.String(nullable: false, maxLength: 20),
                    BASYO = c.String(nullable: false, maxLength: 20),
                    TDKCOD = c.String(nullable: false, maxLength: 15),
                    TDKYUB = c.String(nullable: false, maxLength: 10),
                    TDKJYU = c.String(nullable: false, maxLength: 60),
                    TDKNAM = c.String(nullable: false, maxLength: 20),
                    TDSNAM = c.String(nullable: false, maxLength: 20),
                    TDBNAM = c.String(nullable: false, maxLength: 20),
                    TDKTAN = c.String(nullable: false, maxLength: 20),
                    TDKTEL = c.String(nullable: false, maxLength: 20),
                    DHINCOD = c.String(nullable: false, maxLength: 15),
                    DHINNAM = c.String(nullable: false, maxLength: 80),
                    DSYUKSU = c.Decimal(nullable: false, precision: 11, scale: 0),
                    TKJIKO = c.String(nullable: false, maxLength: 40),
                    COMENT = c.String(nullable: false, maxLength: 20),
                    UNSCOD = c.String(nullable: false, maxLength: 2),
                    UNSCRS = c.String(nullable: false, maxLength: 2),
                    SIRCOD = c.String(nullable: false, maxLength: 9),
                    UNSKBN = c.String(nullable: false, maxLength: 5),
                    DENKBN = c.String(nullable: false, maxLength: 2),
                    DENMSU = c.Int(nullable: false),
                    UFUTAN = c.String(nullable: false, maxLength: 1),
                    YUSONO = c.String(nullable: false, maxLength: 7),
                    PCCOD = c.String(nullable: false, maxLength: 12),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.SYUKNO, t.CDATE });

            CreateTable(
                "dbo.t_unsou_shuuka_tyuumonsho_tehai_meisai",
                c => new
                {
                    SYUKNO = c.String(nullable: false, maxLength: 20),
                    CDATE = c.String(nullable: false, maxLength: 6),
                    RENBAN = c.Int(nullable: false),
                    HINCOD = c.String(maxLength: 15),
                    HINNAM = c.String(maxLength: 80),
                    SYUKSU = c.Decimal(precision: 11, scale: 0),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.SYUKNO, t.CDATE, t.RENBAN });

            CreateTable(
                "dbo.w_unsou_shuuka_tyuumonsho_tehai_kouho",
                c => new
                {
                    ACTCOD = c.String(nullable: false, maxLength: 8),
                    ACKYMD = c.Decimal(nullable: false, precision: 17, scale: 0),
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
                    PCCOD = c.String(maxLength: 12),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                    TORNAM = c.String(maxLength: 40),
                })
                .PrimaryKey(t => new { t.ACTCOD, t.ACKYMD, t.SYUKNO, t.CDATE });

            CreateTable(
                "dbo.w_unsou_shuuka_tyuumonsho_tehai_meisai_kouho",
                c => new
                {
                    ACTCOD = c.String(nullable: false, maxLength: 8),
                    ACKYMD = c.Decimal(nullable: false, precision: 17, scale: 0),
                    SYUKNO = c.String(nullable: false, maxLength: 20),
                    CDATE = c.String(nullable: false, maxLength: 6),
                    RENBAN = c.Int(nullable: false),
                    HINCOD = c.String(maxLength: 15),
                    HINNAM = c.String(maxLength: 80),
                    SYUKSU = c.Decimal(precision: 11, scale: 0),
                    CRTCOD = c.String(maxLength: 8),
                    CRTYMD = c.DateTime(),
                    UPDCOD = c.String(maxLength: 8),
                    UPDYMD = c.DateTime(),
                })
                .PrimaryKey(t => new { t.ACTCOD, t.ACKYMD, t.SYUKNO, t.CDATE, t.RENBAN });

            CreateTable(
                "dbo.m_kishu",
                c => new
                {
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    KISYB1 = c.String(nullable: false, maxLength: 3),
                    KISYB2 = c.String(nullable: false, maxLength: 3),
                    KISNAM = c.String(nullable: false, maxLength: 20),
                    CTLF03 = c.String(nullable: false, maxLength: 1),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.KISYUA, t.KISYB1, t.KISYB2 });

            CreateTable(
                "dbo.m_seikyusaki",
                c => new
                {
                    SEICOD = c.String(nullable: false, maxLength: 9),
                    SEINAM = c.String(nullable: false, maxLength: 40),
                    BUKNAM = c.String(nullable: false, maxLength: 20),
                    TANNAM = c.String(nullable: false, maxLength: 10),
                    TELNO = c.String(nullable: false, maxLength: 20),
                    YUBNO = c.String(nullable: false, maxLength: 8),
                    JYSYO = c.String(nullable: false, maxLength: 60),
                    LSTFLG = c.String(nullable: false, maxLength: 1),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.SEICOD);

            CreateTable(
                "dbo.m_shukkabasho",
                c => new
                {
                    SYBCOD = c.String(nullable: false, maxLength: 2),
                    SYBNAM = c.String(nullable: false, maxLength: 20),
                    JYSYO = c.String(nullable: false, maxLength: 60),
                    SCTL05 = c.String(nullable: false, maxLength: 1),
                    SCTL07 = c.String(nullable: false, maxLength: 1),
                    SCTL09 = c.String(nullable: false, maxLength: 1),
                    SYBN01 = c.String(nullable: false, maxLength: 10),
                    SYBN02 = c.String(nullable: false, maxLength: 10),
                    SYBC15 = c.String(nullable: false, maxLength: 2),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.SYBCOD);

            CreateTable(
                "dbo.m_torihikisaki",
                c => new
                {
                    TORCOD = c.String(nullable: false, maxLength: 9),
                    TORNAM = c.String(nullable: false, maxLength: 40),
                    TORNMK = c.String(nullable: false, maxLength: 80),
                    BUKNAM = c.String(nullable: false, maxLength: 20),
                    TANNAM = c.String(nullable: false, maxLength: 10),
                    TELNO = c.String(nullable: false, maxLength: 20),
                    FAXNO = c.String(nullable: false, maxLength: 20),
                    YUBINN = c.String(nullable: false, maxLength: 8),
                    JYSYO = c.String(nullable: false, maxLength: 60),
                    SIMDAY = c.String(nullable: false, maxLength: 2),
                    SECO01 = c.String(nullable: false, maxLength: 9),
                    SECO06 = c.String(nullable: false, maxLength: 9),
                    FBTCDM = c.String(nullable: false, maxLength: 9),
                    KISYUA = c.String(nullable: false, maxLength: 2),
                    LSTFLG = c.String(nullable: false, maxLength: 1),
                    FBTCOD = c.String(nullable: false, maxLength: 9),
                    KOKCOD = c.String(nullable: false, maxLength: 4),
                    MEHK01 = c.String(nullable: false, maxLength: 1),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.TORCOD);

            CreateTable(
                "dbo.m_calendar",
                c => new
                {
                    SYBCOD = c.String(nullable: false, maxLength: 2),
                    YYMMDD = c.DateTime(nullable: false),
                    CRTCOD = c.String(nullable: false, maxLength: 8),
                    CRTYMD = c.DateTime(nullable: false),
                    UPDCOD = c.String(nullable: false, maxLength: 8),
                    UPDYMD = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.SYBCOD, t.YYMMDD });

        }

        public override void Down()
        {
        }
    }
}
