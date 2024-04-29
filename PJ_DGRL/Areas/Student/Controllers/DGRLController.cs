using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Student.Controllers
{
	public class DGRLController : BaseController
	{
		private readonly DbDgrlContext _context;
		public DGRLController(DbDgrlContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index(int semesterId)
		{
			// group question
			var GroupQuestions = _context.GroupQuestions.Include(u => u.QuestionLists).ThenInclude(u => u.AnswerLists).ToList();
			// lấy ra sinh viên đang đăng nhập lưu trong session
			var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));

			// kiểm tra trạng thái acc
            int Iactive = _context.AccountStudents.Where(u => u.UserName == student.UserName).FirstOrDefault().IsActive.Value;
            ViewBag.Iactive = Iactive;
            ViewBag.semesterId = semesterId;
            return View(GroupQuestions);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(int semesterId, Dictionary<int, int> AnswerIds, Dictionary<int, int> AnswerId)
		{
			if (ModelState.IsValid)
			{

				var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));

				// tạo đối tượng selfAnswer để thêm vào bảng Self Answer
				List<SelfAnswer> selfAnswer = new List<SelfAnswer>();
				foreach (var item in AnswerIds)
				{
					selfAnswer.Add(new SelfAnswer
					{
						StudentId = student.UserName,
						AnswerId = item.Value,
						SemesterId = semesterId,
                    });
				}
				foreach (var item in AnswerId)
				{
					selfAnswer.Add(new SelfAnswer
					{
						StudentId = student.UserName,
						AnswerId = item.Value,
                        SemesterId = semesterId,
                    });

				}

				foreach (var item in selfAnswer)
				{
					_context.SelfAnswers.Add(item);
                    _context.AnswerLists.Where(u => u.Id == item.AnswerId).FirstOrDefault().Checked = 1;

                }
				// tính tổng điểm sinh viên tự đánh giá
				var question = _context.QuestionLists.Include(u => u.AnswerLists).ThenInclude(u => u.SelfAnswers).ToList();
				int sum = 0;
				foreach(var item in question)
				{
					var Answer = item.AnswerLists.Where(u => u.Checked == 1 && u.Status == 1).ToList();
					if(item.TypeQuestionId == 2)
					{
                            sum += Answer.FirstOrDefault()?.AnswerScore.Value ?? 0;
                    }
					else
					{
						foreach(var i in Answer)
						{
							sum += i.AnswerScore.Value;
						}
					}
				}
				// tạo sumaryOfPoint và thêm vào database
                SumaryOfPoint sumaryOfPoint = new SumaryOfPoint() { 
					StudentId = student.UserName,
					SemesterId = semesterId,
					SelfPoint = sum
				};
				_context.SumaryOfPoints.Add(sumaryOfPoint);
				
                _context.AccountStudents.Where(u => u.UserName == student.UserName).FirstOrDefault().IsActive = 1; // set cho account sinh viên về trạng thái đã đánh giá
                _context.SaveChanges();
				return RedirectToAction("Index1");
			}
			return RedirectToAction("Index1");

		}
		public IActionResult Index1()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Update(int? semesterId, Dictionary<int, int> AnswerIds, Dictionary<int, int> AnswerId)
		{
			if (ModelState.IsValid)
			{

				var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));
				//tạo đối tượng selfAnswer để thêm vào bảng Self Answer
				List<SelfAnswer> selfAnswer = new List<SelfAnswer>();
				foreach (var item in AnswerIds)
				{
					selfAnswer.Add(new SelfAnswer
					{
						StudentId = student.UserName,
						AnswerId = item.Value,
						SemesterId = semesterId
					});
				}
				foreach (var item in AnswerId)
				{
					selfAnswer.Add(new SelfAnswer
					{
						StudentId = student.UserName,
						AnswerId = item.Value,
                        SemesterId = semesterId
                    });

				}

				var Answers = _context.AnswerLists.ToList();

				// set lại checked cho Answer

                var SelfAnswers = _context.SelfAnswers.Where(u => u.StudentId == student.UserName && u.SemesterId == semesterId).ToList();
                foreach (var item in Answers)
                {
					item.Checked = 0;
                }
				// xoá tất cả đánh giá của sinh viên trong kì đang diễn ra
                var selfAnswers = _context.SelfAnswers.Where(u => u.StudentId == student.UserName && u.SemesterId == semesterId).ToList();
                _context.SelfAnswers.RemoveRange(selfAnswers);

				// thêm lại đánh giá của sinh viên đã chỉnh sửa
                foreach (var item in selfAnswer)
                {
                    _context.SelfAnswers.Add(item);
                    _context.AnswerLists.Where(u => u.Id == item.AnswerId).FirstOrDefault().Checked = 1;

                }
				// xoá điểm đánh giá trước đó
				var sumaryOfPoint = _context.SumaryOfPoints.Where(x => x.StudentId == student.UserName && x.SemesterId == semesterId).ToList();
				_context.SumaryOfPoints.RemoveRange(sumaryOfPoint);
                // tính tổng điểm sinh viên tự đánh giá
                var question = _context.QuestionLists.Include(u => u.AnswerLists).ThenInclude(u => u.SelfAnswers).ToList();
                int sum = 0;
                foreach (var item in question)
                {
                    var Answer = item.AnswerLists.Where(u => u.Checked == 1 && u.Status == 1).ToList();
                    if (item.TypeQuestionId == 3)
                    {
                        sum += Answer.FirstOrDefault()?.AnswerScore.Value ?? 0;
                    }
                    else
                    {
                        foreach (var i in Answer)
                        {
                            sum += i.AnswerScore.Value;
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
                _context.SumaryOfPoints.Add(sumaryOfPoints);
                _context.SaveChanges();
				return RedirectToAction(nameof(Index1));
			}
			return RedirectToAction(nameof(Index1));
		}
	}
}
