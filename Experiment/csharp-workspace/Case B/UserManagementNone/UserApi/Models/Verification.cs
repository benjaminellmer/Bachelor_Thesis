using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class Verification {
        public Verification() {
            Users = new HashSet<User>();
        }

        public long VerificationId { get; set; }
        public bool IsPhoneNumberVerified { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsIdentityVerified { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
