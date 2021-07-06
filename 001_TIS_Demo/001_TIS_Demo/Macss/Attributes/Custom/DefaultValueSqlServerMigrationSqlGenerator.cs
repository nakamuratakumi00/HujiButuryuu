using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServer;

namespace Macss.Attributes.Custom
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DefaultAttribute : Attribute
    {
        public string Value { get; set; }
    }

    public class DefaultValueAttributeConvention : PrimitivePropertyAttributeConfigurationConvention<DefaultAttribute>
    {
        public override void Apply(ConventionPrimitivePropertyConfiguration configuration, DefaultAttribute attribute)
        {
            configuration.HasColumnAnnotation("DefaultValue", attribute.Value);
        }
    }

    internal class DefaultValueSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(AddColumnOperation addColumnOperation)
        {
            SetAnnotatedColumn(addColumnOperation.Column);
            base.Generate(addColumnOperation);
        }

        protected override void Generate(AlterColumnOperation alterColumnOperation)
        {
            SetAnnotatedColumn(alterColumnOperation.Column);
            base.Generate(alterColumnOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            SetAnnotatedColumns(createTableOperation.Columns);
            base.Generate(createTableOperation);
        }

        protected override void Generate(AlterTableOperation alterTableOperation)
        {
            SetAnnotatedColumns(alterTableOperation.Columns);
            base.Generate(alterTableOperation);
        }

        private void SetAnnotatedColumn(ColumnModel col)
        {
            AnnotationValues values;
            if (col.Annotations.TryGetValue("DefaultValue", out values))
            {
                if (values.NewValue != null)
                {
                    col.DefaultValueSql = (string)values.NewValue;
                }
                else
                {
                    col.DefaultValueSql = null;
                }
            }
        }

        private void SetAnnotatedColumns(IEnumerable<ColumnModel> columns)
        {
            foreach (var column in columns)
            {
                SetAnnotatedColumn(column);
            }
        }
    }

    public class FileRequiredAttribute : Attribute
    {
    }

    public class FileDoubleRequiredAttribute : Attribute
    {
    }

    public class FileMaxLengthAttribute : Attribute
    {
        public FileMaxLengthAttribute(int value)
        {
            this.value = value;
        }

        public int value { get; set; }
    }
}