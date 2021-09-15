using CustomerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Repository
{
    public interface IUserRepository
    {
        Task<int> Resgister(User user, string password);
        Task<string> Login(string username, string password);
        Task<bool> UserExist(string username);
    }
}
