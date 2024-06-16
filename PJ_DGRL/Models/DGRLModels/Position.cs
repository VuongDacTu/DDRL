using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Position
{
    public int Id { get; set; }
    [DisplayName("Chức vụ")]
    [MaxLength(50,ErrorMessage ="Độ dài không được quá 50 ký tự")]
    public string Name { get; set; } = null!;

    public byte Status { get; set; }

    public virtual ICollection<Lecturers> Lecturers { get; set; } = new List<Lecturers>();

    public virtual ICollection<Students> Students { get; set; } = new List<Students>();
}
