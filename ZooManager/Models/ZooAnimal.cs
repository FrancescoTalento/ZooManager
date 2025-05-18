using System;
using System.Collections.Generic;

namespace ZooManager.Models;

public partial class ZooAnimal
{
    public int Id { get; set; }

    public int ZooId { get; set; }

    public int AnimalId { get; set; }

    public virtual Animal Animal { get; set; } = null!;

    public virtual Zoo Zoo { get; set; } = null!;
}
