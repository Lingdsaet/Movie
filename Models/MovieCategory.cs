namespace Movies.Models
{
    public class MovieCategory
    {
        public int MovieID { get; set; }
        public int CategoriesID { get; set; }

        public Movie Movie { get; set; }
        public Category Category { get; set; }
    }
}
