using System;
using System.Collections.Generic;

namespace GSS.Models;

public partial class ReportProcudure
{
    public int Id { get; set; }

    public int? ProcudureId { get; set; }

    public int? ReportId { get; set; }

    public double? ActualAmount { get; set; }

    public virtual Procudure? Procudure { get; set; }

    public virtual Report? Report { get; set; }
}
