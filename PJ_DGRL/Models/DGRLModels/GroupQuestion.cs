using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class GroupQuestion
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Nhóm câu hỏi không được trống")]
    public string? Name { get; set; }

    public virtual ICollection<QuestionList> QuestionLists { get; set; } = new List<QuestionList>();
}
