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
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]

        public IActionResult GetReviews()
        {
            var reviews =_mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

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
    }
}
