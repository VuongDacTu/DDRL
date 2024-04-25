using System;
using System.Collections.Generic;

namespace PJ_DDRL.Models.DDRLModels;

public partial class TypeQuestion
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
