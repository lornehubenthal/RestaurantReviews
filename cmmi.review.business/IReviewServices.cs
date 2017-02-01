using cmmi.review.entities;
using System.Collections.Generic;

namespace cmmi.review.business
{
    public interface IReviewServices
    {
        ICollection<ReviewEntity> GetAllReviews();
        ICollection<ReviewEntity> GetReviewsByRestaurant(int restaurantId);
        ICollection<ReviewEntity> GetReviewsByUser(int userId);
        ReviewEntity GetReviewById(int reviewId);
        int CreateReview(ReviewEntity reviewEntity);
        bool UpdateReview(int reviewId, ReviewEntity reviewEntity);
        bool DeleteReview(int reviewId);
    }
}
