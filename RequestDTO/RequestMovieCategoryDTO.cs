namespace Movie.RequestDTO
{
    public class RequestMovieCategoryDTO
    {
        public int MovieID { get; set; }
        public int CategoriesID { get; set; }

        public required RequestMovieDTO Movie { get; set; }
        public required RequestCategoryDTO Category { get; set; }
    }
}
