using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class User {
        public User() {
            FollowerFromUsers = new HashSet<Follower>();
            FollowerToUsers = new HashSet<Follower>();
            UserWatchLists = new HashSet<UserWatchList>();
            ValuationFromUsers = new HashSet<Valuation>();
            ValuationToUsers = new HashSet<Valuation>();
        }

        public long UserId { get; set; }
        public string Uid { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public long? ContactId { get; set; }
        public long? CreditId { get; set; }
        public long? ProfilePictureMediaId { get; set; }
        public long? PrivateUserId { get; set; }
        public long? BusinessUserId { get; set; }
        public long? VerificationId { get; set; }

        public virtual BusinessUser BusinessUser { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Credit Credit { get; set; }
        public virtual PrivateUser PrivateUser { get; set; }
        public virtual Verification Verification { get; set; }
        public virtual ICollection<Follower> FollowerFromUsers { get; set; }
        public virtual ICollection<Follower> FollowerToUsers { get; set; }
        public virtual ICollection<UserWatchList> UserWatchLists { get; set; }
        public virtual ICollection<Valuation> ValuationFromUsers { get; set; }
        public virtual ICollection<Valuation> ValuationToUsers { get; set; }
    }
}
