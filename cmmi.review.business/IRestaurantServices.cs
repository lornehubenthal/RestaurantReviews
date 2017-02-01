using cmmi.review.entities;
using System.Collections.Generic;

namespace cmmi.review.business
{
    public interface IRestaurantServices
    {
        ICollection<RestaurantEntity> GetAllRestaurants();
        ICollection<RestaurantEntity> GetRestaurantsByCity(string city, string state);
        RestaurantEntity GetRestaurantById(int restaurantId);
        int CreateRestaurant(RestaurantEntity restaurantEntity);
        bool UpdateRestaurant(int restaurantId, RestaurantEntity restaurantEntity);
        bool DeleteRestaurant(int restaurantId);
    }
}
