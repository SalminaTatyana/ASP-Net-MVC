using System;
using System.Collections.Generic;

namespace IvanovaShop;

public partial class User 
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string SecondName { get; set; }

    public string Patronymic { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public int RoleId { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public virtual Role Role { get; set; }
}
