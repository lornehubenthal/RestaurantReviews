using System;
using System.Collections.Generic;
using System.Linq;
using cmmi.review.entities;
using cmmi.review.data;
using AutoMapper;
using System.Transactions;

namespace cmmi.review.business
{
    public class RestaurantServices : IRestaurantServices
    {
        private readonly UnitOfWork _unitOfWork;

        public RestaurantServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public int CreateRestaurant(RestaurantEntity restaurantEntity)
        {
            using (var scope = new TransactionScope())
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<RestaurantEntity, Restaurant>()
                        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.RestaurantType.Id))
                        .ForMember(dest => dest.RestaurantType, opt => opt.Ignore()); });
                var restaurant = Mapper.Map<RestaurantEntity, Restaurant>(restaurantEntity);

                _unitOfWork.RestaurantRepository.Insert(restaurant);
                _unitOfWork.Save();
                scope.Complete();

                return restaurant.Id;
            }
        }

        public bool DeleteRestaurant(int restaurantId)
        {
            // Does not actually delete to avoid index fragmentation
            var restaurant = GetRestaurantById(restaurantId);
            if (restaurant != null)
            {
                restaurant.Deleted = true;
                return UpdateRestaurant(restaurant.Id, restaurant);
            }

            return false;
        }

        public ICollection<RestaurantEntity> GetAllRestaurants()
        {
            var restaurants = _unitOfWork.RestaurantRepository.GetMany(r => !r.Deleted).ToList();

            if (restaurants.Any())
            {
                Mapper.Initialize(cfg => { cfg.CreateMap<Restaurant, RestaurantEntity>(); cfg.CreateMap<RestaurantType, RestaurantTypeEntity>(); });
                var restaurantEntity = Mapper.Map<List<Restaurant>, List<RestaurantEntity>>(restaurants);
                return restaurantEntity;
            }

            return null;
        }

        public ICollection<RestaurantEntity> GetRestaurantsByCity(string city, string state)
        {
            var restaurants = _unitOfWork.RestaurantRepository.GetMany(r => !r.Deleted && r.City == city && r.State == state).ToList();

            if (restaurants.Any())
            {
                Mapper.Initialize(cfg => { cfg.CreateMap<Restaurant, RestaurantEntity>(); cfg.CreateMap<RestaurantType, RestaurantTypeEntity>(); });
                var restaurantEntity = Mapper.Map<List<Restaurant>, List<RestaurantEntity>>(restaurants);
                return restaurantEntity;
            }

            return null;
        }

        public RestaurantEntity GetRestaurantById(int restaurantId)
        {
            var restaurant = _unitOfWork.RestaurantRepository.GetByID(restaurantId);

            if (restaurant != null)
            {
                Mapper.Initialize(cfg => { cfg.CreateMap<Restaurant, RestaurantEntity>(); cfg.CreateMap<RestaurantType, RestaurantTypeEntity>(); });
                var restaurantEntity = Mapper.Map<Restaurant, RestaurantEntity>(restaurant);
                return restaurantEntity;
            }

            return null;
        }

        public bool UpdateRestaurant(int restaurantId, RestaurantEntity restaurantEntity)
        {
            var success = false;
            if (restaurantEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var restaurant = _unitOfWork.RestaurantRepository.GetByID(restaurantId);
                    if (restaurant != null)
                    {
                        restaurant.Name = restaurantEntity.Name;
                        restaurant.StreetAddress1 = restaurantEntity.StreetAddress1;
                        restaurant.StreetAddress2 = restaurantEntity.StreetAddress2;
                        restaurant.City = restaurantEntity.City;
                        restaurant.State = restaurantEntity.State;
                        restaurant.Zip = restaurantEntity.Zip;
                        restaurant.Type = restaurantEntity.RestaurantType.Id;
                        restaurant.Deleted = restaurantEntity.Deleted;

                        _unitOfWork.RestaurantRepository.Update(restaurant);
                        _unitOfWork.Save();
                        scope.Complete();

                        success = true;
                    }
                }
            }

            return success;
        }
    }
}
