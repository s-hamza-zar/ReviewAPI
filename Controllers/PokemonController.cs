using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewAPI.Dto;
using ReviewAPI.Interfaces;
using ReviewAPI.Models;

namespace ReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemon = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }

        [HttpGet("{PokeId:int}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemon(int PokeId)
        {
            if (!_pokemonRepository.PokemonExists(PokeId))
            {
                return NotFound();
            }

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(PokeId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }
        [HttpGet("{PokeId:int}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemonRating(int PokeId)
        {
            if (!_pokemonRepository.PokemonExists(PokeId))
            {
                return NotFound();
            }

            var rating = _pokemonRepository.GetPokemonRatings(PokeId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rating);
        }


        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(string name)
        {
            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(name));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, 
            [FromQuery] int categoryId,
            PokemonDto pokemonCreate)

        {
            if (pokemonCreate == null)
            {
                return BadRequest();
            }

            var pokemonAlreadyExits = _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate);

            if (pokemonAlreadyExits != null)
            {
                ModelState.AddModelError("", "Pokemon Already Exits...");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);


            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving...");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
    }

}
