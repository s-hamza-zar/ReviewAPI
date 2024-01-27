using AutoMapper;
using ReviewAPI.Dto;
using ReviewAPI.Models;

namespace ReviewAPI.Helper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<PokemonDto, Pokemon>();


            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();


            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();


            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();

            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<ReviewerDto, Reviewer>();

        }
    }
}
