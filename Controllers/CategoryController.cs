using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewAPI.Dto;
using ReviewAPI.Interfaces;
using ReviewAPI.Models;

namespace ReviewAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]

        public IActionResult GetCategories()
        {
            var allCategory = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(allCategory);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]

        public IActionResult GetCategory(int id)
        {

            if (!_categoryRepository.CatergoryExists(id))
            {
                return NotFound();
            }
            var category = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategory(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(category);

        }


        [HttpGet("{categoryId:int}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemonByCatergory(int categoryId)
        {

            if (!_categoryRepository.CatergoryExists(categoryId))
            {
                return NotFound();
            }
            var pokemon = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonByCatergory(categoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);

        }

        [HttpPost]
        [ProducesResponseType(201)]
        public IActionResult CreateCatergory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
            {
                return BadRequest(ModelState);
            }

            var catergoryAlreadyExits = _categoryRepository.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.Trim().ToUpper()).FirstOrDefault();

            if (catergoryAlreadyExits != null)
            {
                ModelState.AddModelError("", "Catergory Already Exits");
                return StatusCode(422, ModelState);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }
    }
}
