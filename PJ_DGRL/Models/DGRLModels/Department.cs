using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Department
{
    public int Id { get; set; }
    [DisplayName("Chuyên ngành")]
    [Required(ErrorMessage = "Tên chuyên ngành không được trống")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Số năm học không được trống")]
    public int? Times { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Lecturers> Lecturers { get; set; } = new List<Lecturers>();
}
