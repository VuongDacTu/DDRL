using System;
using System.Collections.Generic;

namespace PJ_DGRL.Models.DGRLModels;

public partial class AnswerList
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public string? ContentAnswer { get; set; }

    public int? AnswerScore { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? UpdateBy { get; set; }

    public byte? Checked { get; set; }

    public bool? IsEdit { get; set; }

    public virtual ICollection<ClassAnswer> ClassAnswers { get; set; } = new List<ClassAnswer>();

    public virtual QuestionList? Question { get; set; }

    public virtual ICollection<SelfAnswer> SelfAnswers { get; set; } = new List<SelfAnswer>();
}
