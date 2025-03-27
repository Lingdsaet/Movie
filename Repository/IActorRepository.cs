using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public interface IActorRepository    
    {
        Task<ActorDetailDTO?> GetActorByIdAsync(int id);
    }
}
