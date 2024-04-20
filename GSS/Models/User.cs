using System;
using System.Collections.Generic;

namespace GSS.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? ImagePath { get; set; }

    public string? Uid { get; set; }

    public int? UserTypeId { get; set; }

    public DateTime? BirthDate { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual UserType? UserType { get; set; }
}
