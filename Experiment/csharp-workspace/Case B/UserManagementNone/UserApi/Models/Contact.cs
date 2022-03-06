using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class Contact {
        public Contact() {
            Locations = new HashSet<Location>();
            Users = new HashSet<User>();
        }

        public long ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public char? Gender { get; set; }
        public string PhoneNumber { get; set; }
        public long? PrimaryLocationId { get; set; }

        public virtual Location PrimaryLocation { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
