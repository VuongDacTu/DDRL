using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class QuestionList
{
    public int Id { get; set; }
    [DisplayName("Nội dung câu hỏi")]
    [MaxLength(500,ErrorMessage ="Độ dài không được quá 500 ký tự")]
    [Required(ErrorMessage = "Nội dung câu trả lời không được trống")]
    public string? ContentQuestion { get; set; }

    public int? TypeQuestionId { get; set; }

    public int? GroupQuestionId { get; set; }

    public int? OrderBy { get; set; }
    [DisplayName("Trạng thái")]
    public byte? Status { get; set; }
    [DisplayName("Ngày tạo")]

    public DateTime? CreateDate { get; set; }
    [DisplayName("Ngày cập nhật")]

    public DateTime? UpdateDate { get; set; }
    [DisplayName("Người cập nhật")]

    public string? UpdateBy { get; set; }

    public bool? IsEdit { get; set; }

    public virtual ICollection<AnswerList> AnswerLists { get; set; } = new List<AnswerList>();

    public virtual GroupQuestion? GroupQuestion { get; set; }

    public virtual ICollection<QuestionHisory> QuestionHisories { get; set; } = new List<QuestionHisory>();

    public virtual TypeQuestion? TypeQuestion { get; set; }
}
