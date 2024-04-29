﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Student.Controllers
{
    public class ResultDGRLController : BaseController
    {
        private readonly DbDgrlContext _context;
        public ResultDGRLController(DbDgrlContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? semesterId)
        {
            var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));
            var SelfAnwser = _context.SelfAnswers.ToList();
            var GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ToList();
            ViewBag.SumSelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == student.UserName && u.SemesterId == semesterId).FirstOrDefault().SelfPoint;
            return View(GroupQuestion);
        }
    }
}
