using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmmi.review.api.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public bool Locked { get; set; }

        public bool Deleted { get; set; }
    }
}