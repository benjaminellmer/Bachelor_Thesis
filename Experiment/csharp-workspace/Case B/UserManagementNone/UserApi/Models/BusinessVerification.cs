using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class BusinessVerification {
        public BusinessVerification() {
            BusinessUsers = new HashSet<BusinessUser>();
        }

        public long BusinessVerificationId { get; set; }
        public long? DocumentMediaId { get; set; }
        public DateTime RequestDate { get; set; }
        public short VerificationRequestState { get; set; }
        public string Message { get; set; }

        public virtual ICollection<BusinessUser> BusinessUsers { get; set; }
    }
}
