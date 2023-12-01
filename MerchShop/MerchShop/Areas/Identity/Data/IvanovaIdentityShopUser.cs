using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IvanovaShop.Areas.Identity.Data;

// Add profile data for application users by adding properties to the IvanovaIdentityShopUser class
public class IvanovaIdentityShopUser : IdentityUser
{
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string Patronimyc { get; set; }
}

