using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmmi.review.entities
{
    public class ReviewEntity
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public bool Deleted { get; set; }

        public RestaurantEntity Restaurant { get; set; }
        public UserEntity User { get; set; }
    }
}
