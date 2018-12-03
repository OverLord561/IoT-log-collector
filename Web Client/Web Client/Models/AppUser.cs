using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoTWebClient.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string FavouriteDevice { get; set; }
    }
}
