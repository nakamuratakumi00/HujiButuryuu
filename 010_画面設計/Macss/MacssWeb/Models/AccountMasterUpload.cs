using Macss.Database.Entity;
using MacssWeb.Common;
using Sic.Common.Attribute;
using System.ComponentModel;

namespace MacssWeb.Models
{
    public class AccountMasterUpload : AccountMaster
    {
        public AccountMasterUpload() : base() { }

        public AccountMasterUpload(AccountMaster obj) : this()
        {
            this.account_id = obj.account_id;
            this.account_name = obj.account_name;
            this.account_name_kana = obj.account_name_kana;
            this.account_password = obj.account_password;
            this.account_password_date = obj.account_password_date;
            this.basyo_cd = obj.basyo_cd;
            this.bumon_cd = obj.bumon_cd;
            this.create_date = obj.create_date;
            this.create_id = obj.create_id;
            this.ctlfl1 = obj.ctlfl1;
            this.delete_flg = obj.delete_flg;
            this.group_cd = obj.group_cd;
            this.last_login_date = obj.last_login_date;
            this.login_count = obj.login_count;
            this.login_failure_count = obj.login_failure_count;
            this.sdek12 = obj.sdek12;
            this.update_date = obj.update_date;
            this.update_id = obj.update_id;
            //this.m_account_role = obj.m_account_role;
        }

        [DisplayName("No.")]
        public int Index { get; set; }
        public bool HasError { get; set; }
        [DisplayName("エラー")]
        public string ErrorMessage { get; set; }
        [DisplayName("処理区分")]
        public ProcTypes ProcType { get; set; }
        public string ProcTypeText
        {
            get
            {
                return ValueAttribute.GetValue(this.ProcType);
            }
        }
    }
}