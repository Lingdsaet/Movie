using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Models
{
    public class MovieCategory
    {
        public int MovieCategoryID { get; set; }
        public int MovieID { get; set; }
        public int CategoriesID { get; set; }
        [ForeignKey("MovieID")]
        public required Movie Movie { get; set; }
        [ForeignKey("CategoryID")]
        public required Category Category { get; set; }
    }
}
