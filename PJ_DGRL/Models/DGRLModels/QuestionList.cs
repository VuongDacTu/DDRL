﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class QuestionList
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Nội dung câu hỏi không được trống")]
    public string? ContentQuestion { get; set; }

    public int? TypeQuestionId { get; set; }

    public int? GroupQuestionId { get; set; }

    public int? OrderBy { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? UpdateBy { get; set; }

    public bool? IsEdit { get; set; }

    public virtual ICollection<AnswerList> AnswerLists { get; set; } = new List<AnswerList>();

    public virtual GroupQuestion? GroupQuestion { get; set; }

    public virtual ICollection<QuestionHisory> QuestionHisories { get; set; } = new List<QuestionHisory>();

    public virtual TypeQuestion? TypeQuestion { get; set; }
}
