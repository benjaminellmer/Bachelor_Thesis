using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class BusinessUser {
        public BusinessUser() {
            BusinessOffices = new HashSet<BusinessOffice>();
            Users = new HashSet<User>();
        }

        public long BusinessUserId { get; set; }
        public long? BusinessVerificationId { get; set; }
        public long? BusinessPackageId { get; set; }

        public virtual BusinessVerification BusinessVerification { get; set; }
        public virtual ICollection<BusinessOffice> BusinessOffices { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
