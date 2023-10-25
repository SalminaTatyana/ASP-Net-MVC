using System;
using System.Collections.Generic;

namespace IvanovaShop;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int ProductCtegoryId { get; set; }

    public int Price { get; set; }

    public string Image { get; set; }

    public string Description { get; set; }

    public int? Size { get; set; }

    public string Material { get; set; }

    public bool? Gender { get; set; }

    public double? Weight { get; set; }

    public string Type { get; set; }

    public string LockType { get; set; }

    public string Genre { get; set; }

    public string Appointment { get; set; }

    public string Color { get; set; }

    public virtual Category ProductCtegory { get; set; }
}
