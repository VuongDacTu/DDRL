using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Semester
{
    public int Id { get; set; }
    [DisplayName("Kỳ")]
    [StringLength(2,ErrorMessage ="Độ dài không vượt quá 2 ký tự")]
    public string? Name { get; set; }
    [DisplayName("Năm học")]
    [StringLength(20,ErrorMessage ="Độ dài không vượt quá 20 ký tự")]
    public string? SchoolYear { get; set; }
    [DisplayName("Ngày mở đánh giá")]

    public DateTime? DateOpenStudent { get; set; }
    [DisplayName("Ngày kết thúc đánh giá của sinh viên")]

    public DateTime? DateEndStudent { get; set; }
    [DisplayName("Ngày kết thúc đánh giá của cán bộ lớp")]

    public DateTime? DateEndClass { get; set; }
    [DisplayName("Ngày kết thúc đánh giá của giảng viên")]
    public DateTime? DateEndLecturer { get; set; }
    [DisplayName("Trạng thái")]

    public byte? IsActive { get; set; }

    public virtual ICollection<ClassAnswer> ClassAnswers { get; set; } = new List<ClassAnswer>();

    public virtual ICollection<QuestionHisory> QuestionHisories { get; set; } = new List<QuestionHisory>();

    public virtual ICollection<SelfAnswer> SelfAnswers { get; set; } = new List<SelfAnswer>();

    public virtual ICollection<SumaryOfPoint> SumaryOfPoints { get; set; } = new List<SumaryOfPoint>();
}
