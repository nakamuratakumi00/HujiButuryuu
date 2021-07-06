using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Macss.HtmlHelpers
{
    public static class BootstrapTableHelper
    {
        public static IHtmlString ThFor<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> exp, object htmlAttributes=null)
        {
            var property = GetProperty(exp);

            var tag = new TagBuilder("th");
            tag.Attributes.Add("data-field", property.Name);
            tag.InnerHtml = helper.DisplayNameFor(exp).ToString();

            if (htmlAttributes != null)
            {
                foreach (var attrProp in htmlAttributes.GetType().GetProperties())
                {
                    var v = attrProp.GetValue(htmlAttributes).ToString();
                    if(attrProp.PropertyType == typeof(bool) || attrProp.PropertyType == typeof(Boolean))
                    {
                        v = v.ToLower();
                    }
                    tag.Attributes.Add(attrProp.Name.Replace('_', '-'), v);
                }

                // ValueがToUpperされたりするので使わない
                // var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            }


            return MvcHtmlString.Create(tag.ToString());
        }

        private static PropertyInfo GetProperty<T>(Expression<Func<T, object>> exp)
        {
            if (exp.Body is MemberExpression)
            {
                return ((MemberExpression)exp.Body).Member as PropertyInfo;
            }
            else if (exp.Body is UnaryExpression)
            {
                var op = ((UnaryExpression)exp.Body).Operand as MemberExpression;
                if (op != null)
                {
                    return op.Member as PropertyInfo;
                }
            }
            return null;
        }
    }
}