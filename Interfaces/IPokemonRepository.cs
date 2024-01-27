using ReviewAPI.Dto;
using ReviewAPI.Models;

namespace ReviewAPI.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();

        Pokemon GetPokemon(int id);

        Pokemon GetPokemon(string name);
        Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate);

        decimal GetPokemonRatings(int PokeId);

        bool PokemonExists(int PokeId);

        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);

        bool Save();
    }
}
