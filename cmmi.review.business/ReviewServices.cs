using System;
using System.Collections.Generic;
using System.Linq;
using cmmi.review.entities;
using cmmi.review.data;
using System.Transactions;
using AutoMapper;

namespace cmmi.review.business
{
    public class ReviewServices : IReviewServices
    {
        private readonly UnitOfWork _unitOfWork;

        public ReviewServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public int CreateReview(ReviewEntity reviewEntity)
        {
            using (var scope = new TransactionScope())
            {
                Mapper.Initialize(cfg => cfg.CreateMap<ReviewEntity, Review>()
                    .ForMember(dest => dest.Restaurant, opt => opt.Ignore())
                    .ForMember(dest => dest.User, opt => opt.Ignore())
                );
                var review = Mapper.Map<ReviewEntity, Review>(reviewEntity);
                
                _unitOfWork.ReviewRepository.Insert(review);
                _unitOfWork.Save();
                scope.Complete();

                return review.Id;
            }
        }

        public bool DeleteReview(int reviewId)
        {
            // Does not actually delete to avoid index fragmentation
            var review = GetReviewById(reviewId);
            if (review != null)
            {
                review.Deleted = true;
                return UpdateReview(review.Id, review);
            }

            return false;
        }

        public ICollection<ReviewEntity> GetAllReviews()
        {
            var reviews = _unitOfWork.ReviewRepository.GetMany(r => !r.Deleted).ToList();

            if (reviews.Any())
            {
                Mapper.Initialize(cfg => { cfg.CreateMap<Review, ReviewEntity>(); cfg.CreateMap<Restaurant, RestaurantEntity>().ForMember(dest => dest.Review, opt => opt.Ignore());
                    cfg.CreateMap<User, UserEntity>(); cfg.CreateMap<RestaurantType, RestaurantTypeEntity>(); });
                var reviewEntity = Mapper.Map<List<Review>, List<ReviewEntity>>(reviews);
                return reviewEntity;
            }

            return null;
        }

        public ReviewEntity GetReviewById(int reviewId)
        {
            var review = _unitOfWork.ReviewRepository.GetByID(reviewId);

            if (review != null)
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<Review, ReviewEntity>(); cfg.CreateMap<Restaurant, RestaurantEntity>().ForMember(dest => dest.Review, opt => opt.Ignore());
                    cfg.CreateMap<User, UserEntity>(); cfg.CreateMap<RestaurantType, RestaurantTypeEntity>();
                });
                var reviewEntity = Mapper.Map<Review, ReviewEntity>(review);
                return reviewEntity;
            }

            return null;
        }

        public ICollection<ReviewEntity> GetReviewsByRestaurant(int restaurantId)
        {
            var reviews = _unitOfWork.ReviewRepository.GetMany(r => !r.Deleted && r.RestaurantId == restaurantId).ToList();

            if (reviews.Any())
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<Review, ReviewEntity>(); cfg.CreateMap<Restaurant, RestaurantEntity>().ForMember(dest => dest.Review, opt => opt.Ignore());
                    cfg.CreateMap<User, UserEntity>(); cfg.CreateMap<RestaurantType, RestaurantTypeEntity>();
                });
                var reviewEntity = Mapper.Map<List<Review>, List<ReviewEntity>>(reviews);
                return reviewEntity;
            }

            return null;
        }

        public ICollection<ReviewEntity> GetReviewsByUser(int userId)
        {
            var reviews = _unitOfWork.ReviewRepository.GetMany(r => !r.Deleted && r.UserId == userId).ToList();

            if (reviews.Any())
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<Review, ReviewEntity>(); cfg.CreateMap<Restaurant, RestaurantEntity>().ForMember(dest => dest.Review, opt => opt.Ignore());
                    cfg.CreateMap<User, UserEntity>(); cfg.CreateMap<RestaurantType, RestaurantTypeEntity>();
                });
                var reviewEntity = Mapper.Map<List<Review>, List<ReviewEntity>>(reviews);
                return reviewEntity;
            }

            return null;
        }

        public bool UpdateReview(int reviewId, ReviewEntity reviewEntity)
        {
            var success = false;
            if (reviewEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var review = _unitOfWork.ReviewRepository.GetByID(reviewId);
                    if (review != null)
                    {
                        review.Rating = reviewEntity.Rating;
                        review.Comments = reviewEntity.Comments;
                        review.Deleted = reviewEntity.Deleted;

                        _unitOfWork.ReviewRepository.Update(review);
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
