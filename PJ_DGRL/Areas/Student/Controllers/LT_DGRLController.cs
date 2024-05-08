using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            var groupQuestions = _context.GroupQuestions.Include(u => u.QuestionLists).ThenInclude(u => u.QuestionHisories).Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ToList();
            // lấy ra sinh viên đang đăng nhập lưu trong session
            var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));
			int semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.DateEndStudent < DateTime.Now && x.DateEndClass >= DateTime.Now)?.Id ?? 0;

            if (semesterId == 0)
            {
                return RedirectToAction("Status1","LT_DGRL",new {id = studentId});
            }

            var selfAnswer = _context.SelfAnswers.Where(x => x.StudentId == studentId && x.SemesterId == semesterId).ToList();
            var answers = _context.AnswerLists.ToList();
            // kiểm tra xem nếu sinh viên đã đánh giá thì mới hiển thị
            int selfPoint = _context.SumaryOfPoints.Where(x => x.StudentId == studentId).FirstOrDefault(x => x.SemesterId == semesterId)?.SelfPoint ?? 0;
            if (selfPoint == 0)
            {
                return RedirectToAction("Status1", new { id = studentId });
            }
            foreach (var item in answers)
            {
                item.Checked = 0;
            }

            foreach (var item in selfAnswer)
			{
				_context.AnswerLists.Where(x => x.Id == item.AnswerId).FirstOrDefault().Checked = 1;

			}
            ViewBag.Student = _context.Students.Where(x => x.Id == studentId).FirstOrDefault();
            ViewBag.semesterId = semesterId;
            ViewBag.Id = studentId;
            return View(groupQuestions);
		}
		public IActionResult submit(string studentId, Dictionary<int, int> AnswerIds, Dictionary<int, int> AnswerId)
		{
            if (ModelState.IsValid)
            {

                int semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.DateEndStudent <= DateTime.Now && x.DateEndClass >= DateTime.Now)?.Id ?? 0;

                var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));
                // kiểm tra trạng thái acc (0: chưa đánh giá, 1: đã đánh giá, 2:không được đánh giá)
                int Iactive = _context.AccountStudents.Where(u => u.UserName == student.UserName).FirstOrDefault().IsActive.Value;

                var classAnswers = _context.ClassAnswers.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).ToList();
                if (classAnswers != null)
                { 
                    _context.ClassAnswers.RemoveRange(classAnswers);

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
                int sum = 0;
                foreach (var item in question)
                {
                    foreach (var answer in item.AnswerLists)
                    {
                        bool check= false;
                        foreach (var self in answer.ClassAnswers)
                        {
                            sum += self.Answer.AnswerScore.Value;
                            if (item.TypeQuestionId == 3)
                            {
                                check = true;
                                break;
                            }
                        }
                        if (check == true)
                        {
                            break;
                        }

                    }

                }

                var point = _context.SumaryOfPoints.Where(x => x.StudentId == studentId).FirstOrDefault(x => x.SemesterId == semesterId);
                if(point != null)
                {
                    point.ClassPoint = sum;
                    point.UserClass = student.UserName;
                    point.UpdateDate = DateTime.Now;
                }
                _context.SaveChanges();
                return RedirectToAction("Index", "LT_DGRL");
            }
            return RedirectToAction("Status", "LT_DGRL");
        }
        public IActionResult Status(string? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult Status1(string? id)
        {
            int semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.DateEndStudent < DateTime.Now && x.DateEndClass >= DateTime.Now)?.Id ?? 0;
            if (semesterId == 0)
            {
                ViewBag.Status = "Kì đánh giá chưa diễn ra hoặc đã kết thúc";

            }else
            {
                ViewBag.Status = "Không thể đánh giá do sinh viên chưa tự đánh giá";
            }
            ViewBag.Id = id;
            return View();
        }
    }
}
