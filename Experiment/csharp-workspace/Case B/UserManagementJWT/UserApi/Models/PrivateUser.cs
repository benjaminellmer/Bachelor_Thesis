using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class PrivateUser {
        public PrivateUser() {
            Users = new HashSet<User>();
        }

        public long PrivateUserId { get; set; }
        public int SwapysAmount { get; set; }
        public long? PremiumId { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}