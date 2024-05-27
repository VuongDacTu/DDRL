using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Course
{
    [Required(ErrorMessage = "Mã khoá học không được trống")]
    public string Id { get; set; } = null!;
    [DisplayName("Khoá học")]
    [Required(ErrorMessage = "Tên khoá học không được trống")]
    public string? Name { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
