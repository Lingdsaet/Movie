namespace Movies.Repository
{
    public interface IMovieRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie> GetByIdAsync(int id);
        Task AddAsync(Movie entity);
        Task UpdateAsync(Movie entity);
        Task DeleteAsync(int id);
    }
}
