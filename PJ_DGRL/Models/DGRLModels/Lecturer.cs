using System;
using System.Collections.Generic;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Lecturer
{
    public string Id { get; set; } = null!;

    public string? FullName { get; set; }

    public int? DepartmentId { get; set; }

    public string? PositionId { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public byte? IsActive { get; set; }

    public virtual ICollection<AccountLecturer> AccountLecturers { get; set; } = new List<AccountLecturer>();

    public virtual Department? Department { get; set; }

    public virtual Position? Position { get; set; }
}
