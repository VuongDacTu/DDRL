using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class GroupQuestion
{
    public int Id { get; set; }
    [DisplayName("NHóm câu hỏi")]
    [MaxLength(255, ErrorMessage ="Độ dài không được quá 255 ký tự")]
    public string? Name { get; set; }

    public virtual ICollection<QuestionList> QuestionLists { get; set; } = new List<QuestionList>();
}
