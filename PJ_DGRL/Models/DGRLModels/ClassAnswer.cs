using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class ClassAnswer
{
    public int Id { get; set; }
    [DisplayName("Mã sinh viên")]
    public string? StudentId { get; set; }

    public int? AnswerId { get; set; }
    [DisplayName("Người tạo")]
    public string? CreateBy { get; set; }
    [DisplayName("Ngày tạo")]
    public DateTime? CreateDate { get; set; }

    public int? SemesterId { get; set; }

    public virtual AnswerList? Answer { get; set; }

    public virtual Semester? Semester { get; set; }

    public virtual Students? Student { get; set; }
}
