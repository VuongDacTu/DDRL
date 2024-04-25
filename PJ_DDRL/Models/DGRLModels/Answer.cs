using System;
using System.Collections.Generic;

namespace PJ_DDRL.Models.DDRLModels;

public partial class Answer
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public string? ContentAnswer { get; set; }

    public int? AnswerScore { get; set; }

    public virtual ICollection<ClassAnswer> ClassAnswers { get; set; } = new List<ClassAnswer>();

    public virtual Question? Question { get; set; }

    public virtual ICollection<SelfAnswer> SelfAnswers { get; set; } = new List<SelfAnswer>();
}
