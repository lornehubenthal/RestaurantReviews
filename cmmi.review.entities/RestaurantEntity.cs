using System.Collections.Generic;

namespace cmmi.review.entities
{
    public class RestaurantEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }
        
        public bool Deleted { get; set; }

        public RestaurantTypeEntity RestaurantType { get; set; }
        
        public ICollection<ReviewEntity> Review { get; set; }
    }
}
