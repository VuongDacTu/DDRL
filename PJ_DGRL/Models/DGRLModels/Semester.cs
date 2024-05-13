using System;
using System.Collections.Generic;

namespace PJ_DGRL.Models.DGRLModels;

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

    public virtual ICollection<ClassAnswer> ClassAnswers { get; set; } = new List<ClassAnswer>();

    public virtual ICollection<QuestionHisory> QuestionHisories { get; set; } = new List<QuestionHisory>();

    public virtual ICollection<SelfAnswer> SelfAnswers { get; set; } = new List<SelfAnswer>();

    public virtual ICollection<SumaryOfPoint> SumaryOfPoints { get; set; } = new List<SumaryOfPoint>();
}
