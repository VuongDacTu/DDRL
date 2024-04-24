using System;
using System.Collections.Generic;

namespace PJ_DDRL.Models.DDRLModels;

public partial class SumaryOfPoint
{
    public int Id { get; set; }

    public string? StudentId { get; set; }

    public int? SemesterId { get; set; }

    public int? SelfPoint { get; set; }

    public int? ClassPoint { get; set; }

    public int? LecturerPoint { get; set; }

    public string? Classify { get; set; }

    public string? CreateBy { get; set; }

    public string? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual Semester? Semester { get; set; }

    public virtual Student? Student { get; set; }
}
