using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Class
{
    public int Id { get; set; }
    [DisplayName("Lớp")]
    public string? Name { get; set; }

    public string? CourseId { get; set; }

    public int? DepartmentId { get; set; }
    [DisplayName("Trạng thái")]
    public byte? IsActive { get; set; }
    public bool? IsDelete { get; set; }
    public virtual Course? Course { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Students> Students { get; set; } = new List<Students>();
}
