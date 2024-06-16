using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class QuestionHisory
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public int? SemesterId { get; set; }
    
    public int? OrderBy { get; set; }
    [DisplayName("Người tạo")]
    public string? CreateBy { get; set; }
    [DisplayName("Ngày tạo")]

    public DateTime? CreateDate { get; set; }

    public virtual QuestionList? Question { get; set; }

    public virtual Semester? Semester { get; set; }
}
