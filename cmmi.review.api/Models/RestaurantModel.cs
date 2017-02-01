using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmmi.review.api.Models
{
    public class RestaurantModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public bool Deleted { get; set; }

        public RestaurantTypeModel RestaurantType { get; set; }        

    }
}