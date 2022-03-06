using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class BusinessOffice {
        public BusinessOffice() {
            BusinessOfficePictures = new HashSet<BusinessOfficePicture>();
        }

        public long BusinessOfficeId { get; set; }
        public long? LocationId { get; set; }
        public long BusinessUserId { get; set; }

        public virtual BusinessUser BusinessUser { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<BusinessOfficePicture> BusinessOfficePictures { get; set; }
    }
}
