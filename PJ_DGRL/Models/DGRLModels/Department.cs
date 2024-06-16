using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Department
{

    public int Id { get; set; }
    [DisplayName("Chuyên ngành"),
        MaxLength(50, ErrorMessage ="Độ dài không được quá 50 ký tự")
     
        ]
    public string? Name { get; set; }
    [DisplayName("Số năm học")]
    [MaxLength(50,ErrorMessage = "Độ dài không được quá 50 ký tự")]
    public int? Times { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Lecturers> Lecturers { get; set; } = new List<Lecturers>();
}
