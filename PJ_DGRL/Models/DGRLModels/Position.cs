using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Position
{
    public int Id { get; set; }
    [DisplayName("Chức vụ")]
    [Required(ErrorMessage = "Tên chức vụ không được trống")]
    public string Name { get; set; } = null!;

    public byte Status { get; set; }

    public virtual ICollection<Lecturers> Lecturers { get; set; } = new List<Lecturers>();

    public virtual ICollection<Students> Students { get; set; } = new List<Students>();
}
