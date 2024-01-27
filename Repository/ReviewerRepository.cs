using ReviewAPI.Data;
using ReviewAPI.Interfaces;
using ReviewAPI.Models;

namespace ReviewAPI.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _dbContext;

        public ReviewerRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _dbContext.Reviewers.ToList();
        }
        public Reviewer GetReviewer(int reviewerId)
        {
            return _dbContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _dbContext.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _dbContext.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _dbContext.Add(reviewer);
            return Save();
        }

        public bool Save()
        {
            var saved = _dbContext.SaveChanges();

            return saved>0?true:false;
        }
    }
}
