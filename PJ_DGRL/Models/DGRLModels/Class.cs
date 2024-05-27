using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Class
{
    [DisplayName("Mã lớp")]
    public int Id { get; set; }
    [DisplayName("Lớp")]
    [Required(ErrorMessage = "Tên lớp không được trống")]
    public string? Name { get; set; }

    public string? CourseId { get; set; }

    public int? DepartmentId { get; set; }
    [DisplayName("Trạng thái")]
    public byte? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Students> Students { get; set; } = new List<Students>();

    public virtual ICollection<SumaryOfPoint> SumaryOfPoints { get; set; } = new List<SumaryOfPoint>();
}
