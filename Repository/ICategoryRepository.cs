﻿using Movie.RequestDTO;

namespace Movie.Repository
{
    public interface ICategoryRepository
    {
        Task<RequestCategoryDTO> CreateCategoryAsync(string categoryName);

        Task<IEnumerable<RequestCategoryDTO>> GetAllCategoriesAsync(
            string? search = null,
            string sortBy = "CategoryId",
            string sortDirection = "asc"
);

        Task<RequestCategoryDTO> GetCategoryByIdAsync(int id);

        Task<RequestCategoryDTO> UpdateCategoryAsync(int id, string categoryName);

        Task<bool> DeleteCategoryAsync(int id);
    }
}
