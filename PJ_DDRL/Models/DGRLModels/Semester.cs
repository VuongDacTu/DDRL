using System;
using System.Collections.Generic;

namespace PJ_DDRL.Models.DDRLModels;

public partial class Semester
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? SchoolYear { get; set; }

    public DateTime? DateOpenStudent { get; set; }

    public DateTime? DateEndStudent { get; set; }

    public DateTime? DateEndClass { get; set; }

    public DateTime? DateEndLecturer { get; set; }

    public byte? IsActive { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<SumaryOfPoint> SumaryOfPoints { get; set; } = new List<SumaryOfPoint>();
}
