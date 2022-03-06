using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class Follower {
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
    }
}
