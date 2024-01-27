using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewAPI.Dto;
using ReviewAPI.Interfaces;
using ReviewAPI.Models;

namespace ReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;

        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwner());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owners);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(Owner))]

        public IActionResult GetOwner(int id)
        {
            if (!_ownerRepository.OwnerExists(id))
            {
                return NotFound("Owner Not Found");
            }

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }

        [HttpGet("{pokeId:int}")]
        [ProducesResponseType(200, Type = typeof(Owner))]

        public IActionResult GetOwnerOfAPokemon(int pokeId)
        {
            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwnerOfAPokemon(pokeId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
            // GetPokemonByOwner
        }


        [HttpGet("{ownerId:int}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]

        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var pokemon = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public IActionResult CreateOwner([FromQuery] int countryId,[FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var ownerAlreadyExits = _ownerRepository.GetOwner().Where(o => o.LastName == ownerCreate.LastName).FirstOrDefault();

            if (ownerAlreadyExits != null)
            {
                ModelState.AddModelError("", "Owner Already Exits...");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerCreate);

            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong will saving...");
                return StatusCode(500, ModelState);
            }

            return Ok("Sucessfully Created...");

        }
    }
}
