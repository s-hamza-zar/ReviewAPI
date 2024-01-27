using ReviewAPI.Models;

namespace ReviewAPI.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwner();

        Owner GetOwner(int id);

        ICollection<Owner> GetOwnerOfAPokemon(int pokeId);

        ICollection<Pokemon> GetPokemonByOwner(int ownerId);

        bool CreateOwner(Owner owner);

        bool Save();

        bool OwnerExists(int ownerId);

    }
}
