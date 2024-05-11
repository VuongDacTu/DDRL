using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Course
{
    public string Id { get; set; } = null!;
    [DisplayName("Khoá học")]
    public string? Name { get; set; }
    public bool IsDelete { get; set; }
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
