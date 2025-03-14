namespace Movie.RequestDTO
{
    public class RequestMovieCategoryDTO
    {
        public int MovieId { get; set; }
        public int CategoriesId { get; set; }

        public required RequestMovieDTO Movie { get; set; }
        public required RequestCategoryDTO Category { get; set; }
    }
}
