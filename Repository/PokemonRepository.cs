using ReviewAPI.Data;
using ReviewAPI.Dto;
using ReviewAPI.Interfaces;
using ReviewAPI.Models;

namespace ReviewAPI.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _dbContext;

        public PokemonRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _dbContext.Pokemons.ToList();
        }

        public Pokemon GetPokemon(int id)
        {
            return _dbContext.Pokemons.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _dbContext.Pokemons.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRatings(int PokeId)
        {
            var review = _dbContext.Reviews.Where(p => p.Pokemon.Id == PokeId);

            if (review.Count() <= 0)
            {
                return 0;
            }
            var avgerage = (decimal)review.Sum(p => p.Rating) / review.Count();

            return (avgerage);
        }

        public bool PokemonExists(int PokeKId)
        {
            return _dbContext.Pokemons.Any(p => p.Id == PokeKId);
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var ownerEntity = _dbContext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var categoryEntity = _dbContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Pokemon = pokemon,
                Owner = ownerEntity
            };
            _dbContext.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {

                Pokemon = pokemon,
                Category = categoryEntity
            };

            _dbContext.Add(pokemonCategory);

            _dbContext.Add(pokemon);

            return Save();

        }
        public Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate)
        {
            return GetPokemons().Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _dbContext.SaveChanges();

            return saved > 0 ? true : false;
        }
    }
}
