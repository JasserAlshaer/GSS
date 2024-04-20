using System;
using System.Collections.Generic;

namespace GSS.Models;

public partial class Invoice
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public DateTime? IssuedDate { get; set; }

    public double? Amount { get; set; }

    public int? ReportId { get; set; }

    public virtual Report? Report { get; set; }
}
