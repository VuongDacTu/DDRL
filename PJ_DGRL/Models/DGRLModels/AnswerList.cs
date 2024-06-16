using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class AnswerList
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }
    [DisplayName("Nội dung câu trả lời")]
    [Required(ErrorMessage ="Nội dung câu trả lời không được trống")]
    [MaxLength(500,ErrorMessage ="Độ dài không được quá 500 ký tự")]
    public string? ContentAnswer { get; set; }
    [DisplayName("Điểm")]
    [Required(ErrorMessage = "Điểm không được trống")]
    public int? AnswerScore { get; set; }
    [DisplayName("Trạng thái")]

    public byte? Status { get; set; }
    [DisplayName("Ngày tạo")]

    public DateTime? CreateDate { get; set; }
    [DisplayName("Ngày cập nhật")]

    public DateTime? UpdateDate { get; set; }
    [DisplayName("Người cập nhật")]

    public string? UpdateBy { get; set; }

    public byte? Checked { get; set; }

    public bool? IsEdit { get; set; }

    public virtual ICollection<ClassAnswer> ClassAnswers { get; set; } = new List<ClassAnswer>();

    public virtual QuestionList? Question { get; set; }

    public virtual ICollection<SelfAnswer> SelfAnswers { get; set; } = new List<SelfAnswer>();
}
