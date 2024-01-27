using ReviewAPI.Models;

namespace ReviewAPI.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();

        Country GetCountry(int id);

        Country GetCountryByOwner(int ownerId);

        ICollection<Owner> GetOwnersFromACountry(int countryId);

        bool CreateCountry(Country country);

        bool Save();
        bool DeleteCountry(Country country);

        bool UpdateCountry(Country country);

        bool CountryExits(int countryId);
    }
}
