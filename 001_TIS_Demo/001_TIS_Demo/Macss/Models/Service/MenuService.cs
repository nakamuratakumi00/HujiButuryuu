using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;
using Macss.Repositories;
using Macss.ViewModels;

namespace Macss.Service
{
    public static class MenuService
    {
        private const string NoneParent = "MAIN";

        /// <summary>
        ///  階層別のメニュー一覧を作成する
        /// </summary>
        /// <param name="menuList">メニュー一覧</param>
        /// <param name="menuRole">対象Roleの表示対象一覧</param>
        /// <returns></returns>
        internal static List<MenuViewModels> GetMenuListAsync(List<MenuViewModels> menuList, List<string> menuRole)
        {
            // 表示対象を一覧化
            var dispMenu = new List<MenuViewModels>();
            foreach (string menuId in menuRole)
            {
                var dispRow = menuList.SingleOrDefault(x => x.MenuId == menuId);

                if (dispRow != null)
                {
                    AddDispMenu(menuList, dispRow, dispMenu);
                }
            }

            dispMenu.OrderBy(x => x.MenuSort);

            var notDisp = dispMenu.Where(x => x.MenuKbn == 0).ToList();
            dispMenu = dispMenu.Where(x => x.MenuKbn == 1).ToList();

            // 親毎に子レコードを作成（表示用に整列））
            var parentList = dispMenu.Where(x => x.ParentId == NoneParent).ToList();
            var childList = dispMenu.Where(x => x.ParentId != NoneParent).ToList();
            var ret = new List<MenuViewModels>();
            var group = 0;

            foreach (MenuViewModels row in parentList)
            {
                var level = 0;
                var endTagCnt = 0;
                AddMenuRow(row, childList, ret, level, endTagCnt, group, false);
                group++;
            }

            ret.AddRange(notDisp);
            return ret;
        }

        /// <summary>
        /// 表示用のメニュー一覧を作成する（再帰）
        /// </summary>
        /// <param name="menuList">メニュー一覧</param>
        /// <param name="dispRow">処理行</param>
        /// <param name="dispMenu">作成する一覧</param>
        private static void AddDispMenu(List<MenuViewModels> menuList, MenuViewModels dispRow, List<MenuViewModels> dispMenu)
        {

            if (dispMenu.Any(x => x.MenuId == dispRow.MenuId))
            {
                return;
            }

            if (dispRow.ParentId == NoneParent)
            {
                dispMenu.Add(dispRow);
                return;
            }

            dispMenu.Add(dispRow);
            var parentRow = menuList.SingleOrDefault(x => x.MenuId == dispRow.ParentId);
            if (parentRow == null)
            {
                return;
            }

            AddDispMenu(menuList, parentRow, dispMenu);
        }

        /// <summary>
        /// 子の有無を確認しながらレコードを作成（再帰処理）
        /// </summary>
        /// <param name="row">レコード</param>
        /// <param name="childList">子メニュー一覧</param>
        /// <param name="ret">メニュー一覧</param>
        /// <param name="level">階層</param>
        /// <param name="endTagCnt">要素の終了タグ数</param>
        /// <param name="group">所属グループ</param>
        /// <param name="islast">ループの最後の場合はtrue</param>
        private static void AddMenuRow(MenuViewModels row, List<MenuViewModels> childList, List<MenuViewModels> ret, int level, int endTagCnt,int group, bool islast)
        {
            if (childList.Any(x => x.ParentId == row.MenuId))
            {
                // 子がいる場合、自身を追加後に子を追加（再帰）
                var thisLevel = level;
                level++;

                row.IsChild = true;
                row.level = thisLevel;
                row.Group = group;
                ret.Add(row);

                var children = childList.Where(x => x.ParentId == row.MenuId).ToList();

                foreach (MenuViewModels nextRow in children)
                {
                    // 階層の最後の場合、終了タグを作成する
                    var endFlg = false;
                    var endTag = 0;      // 初期化することで、ネストの深さを管理
                    if (children.IndexOf(nextRow) == children.Count - 1)
                    {
                        endTag = ++endTagCnt;
                        endFlg = true;
                    }
                    AddMenuRow(nextRow, childList, ret, level, endTag, group, endFlg);
                }
            }
            else
            {
                // 子がいない場合、自身を追加して終了
                row.IsChild = false;
                row.level = level;
                row.Group = group;
                if (islast)
                {
                    row.CntUlTag = endTagCnt;

                    if (row.level == endTagCnt)
                    {
                        row.IsLast = true;
                    }
                }
                ret.Add(row);
            }
        }
    }
}