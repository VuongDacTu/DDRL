﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Classify
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? PointMin { get; set; }

    public int? PointMax { get; set; }

    public int? OrderBy { get; set; }
}
