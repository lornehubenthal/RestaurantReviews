using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmmi.review.api.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public bool Deleted { get; set; }

        public RestaurantModel Restaurant { get; set; }
        public UserModel User { get; set; }

    }
}