namespace ReviewAPI.Models
{
    public class PokemonCategory
    {
        public int PokemonId { get; set; }
        public int CatergoryId { get; set; }

        public Pokemon Pokemon { get; set; }

        public Category Category { get; set; }
    }
}
