using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phonebook.Models
{
    public class Contacts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Address { get; set; }
        //public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}