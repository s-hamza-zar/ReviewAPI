using Microsoft.AspNetCore.Mvc;
using ReviewAPI.Models;

namespace ReviewAPI.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int id);

        ICollection<Pokemon> GetPokemonByCatergory(int categoryId);

        bool CatergoryExists(int id);

        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
       bool DeleteCategory(Category category);
        bool Save();
    }
}
