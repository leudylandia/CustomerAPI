using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Models.Dto
{
    public class UserDto
    {
        public User User { get; set; }

        //public string UserName { get; set; }
        public string Password { get; set; }
    }
}
