using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoTDevicesManagment.Models
{
    public class ApplicationUsers : IdentityUser
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String FullName => $"{FirstName} {LastName}";
    }
}
