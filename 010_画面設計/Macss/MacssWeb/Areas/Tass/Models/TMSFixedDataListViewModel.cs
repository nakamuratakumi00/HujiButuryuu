using Macss.Database.Entity;
using System.Collections.Generic;

namespace MacssWeb.Areas.Tass.Models
{
    public class TMSFixedDataListViewModel
    {
        public TMSFixedDataListViewModel()
        {
            this.TMSFixedDataRequests = new List<DummyTMSFixedDataRequest>();
            this.TMSFixedDataBillings = new List<DummyTMSFixedDataBilling>();
            this.TMSFixedDataPayments = new List<DummyTMSFixedDataPayment>();
        }

        public TMSFixedDataListViewModel(IEnumerable<DummyTMSFixedDataRequest> requests
            , IEnumerable<DummyTMSFixedDataBilling> billings
            , IEnumerable<DummyTMSFixedDataPayment> payments) : this()
        {
            if (requests != null)
            {
                this.TMSFixedDataRequests = requests;
            }
            if (billings != null)
            {
                this.TMSFixedDataBillings = billings;
            }
            if (payments != null)
            {
                this.TMSFixedDataPayments = payments;
            }
        }

        public IEnumerable<DummyTMSFixedDataRequest> TMSFixedDataRequests { get; set; }
        public IEnumerable<DummyTMSFixedDataBilling> TMSFixedDataBillings { get; set; }
        public IEnumerable<DummyTMSFixedDataPayment> TMSFixedDataPayments { get; set; }
    }
}