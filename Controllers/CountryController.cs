using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewAPI.Dto;
using ReviewAPI.Interfaces;
using ReviewAPI.Models;

namespace ReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(countries);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(Country))]

        public IActionResult GetCountry(int id)
        {
            if (!_countryRepository.CountryExits(id))
            {
                return NotFound();
            }

            var country = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountry(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);
        }

        [HttpGet("{ownerId:int}")]
        [ProducesResponseType(200, Type = typeof(Country))]

        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = _mapper.Map<List<Country>>(_countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);

        }

        [HttpGet("{countryId:int}/owner")]
        [ProducesResponseType(200, Type = typeof(Owner))]

        public IActionResult GetOwnerFromACountry(int countryId)
        {
            if (!_countryRepository.CountryExits(countryId))
            {
                return NotFound();
            }

            var owner = _mapper.Map<List<OwnerDto>>(_countryRepository.GetOwnersFromACountry(countryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public IActionResult CreateCountry(CountryDto countryCreate)
        {
            if (countryCreate == null)
            {
                return BadRequest(ModelState);
            }

            var countryAlreadyExits = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim() == countryCreate.Name.Trim()).FirstOrDefault();

            if (countryAlreadyExits != null)
            {
                ModelState.AddModelError("", "Country Already Exits");
                return StatusCode(204, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Country Created sucessfully");
        }
    }
}
