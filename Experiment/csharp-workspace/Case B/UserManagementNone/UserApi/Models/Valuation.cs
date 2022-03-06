using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class Valuation {
        public long ValuationId { get; set; }
        public long? FromUserId { get; set; }
        public long ToUserId { get; set; }
        public decimal ValuationValue { get; set; }
        public string Text { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
    }
}
