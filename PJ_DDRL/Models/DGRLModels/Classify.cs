using System;
using System.Collections.Generic;

namespace PJ_DDRL.Models.DDRLModels;

public partial class Classify
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? PointMin { get; set; }

    public int? PointMax { get; set; }

    public int? OrderBy { get; set; }
}
