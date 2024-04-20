using System;
using System.Collections.Generic;

namespace GSS.Models;

public partial class Department
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Procudure> Procudures { get; set; } = new List<Procudure>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
