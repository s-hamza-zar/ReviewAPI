using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewAPI.Data;
using ReviewAPI.Interfaces;
using ReviewAPI.Models;

namespace ReviewAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCatergory(int categoryId)
        {
           return _context.PokemonCategories.Where(c=>c.CatergoryId==categoryId).Select(p=>p.Pokemon).ToList();
        }

        public bool CatergoryExists(int id)
        {
           return _context.Categories.Any(c=>c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }
    }
}
