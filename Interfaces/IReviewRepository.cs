using ReviewAPI.Models;

namespace ReviewAPI.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();

        Review GetReview(int id);

        ICollection<Review> GetReviewsOfAPokemon(int pokeId);

        bool CreateReview(Review review);

        bool Save();

        bool ReviewExits(int id);   
    }
}
