using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Macss.Attributes.Validation
{
    public class ListRequiredAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IEnumerable;
            return list != null && list.GetEnumerator().MoveNext();
        }
    }
}