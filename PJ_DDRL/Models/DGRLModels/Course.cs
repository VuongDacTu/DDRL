using System;
using System.Collections.Generic;

namespace PJ_DDRL.Models.DDRLModels;

public partial class Course
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
