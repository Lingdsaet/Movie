namespace Movies.RequestDTO
{
    public class RequestMovieCategoryDTO
    {
        public int MovieID { get; set; }
        public int CategoriesID { get; set; }

        public RequestMovieDTO Movie { get; set; }
        public RequestCategoryDTO Category { get; set; }
    }
}
