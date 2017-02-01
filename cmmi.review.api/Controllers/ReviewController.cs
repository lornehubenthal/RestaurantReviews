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
    [RoutePrefix("api/review")]
    public class ReviewController : ApiController
    {
        private readonly IReviewServices _reviewServices;

        public ReviewController()
        {
            _reviewServices = new ReviewServices();
        }

        [HttpGet]
        [Route("get/all")]
        public HttpResponseMessage GetAllReviews()
        {
            var reviews = _reviewServices.GetAllReviews();
            if (reviews != null)
            {
                var reviewEntities = reviews.ToList();
                if (reviewEntities.Any())
                {
                    Mapper.Initialize(cfg => { cfg.CreateMap<ReviewEntity, ReviewModel>(); cfg.CreateMap<RestaurantEntity, RestaurantModel>();
                        cfg.CreateMap<UserEntity, UserModel>(); cfg.CreateMap<RestaurantTypeEntity, RestaurantTypeModel>(); });
                    var reviewModel = Mapper.Map<List<ReviewEntity>, List<ReviewModel>>(reviewEntities);
                    return Request.CreateResponse(HttpStatusCode.OK, reviewModel);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Reviews not found.");
        }

        [HttpGet]
        [Route("get/byrestaurant")]
        public HttpResponseMessage GetReviewsByRestaurant(int restaurantId)
        {
            var reviews = _reviewServices.GetReviewsByRestaurant(restaurantId);
            if (reviews != null)
            {
                var reviewEntities = reviews.ToList();
                if (reviewEntities.Any())
                {
                    Mapper.Initialize(cfg => {
                        cfg.CreateMap<ReviewEntity, ReviewModel>(); cfg.CreateMap<RestaurantEntity, RestaurantModel>();
                        cfg.CreateMap<UserEntity, UserModel>(); cfg.CreateMap<RestaurantTypeEntity, RestaurantTypeModel>();
                    });
                    var reviewModel = Mapper.Map<List<ReviewEntity>, List<ReviewModel>>(reviewEntities);
                    return Request.CreateResponse(HttpStatusCode.OK, reviewModel);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Reviews not found.");
        }

        // 4. Get of a list of reviews by user
        [HttpGet]
        [Route("get/byuser")]
        public HttpResponseMessage GetReviewsByUser(int userId)
        {
            var reviews = _reviewServices.GetReviewsByUser(userId);
            if (reviews != null)
            {
                var reviewEntities = reviews.ToList();
                if (reviewEntities.Any())
                {
                    Mapper.Initialize(cfg => {
                        cfg.CreateMap<ReviewEntity, ReviewModel>(); cfg.CreateMap<RestaurantEntity, RestaurantModel>();
                        cfg.CreateMap<UserEntity, UserModel>(); cfg.CreateMap<RestaurantTypeEntity, RestaurantTypeModel>();
                    });
                    var reviewModel = Mapper.Map<List<ReviewEntity>, List<ReviewModel>>(reviewEntities);
                    return Request.CreateResponse(HttpStatusCode.OK, reviewModel);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Reviews not found.");
        }

        [HttpGet]
        [Route("get/{reviewId}")]
        public HttpResponseMessage GetReviewById(int reviewId)
        {
            var review = _reviewServices.GetReviewById(reviewId);
            if (review != null)
            {
                // Map this to a model so we are only sending certain info to the requestor.  Password Hash and Salt not sent to requestor for security reasons.
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<ReviewEntity, ReviewModel>(); cfg.CreateMap<RestaurantEntity, RestaurantModel>();
                    cfg.CreateMap<UserEntity, UserModel>(); cfg.CreateMap<RestaurantTypeEntity, RestaurantTypeModel>();
                });
                var reviewModel = Mapper.Map<ReviewEntity, ReviewModel>(review);
                return Request.CreateResponse(HttpStatusCode.OK, reviewModel);
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Review not found.");
        }

        // 3. Post a review for a restaurant
        [HttpPost]
        [Route("create")]
        public int CreateReview(ReviewModel review)
        {
            Mapper.Initialize(cfg => { cfg.CreateMap<ReviewModel, ReviewEntity>()
                    .ForMember(dest => dest.Restaurant, opt => opt.Ignore())
                    .ForMember(dest => dest.User, opt => opt.Ignore()); });
            var reviewEntity = Mapper.Map<ReviewModel, ReviewEntity>(review);

            return _reviewServices.CreateReview(reviewEntity);
        }

        [HttpPut]
        [Route("update")]
        public bool UpdateReview(ReviewModel review)
        {
            Mapper.Initialize(cfg => { cfg.CreateMap<ReviewModel, ReviewEntity>()
                .ForMember(dest => dest.Restaurant, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore()); });
            var reviewEntity = Mapper.Map<ReviewModel, ReviewEntity>(review);

            return _reviewServices.UpdateReview(reviewEntity.Id, reviewEntity);
        }

        // 5. Delete a review
        [HttpDelete]
        [Route("delete/{reviewId}")]
        public bool DeleteReview(int reviewId)
        {
            // LCH :: 2017.01.30 ~~ "Delete" actually sets a flag deleted to true.  This is to prevent index fragmentation.  I never delete live on a database.
            // If space needs to be reclaimed I create a job that can run during off hours that removes Deleted=True records and rebuilds the index.
            return _reviewServices.DeleteReview(reviewId);
        }

    }
}