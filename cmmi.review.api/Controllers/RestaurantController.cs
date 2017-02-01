using AutoMapper;
using cmmi.review.api.Models;
using cmmi.review.business;
using cmmi.review.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cmmi.review.api.Controllers
{
    [RoutePrefix("api/restaurant")]
    public class RestaurantController : ApiController
    {

        private readonly IRestaurantServices _restaurantServices;

        public RestaurantController()
        {
            _restaurantServices = new RestaurantServices();
        }

        [HttpGet]
        [Route("get/all")]
        public HttpResponseMessage GetAllRestaurants()
        {
            var restaurants = _restaurantServices.GetAllRestaurants();
            if (restaurants != null)
            {
                var restaurantEntities = restaurants.ToList();
                if (restaurantEntities.Any())
                {
                    // Map this to a model so we are only sending certain info to the requestor.  Password Hash and Salt not sent to requestor for security reasons.
                    Mapper.Initialize(cfg => { cfg.CreateMap<RestaurantEntity, RestaurantModel>(); cfg.CreateMap<RestaurantTypeEntity, RestaurantTypeModel>(); });
                    var restaurantModel = Mapper.Map<List<RestaurantEntity>, List<RestaurantModel>>(restaurantEntities);
                    return Request.CreateResponse(HttpStatusCode.OK, restaurantModel);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Restaurants not found.");
        }

        // 1. Get a list of restaurants by city
        [HttpGet]
        [Route("get/bycity")]
        public HttpResponseMessage GetRestaurants(string city, string state)
        {
            var restaurants = _restaurantServices.GetRestaurantsByCity(city, state);
            if (restaurants != null)
            {
                var restaurantEntities = restaurants.ToList();
                if (restaurantEntities.Any())
                {
                    // Map this to a model so we are only sending certain info to the requestor.  Password Hash and Salt not sent to requestor for security reasons.
                    Mapper.Initialize(cfg => { cfg.CreateMap<RestaurantEntity, RestaurantModel>(); cfg.CreateMap<RestaurantTypeEntity, RestaurantTypeModel>(); });
                    var restaurantModel = Mapper.Map<List<RestaurantEntity>, List<RestaurantModel>>(restaurantEntities);
                    return Request.CreateResponse(HttpStatusCode.OK, restaurantModel);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Restaurants not found.");
        }

        [HttpGet]
        [Route("get/{restaurantId}")]
        public HttpResponseMessage GetRestaurantById(int restaurantId)
        {
            var restaurant = _restaurantServices.GetRestaurantById(restaurantId);
            if (restaurant != null)
            {
                // Map this to a model so we are only sending certain info to the requestor.  Password Hash and Salt not sent to requestor for security reasons.
                Mapper.Initialize(cfg => { cfg.CreateMap<RestaurantEntity, RestaurantModel>(); cfg.CreateMap<RestaurantTypeEntity, RestaurantTypeModel>(); });
                var userModel = Mapper.Map<RestaurantEntity, RestaurantModel>(restaurant);
                return Request.CreateResponse(HttpStatusCode.OK, userModel);
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Restaurant not found.");
        }

        // 2. Post a restaurant that is not in the database
        [HttpPost]
        [Route("create")]
        public int CreateRestaurant(RestaurantModel restaurant)
        {
            Mapper.Initialize(cfg => { cfg.CreateMap<RestaurantModel, RestaurantEntity>(); cfg.CreateMap<RestaurantTypeModel, RestaurantTypeEntity>(); });
            var restaurantEntity = Mapper.Map<RestaurantModel, RestaurantEntity>(restaurant);

            return _restaurantServices.CreateRestaurant(restaurantEntity);
        }

        [HttpPut]
        [Route("update")]
        public bool UpdateRestaurant(RestaurantModel restaurant)
        {
            Mapper.Initialize(cfg => { cfg.CreateMap<RestaurantModel, RestaurantEntity>(); cfg.CreateMap<RestaurantTypeModel, RestaurantTypeEntity>(); });
            var restaurantEntity = Mapper.Map<RestaurantModel, RestaurantEntity>(restaurant);

            return _restaurantServices.UpdateRestaurant(restaurant.Id, restaurantEntity);
        }

        [HttpDelete]
        [Route("delete/{restaurantId}")]
        public bool DeleteRestaurant(int restaurantId)
        {
            return _restaurantServices.DeleteRestaurant(restaurantId);
        }
    }
}
