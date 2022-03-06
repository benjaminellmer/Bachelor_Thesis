using System;
using System.Collections.Generic;

#nullable disable

namespace UserApi.Models {
    public partial class Location {
        public Location() {
            BusinessOffices = new HashSet<BusinessOffice>();
            Contacts = new HashSet<Contact>();
        }

        public long LocationId { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public int PostCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool IsPrimaryAddress { get; set; }
        public long? ContactId { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual ICollection<BusinessOffice> BusinessOffices { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
