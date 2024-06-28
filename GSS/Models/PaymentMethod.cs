using System;
using System.Collections.Generic;

namespace GSS.Models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string? VisaNumber { get; set; }

    public int? Code { get; set; }

    public double? Balance { get; set; }

    public DateTime? Expire { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
}
