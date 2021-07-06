using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace Macss.Attributes.Validation
{
    public class TwoByteCharacterAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "全角文字を入力してください";
        }

        public override bool IsValid(object value)
        {

            if (value == null) return false;
            Encoding Enc = Encoding.GetEncoding("Shift-JIS");
            if (Enc.GetByteCount(value.ToString()) == value.ToString().Length * 2)
            {
                // 全角
                return true;
            }
            else
            {
                return false;
            }

        }
    }

}