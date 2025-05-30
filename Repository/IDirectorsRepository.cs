﻿﻿using Movie.RequestDTO;

public interface IDirectorsRepository
{
    Task<DirectorDetailDTO?> GetDirectorByIdAsync(int id);
    Task<IEnumerable<RequestDirectorDTO>> GetAllDirectorsAsync(
        string? search = null,
        string sortBy = "NameDir",
        string sortDirection = "asc"
   
    );
    Task<RequestDirectorDTO?> UpdateDirectorAsync(int id, RequestDirectorDTO directorDTO, IFormFile? AvatarUrlFile);
    Task<RequestDirectorDTO> AddDirectorAsync(RequestDirectorDTO directorDTO, IFormFile AvatarUrlFile);
    Task<bool> DeleteDirectorAsync(int id);

}