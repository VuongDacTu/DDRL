using System;
using System.Collections.Generic;

namespace PJ_DGRL.Models.DGRLModels;

public partial class LecturerInfor
{
    public int Id { get; set; }

    public string? Rank { get; set; }

    public string? Position { get; set; }

    public string? Role { get; set; }

    public string? Note { get; set; }

    public string? LecturerId { get; set; }

    public virtual Lecturers? Lecturer { get; set; }
}
