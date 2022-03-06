using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class UserWatchList {
        public long UserId { get; set; }
        public long AdId { get; set; }
        public DateTime DateOfCreation { get; set; }

        public virtual User User { get; set; }
    }
}
