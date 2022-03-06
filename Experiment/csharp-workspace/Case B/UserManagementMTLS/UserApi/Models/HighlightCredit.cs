using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class HighlightCredit {
        public long HighlightCreditId { get; set; }
        public short LifespanDays { get; set; }
        public long CreditId { get; set; }

        public virtual Credit Credit { get; set; }
    }
}
