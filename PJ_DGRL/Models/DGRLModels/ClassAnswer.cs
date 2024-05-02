using System;
using System.Collections.Generic;

namespace PJ_DGRL.Models.DGRLModels;

public partial class ClassAnswer
{
    public int Id { get; set; }

    public string? StudentId { get; set; }

    public int? AnswerId { get; set; }
    public int? SemesterId { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual AnswerList? Answer { get; set; }
    public virtual Semester? Semester { get; set; }

    public virtual Student? Student { get; set; }
}
