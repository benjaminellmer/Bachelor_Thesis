using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class BusinessOfficePicture {
        public long BusinessOfficePictureId { get; set; }
        public long PictureMedia { get; set; }
        public long BusinessOfficeId { get; set; }

        public virtual BusinessOffice BusinessOffice { get; set; }
    }
}
