using Macss.Database.Entity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MacssWeb.Areas.Tass.Helper
{
    public class ShipmentResultsForTMSHelper
    {
        static ShipmentResultsForTMSHelper()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "すべて",
                Selected = true
            });
            foreach (DummyShipmentResults.DataTypes dataType in Enum.GetValues(typeof(DummyShipmentResults.DataTypes)))
            {
                list.Add(new SelectListItem()
                {
                    Value = ((int)dataType).ToString(),
                    Text = DummyShipmentResults.GetDataTypeName(dataType)
                });
            }

            DataTypeList = list.ToArray();
        }

        public static IEnumerable<SelectListItem> DataTypeList;
        public static IEnumerable<SelectListItem> TMSEntrySelectList =
            new SelectListItem[] {
                new SelectListItem() { Value="業務登録データ連携未登録", Text="業務登録データ連携未登録", Selected=true },
                new SelectListItem() { Value="スポット手配分", Text="スポット手配分" },
            };

    }
}