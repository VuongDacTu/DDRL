using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Position
{
    public string Id { get; set; } = null!;
    [DisplayName("Chức vụ")]
    public string? Name { get; set; }

    public virtual ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();

    public virtual ICollection<Students> Students { get; set; } = new List<Students>();
}
