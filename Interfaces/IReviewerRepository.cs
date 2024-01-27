using ReviewAPI.Models;

namespace ReviewAPI.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);

        bool CreateReviewer(Reviewer reviewer);

        bool Save();
        bool ReviewerExists(int reviewerId);

    }
}
