using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Course
{
    [MaxLength(4, ErrorMessage = "Độ dài không được quá 4 ký tự")]
    [Required(ErrorMessage ="Tên khoá học không được trống")]
    [DisplayName("Khoá")]

    public string Id { get; set; } = null!;
    [DisplayName("Năm học")]
    [MaxLength(50,ErrorMessage ="Độ dài không được quá 50 ký tự")]
    [Required(ErrorMessage = "Năm học không được trống")]

    public string? Name { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
