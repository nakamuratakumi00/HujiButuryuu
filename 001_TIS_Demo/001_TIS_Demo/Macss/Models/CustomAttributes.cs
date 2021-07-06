using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using static Macss.Models.CustomAttributes;

namespace Macss.Models
{
    public class CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public sealed class DecimalPrecisionAttribute : Attribute
        {
            public DecimalPrecisionAttribute(byte precision, byte scale)
            {
                Precision = precision;
                Scale = scale;
            }

            public byte Precision { get; set; }
            public byte Scale { get; set; }
        }

        public class DecimalPrecisionAttributeConvention
        : PrimitivePropertyAttributeConfigurationConvention<DecimalPrecisionAttribute>
        {
            public override void Apply(ConventionPrimitivePropertyConfiguration configuration, DecimalPrecisionAttribute attribute)
            {
                configuration.HasPrecision(attribute.Precision, attribute.Scale);
            }
        }

    }



}