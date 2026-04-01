using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaSystem.Models;
using CinemaSystem.DataStructures;

namespace CinemaSystem.Pages
{
    public class FilmsModel : PageModel
    {
        private readonly CustomHashTable<int, Film> _filmTable;

        public List<Film> Films { get; set; } = new List<Film>();
        public string SearchTerm { get; set; } = "";
        public string GenreFilter { get; set; } = "";

        public FilmsModel(CustomHashTable<int, Film> filmTable)
        {
            _filmTable = filmTable;
        }

        public void OnGet(string search, string genre)
        {
            SearchTerm = search ?? "";
            GenreFilter = genre ?? "";

            Film[] allFilms = _filmTable.GetAllValues();

            for (int i = 0; i < allFilms.Length; i++)
            {
                bool matchesSearch = string.IsNullOrEmpty(SearchTerm) ||
                    allFilms[i].Title.ToLower().Contains(SearchTerm.ToLower());

                bool matchesGenre = string.IsNullOrEmpty(GenreFilter) ||
                    allFilms[i].Genre.ToLower() == GenreFilter.ToLower();

                if (matchesSearch && matchesGenre)
                {
                    Films.Add(allFilms[i]);
                }
            }
        }
    }
}