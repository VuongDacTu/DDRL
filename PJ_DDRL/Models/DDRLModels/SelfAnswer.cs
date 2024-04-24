using System;
using System.Collections.Generic;

namespace PJ_DDRL.Models.DDRLModels;

public partial class SelfAnswer
{
    public int Id { get; set; }

    public string? StudentId { get; set; }

    public int? AnswerId { get; set; }

    public virtual Answer? Answer { get; set; }

    public virtual Student? Student { get; set; }
}
