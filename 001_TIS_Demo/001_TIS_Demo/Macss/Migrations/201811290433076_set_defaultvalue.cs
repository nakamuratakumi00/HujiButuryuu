namespace WebDevelopmentTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class set_defaultvalue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.m_account", "login_count", c => c.Int(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "0")
                    },
                }));
            AlterColumn("dbo.m_account", "login_failure_count", c => c.Int(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "0")
                    },
                }));
            AlterColumn("dbo.m_account", "delete_flg", c => c.Int(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "0")
                    },
                }));
            AlterColumn("dbo.m_menu", "menu_kbn", c => c.Int(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "0")
                    },
                }));
            AlterColumn("dbo.m_menu", "role_kbn", c => c.Int(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "0")
                    },
                }));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.m_menu", "role_kbn", c => c.Int(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DefaultValue",
                        new AnnotationValues(oldValue: "0", newValue: null)
                    },
                }));
            AlterColumn("dbo.m_menu", "menu_kbn", c => c.Int(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DefaultValue",
                        new AnnotationValues(oldValue: "0", newValue: null)
                    },
                }));
            AlterColumn("dbo.m_account", "delete_flg", c => c.Int(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DefaultValue",
                        new AnnotationValues(oldValue: "0", newValue: null)
                    },
                }));
            AlterColumn("dbo.m_account", "login_failure_count", c => c.Int(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DefaultValue",
                        new AnnotationValues(oldValue: "0", newValue: null)
                    },
                }));
            AlterColumn("dbo.m_account", "login_count", c => c.Int(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DefaultValue",
                        new AnnotationValues(oldValue: "0", newValue: null)
                    },
                }));
        }
    }
}
