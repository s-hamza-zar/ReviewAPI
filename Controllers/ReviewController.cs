using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewAPI.Dto;
using ReviewAPI.Interfaces;
using ReviewAPI.Models;

namespace ReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository,
            IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository
            , IMapper mapper)

        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]

        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(Review))]

        public IActionResult GetReview(int id)
        {
            if (_reviewRepository.ReviewExits(id))
            {
                return NotFound();
            }

            var review = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReview(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(review);
        }


        [HttpGet("pokemon/{pokeId:int}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]

        public IActionResult GetReviewsOfAPokemon(int pokeId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public IActionResult CreateReview([FromQuery] int reviwerId,
            [FromQuery] int pokeId,
            [FromBody] ReviewDto reviewCreate)

        {
            if (reviewCreate == null)
            {
                return BadRequest();
            }

            var reviewAlreadyExits = _reviewRepository.GetReviews()
                .Where(r => r.Title.Trim().ToLower() == reviewCreate.Title.Trim().ToLower()).FirstOrDefault();

            if (reviewAlreadyExits != null)
            {
                ModelState.AddModelError("", "Review alreay exits...");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewMap = _mapper.Map<Review>(reviewCreate);

            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviwerId);
            if (_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving...");
                return StatusCode(500, ModelState);
            }

            return Ok("sucessfully Created...");

        }

        [HttpPut("{reviewId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]

        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updateReview)
        {

            if (updateReview == null)
            {
                return BadRequest();
            }
            if (reviewId != updateReview.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.ReviewExits(reviewId))
            {
                return NotFound();
            }

            var reviewMap = _mapper.Map<Review>(updateReview);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving...");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewId:int}")]
        [ProducesResponseType(204)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (_reviewRepository.ReviewExits(reviewId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleteReview = _reviewRepository.GetReview(reviewId);

            if (_reviewRepository.DeleteReview(deleteReview))
            {
                ModelState.AddModelError("", "Something went wrong while deleting...");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("deleteReviewsByReviewer/{reviewerId:int}")]
        [ProducesResponseType(204)]
        public IActionResult DeleteReviewsByReviewer(int reviewerId)
        {
            if (_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleteReviews = _reviewerRepository.GetReviewer(reviewerId)
                .Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();


            if (!_reviewRepository.DeleteReviews(deleteReviews))
            {
                ModelState.AddModelError("", "Something went wrong while deleting...");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
