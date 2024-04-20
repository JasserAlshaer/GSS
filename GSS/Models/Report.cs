using System;
using System.Collections.Generic;

namespace GSS.Models;

public partial class Report
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public DateTime? RequetDate { get; set; }

    public DateTime? GeneratedDate { get; set; }

    public double? DueAmount { get; set; }

    public bool? IsPaid { get; set; }

    public bool? IsComplete { get; set; }

    public bool? IsPublished { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<ReportProcudure> ReportProcudures { get; set; } = new List<ReportProcudure>();

    public virtual User? User { get; set; }
}
