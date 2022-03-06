using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class Credit {
        public Credit() {
            HighlightCredits = new HashSet<HighlightCredit>();
            PrioCredits = new HashSet<PrioCredit>();
            Users = new HashSet<User>();
        }

        public long CreditId { get; set; }
        public short FotoCreditsAmount { get; set; }

        public virtual ICollection<HighlightCredit> HighlightCredits { get; set; }
        public virtual ICollection<PrioCredit> PrioCredits { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
