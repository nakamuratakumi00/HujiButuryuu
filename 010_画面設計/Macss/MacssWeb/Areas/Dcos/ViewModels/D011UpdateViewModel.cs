using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MacssWeb.Areas.Dcos.ViewModels
{
    public class D011UpdateViewModel
    {
        public IEnumerable<Macss.Database.Entity.DummyStock> Stocks { get; set; }

        [Display(Name = "得意先")]
        public string Client
        {
            get
            {
                return String.Format("{0} {1}", this.ClientCode, this.ClientName);
            }
        }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        [Display(Name = "品名")]
        public string Product
        {
            get
            {
                return String.Format("{0} {1}", this.ProductCode, this.ProductName);
            }
        }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        [Display(Name = "倉庫")]
        public string Warehouse
        {
            get
            {
                return String.Format("{0} {1}", this.WarehouseCode, this.WarehouseName);
            }
        }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        [Display(Name = "棚番")]
        public string RackNumber { get; set; }
        [Display(Name = "ロットNo.")]
        public string LotNumber { get; set; }
        [Display(Name = "保存期限")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM}", ApplyFormatInEditMode = true)]
        public DateTime? StockLimitYM { get; set; }
    }
}