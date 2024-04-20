using System;
using System.Collections.Generic;

namespace GSS.Models;

public partial class Procudure
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public double? MinAmount { get; set; }

    public double? MaxAmount { get; set; }

    public double? DefaultAmount { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<ReportProcudure> ReportProcudures { get; set; } = new List<ReportProcudure>();
}
