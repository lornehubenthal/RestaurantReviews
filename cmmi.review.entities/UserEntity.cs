using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmmi.review.entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool RememberMe { get; set; }
        public bool Locked { get; set; }
        public bool ForcePasswordChange { get; set; }
        public bool Deleted { get; set; }

    }
}
