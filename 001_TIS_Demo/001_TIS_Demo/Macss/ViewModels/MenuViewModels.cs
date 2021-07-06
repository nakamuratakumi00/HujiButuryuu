using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Macss.ViewModels
{
    public class MenuViewModels
    {
        public string MenuId { get; set; }

        public string MenuName { get; set; }
        
        public string RoleName { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public string TitleName { get; set; }

        public string ParentId { get; set; }

        public int MenuKbn { get; set; } = 0;

        public int RoleKbn { get; set; } = 0;

        public int? MenuSort { get; set; } = 0;

        /// <summary>
        /// 階層の深さを保持
        /// </summary>
        public int level { get; set; } = 0;

        /// <summary>
        /// 子要素の有無を保持
        /// </summary>
        public bool IsChild { get; set; } = false;
        
        /// <summary>
        /// 階層が終了場合、閉じる深さを保持
        /// </summary>
        public int CntUlTag { get; set; } = 0;

        /// <summary>
        /// 選択中の画面の親を判断する（サイドメニューの初期展開を管理）
        /// </summary>
        public bool IsExpansion { get; set; } = false;

        /// <summary>
        /// 選択中の画面を判断する
        /// </summary>
        public bool IsTarget { get; set; } = false;

        /// <summary>
        /// グループの最終レコードの場合はtrue
        /// </summary>
        public bool IsLast { get; set; } = false;

        /// <summary>
        /// どの親メニューに所属するかを保持
        /// </summary>
        public int Group { get; set; } = 0;

        public MenuViewModels Clone()
        {
            return (MenuViewModels)MemberwiseClone();
        }
    }
}