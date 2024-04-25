using System;
using System.Collections.Generic;

namespace PJ_DDRL.Models.DDRLModels;

public partial class Question
{
    public int Id { get; set; }

    public string? ContentQuestion { get; set; }

    public int? TypeQuestionId { get; set; }

    public int? SemesterId { get; set; }

    public int? GroupQuestionId { get; set; }

    public int? OrderBy { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual GroupQuestion? GroupQuestion { get; set; }

    public virtual Semester? Semester { get; set; }

    public virtual TypeQuestion? TypeQuestion { get; set; }
}
