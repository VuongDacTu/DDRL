using System;
using System.Collections.Generic;

namespace PJ_DDRL.Models.DDRLModels;

public partial class Student
{
    public string Id { get; set; } = null!;

    public string? FullName { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool? Gender { get; set; }

    public int? ClassId { get; set; }

    public string? PositionId { get; set; }

    public byte? IsActive { get; set; }

    public virtual ICollection<AccountStudent> AccountStudents { get; set; } = new List<AccountStudent>();

    public virtual Class? Class { get; set; }

    public virtual ICollection<ClassAnswer> ClassAnswers { get; set; } = new List<ClassAnswer>();

    public virtual Position? Position { get; set; }

    public virtual ICollection<SelfAnswer> SelfAnswers { get; set; } = new List<SelfAnswer>();

    public virtual ICollection<SumaryOfPoint> SumaryOfPoints { get; set; } = new List<SumaryOfPoint>();
}
