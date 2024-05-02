using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Student.Controllers
{
	public class LT_DGRLController : LTBaseController
	{
		private readonly DbDgrlContext _context;
		public LT_DGRLController(DbDgrlContext context)
		{
			_context = context;
		}

		public IActionResult Index(string? studentId)
		{
            // group question
            var GroupQuestions = _context.GroupQuestions.Include(u => u.QuestionLists).ThenInclude(u => u.QuestionHisories).Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ToList();
            // lấy ra sinh viên đang đăng nhập lưu trong session
            var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));
			int semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.IsActive == 1)?.Id ?? 0;

            ViewBag.semesterId = semesterId;
            ViewBag.studentId = studentId;

            var selfAnswer = _context.SelfAnswers.Where(x => x.StudentId == studentId && x.SemesterId == semesterId).ToList();
            var Answers = _context.AnswerLists.ToList();
            foreach (var item in Answers)
            {
                item.Checked = 0;
            }

            foreach (var item in selfAnswer)
			{
				_context.AnswerLists.Where(x => x.Id == item.AnswerId).FirstOrDefault().Checked = 1;

			}
			return View(GroupQuestions);
		}
		public IActionResult submit(string studentId, Dictionary<int, int> AnswerIds, Dictionary<int, int> AnswerId)
		{
            if (ModelState.IsValid)
            {

                int semesterId = _context.Semesters.FirstOrDefault(x => x.IsActive == 1).Id;
                var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));
                // kiểm tra trạng thái acc (0: chưa đánh giá, 1: đã đánh giá, 2:không được đánh giá)
                int Iactive = _context.AccountStudents.Where(u => u.UserName == student.UserName).FirstOrDefault().IsActive.Value;

                var classAnswers = _context.ClassAnswers.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).ToList();
                if (classAnswers != null)
                {
                    // xoá tất cả đánh giá của sinh viên trong kì đang diễn ra
                    
                    _context.ClassAnswers.RemoveRange(classAnswers);
                    //// xoá điểm đánh giá trước đó
                    //var sumaryOfPoint = _context.SumaryOfPoints.Where(x => x.StudentId == student.UserName && x.SemesterId == semesterId).ToList();
                    //_context.SumaryOfPoints.RemoveRange(sumaryOfPoint);
                }
                //tạo đối tượng selfAnswer để thêm vào bảng Self Answer
                List<ClassAnswer> classAnswer = new List<ClassAnswer>();
                foreach (var item in AnswerIds)
                {
                    classAnswer.Add(new ClassAnswer
                    {
                        StudentId = studentId,
                        AnswerId = item.Value,
                        SemesterId = semesterId,
                        CreateBy = student.UserName,
                        CreateDate = DateTime.Now,
                       
                    });
                }
                foreach (var item in AnswerId)
                {
                    classAnswer.Add(new ClassAnswer
                    {
                        StudentId = studentId,
                        AnswerId = item.Value,
                        SemesterId = semesterId,
                        CreateBy = student.UserName,
                        CreateDate = DateTime.Now,
                    });

                }

                // thêm lại đánh giá của sinh viên đã chỉnh sửa
                foreach (var item in classAnswer)
                {
                    _context.ClassAnswers.Add(item);

                }


                // tính tổng điểm sinh viên tự đánh giá
                var question = _context.QuestionLists.Include(u => u.AnswerLists).ThenInclude(u => u.SelfAnswers).ToList();
                var SelfAnswers = _context.SelfAnswers.Where(u => u.StudentId == student.UserName && u.SemesterId == semesterId).ToList();

                int sum = 0;
                foreach (var item in question)
                {
                    foreach (var answer in item.AnswerLists)
                    {
                        foreach (var self in answer.SelfAnswers)
                        {
                            sum += self.Answer.AnswerScore.Value;
                            if (item.TypeQuestionId == 3)
                            {
                                break;
                            }
                        }
                        if (item.TypeQuestionId == 3)
                        {
                            break;
                        }

                    }

                }

                // tạo sumaryOfPoint và thêm vào database
                SumaryOfPoint sumaryOfPoints = new SumaryOfPoint()
                {
                    StudentId = student.UserName,
                    SemesterId = semesterId,
                    SelfPoint = sum
                };
                //_context.SumaryOfPoints.Add(sumaryOfPoints);
                _context.SaveChanges();
                return RedirectToAction("Index1","DGRL");
            }
            return RedirectToAction("Index1", "DGRL");
        }
	}
}
