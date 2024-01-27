using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewAPI.Dto;
using ReviewAPI.Interfaces;
using ReviewAPI.Models;

namespace ReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]

        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewers);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]

        public IActionResult GetReviewer(int id)
        {
            if (!_reviewerRepository.ReviewerExists(id))
            {
                return NotFound();
            }
            var reviewer = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewer(id));

            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewer);
        }

        [HttpGet("reviews/{reviewerId:int}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]

        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateReviwer([FromBody] ReviewerDto reviewerCreate)
        {

            if (reviewerCreate == null)
            {
                return BadRequest();
            }

            var reviwerAlreadyExits = _reviewerRepository.GetReviewers()
                .Where(r => r.LastName.Trim().ToLower() == reviewerCreate.LastName.Trim().ToLower()).FirstOrDefault();
            if (reviwerAlreadyExits != null)
            {
                ModelState.AddModelError("", "Reviwer already Exits");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviwerMap = _mapper.Map<Reviewer>(reviewerCreate);
            if (!_reviewerRepository.CreateReviewer(reviwerMap))
            {
                ModelState.AddModelError("", "Something went wrong...");
                return StatusCode(500, ModelState);
            }

            return Ok("Sucessfully Created...");
        }
    }
}
