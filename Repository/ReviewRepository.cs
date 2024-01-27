using ReviewAPI.Data;
using ReviewAPI.Interfaces;
using ReviewAPI.Models;

namespace ReviewAPI.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _dbContext;

        public ReviewRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<Review> GetReviews()
        {
            return _dbContext.Reviews.ToList();
        }

        public Review GetReview(int id)
        {
            return _dbContext.Reviews.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
        {
            return _dbContext.Reviews.Where(r=>r.Pokemon.Id==pokeId).ToList();
        }

        public bool ReviewExits(int id)
        {
          return _dbContext.Reviews.Any(r=>r.Id == id);
        }

        public bool CreateReview(Review review)
        {
            _dbContext.Add(review);

            return Save();
        }

        public bool Save()
        {
            var saved = _dbContext.SaveChanges();

            return saved > 0 ? true : false;
        }
    }
}
