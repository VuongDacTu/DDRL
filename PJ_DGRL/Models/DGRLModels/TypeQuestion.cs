using System;
using System.Collections.Generic;

namespace PJ_DGRL.Models.DGRLModels;

public partial class TypeQuestion
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<QuestionList> QuestionLists { get; set; } = new List<QuestionList>();
}
