using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Department
{
    public int Id { get; set; }
    [DisplayName("Chuyên ngành")]
    public string? Name { get; set; }

    public int? Times { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
}
