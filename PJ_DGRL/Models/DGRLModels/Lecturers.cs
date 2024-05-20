using System;
using System.Collections.Generic;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Lecturers
{
    public string Id { get; set; } = null!;

    public string? FullName { get; set; }

    public int? DepartmentId { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public byte? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public int? PositionId { get; set; }

    public byte? IsLeader { get; set; }
    public string? Education { get; set; }
    public virtual ICollection<AccountLecturer> AccountLecturers { get; set; } = new List<AccountLecturer>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<LecturerInfor> LecturerInfors { get; set; } = new List<LecturerInfor>();

    public virtual Position? Position { get; set; }
}
