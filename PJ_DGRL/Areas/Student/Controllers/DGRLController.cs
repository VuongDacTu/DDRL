using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;
using System.Net.WebSockets;

namespace PJ_DGRL.Areas.Student.Controllers
{
    public class DGRLController : BaseController
    {
        private readonly DbDgrlContext _context;
        public DGRLController(DbDgrlContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // group question
            var groupQuestions = _context.GroupQuestions.Include(u => u.QuestionLists).ThenInclude(u => u.QuestionHisories).Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ToList();
            // lấy ra sinh viên đang đăng nhập lưu trong session

            var ss = HttpContext.Session.GetString("StudentLogin");
            if(ss == null)
            {
                return RedirectToAction("Index","Login");
            }
            var student = JsonConvert.DeserializeObject<AccountStudent>(ss);

            // set lại checked cho Answer
            var answers = _context.AnswerLists.ToList();
            int semesterId = _context.Semesters.FirstOrDefault(x => x.DateOpenStudent <= DateTime.Now && x.DateEndStudent >= DateTime.Now)?.Id??0;
            
            var selfAnswers = _context.SelfAnswers.Where(u => u.StudentId == student.UserName && u.SemesterId == semesterId).ToList();
            foreach (var item in answers)
            {
                item.Checked = 0;
            }
            foreach (var item in selfAnswers)
            {
                _context.AnswerLists.Where(u => u.Id == item.AnswerId).FirstOrDefault().Checked = 1;

            }
            ViewBag.SemesterId = semesterId;
            ViewBag.Semester = _context.Semesters.FirstOrDefault(x => x.Id == semesterId);
            return View(groupQuestions);
        }
        public IActionResult Index1()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Submit(Dictionary<int, int> answerIds, Dictionary<int, int> answerId)
        {
            if (ModelState.IsValid)
            {

                int semesterId = _context.Semesters.FirstOrDefault(x => x.DateOpenStudent <= DateTime.Now && x.DateEndStudent >= DateTime.Now).Id;
                var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));
                // kiểm tra trạng thái acc (0: chưa đánh giá, 1: đã đánh giá, 2:không được đánh giá)
                int iActive = _context.AccountStudents.Where(u => u.UserName == student.UserName).FirstOrDefault().IsActive.Value;
                //nếu như đã đánh giá
                if (iActive == 1)
                {
                    // xoá tất cả đánh giá của sinh viên trong kì đang diễn ra
                    var selfAnswers = _context.SelfAnswers.Where(u => u.StudentId == student.UserName && u.SemesterId == semesterId).ToList();
                    _context.SelfAnswers.RemoveRange(selfAnswers);
                    // xoá điểm đánh giá trước đó
                    var sumaryOfPoint = _context.SumaryOfPoints.Where(x => x.StudentId == student.UserName && x.SemesterId == semesterId).ToList();
                    _context.SumaryOfPoints.RemoveRange(sumaryOfPoint);
                }
                //tạo đối tượng selfAnswer để thêm vào bảng Self Answer
                List<SelfAnswer> selfAnswer = new List<SelfAnswer>();
                foreach (var item in answerIds)
                {
                    selfAnswer.Add(new SelfAnswer
                    {
                        StudentId = student.UserName,
                        AnswerId = item.Value,
                        SemesterId = semesterId
                    });
                }
                foreach (var item in answerId)
                {
                    selfAnswer.Add(new SelfAnswer
                    {
                        StudentId = student.UserName,
                        AnswerId = item.Value,
                        SemesterId = semesterId
                    });

                }

                // thêm lại đánh giá của sinh viên đã chỉnh sửa
                foreach (var item in selfAnswer)
                {
                    _context.SelfAnswers.Add(item);

                }


                // tính tổng điểm sinh viên tự đánh giá
                var question = _context.QuestionLists.Include(u => u.AnswerLists).ThenInclude(u => u.SelfAnswers.Where(u => u.StudentId == student.UserName && u.SemesterId == semesterId)).ToList();
                var SelfAnswers = _context.SelfAnswers.Where(u => u.StudentId == student.UserName && u.SemesterId == semesterId).ToList();

                int sum = 0;
                foreach (var item in question)
                {
                    foreach (var answer in item.AnswerLists)
                    {
                        bool checkType = false;
                        foreach (var self in answer.SelfAnswers)
                        {

                            sum += self.Answer.AnswerScore.Value;
                            if (item.TypeQuestionId == 3)
                            {
                                checkType = true;
                                break;
                            }
                        }
                        
                        if (checkType == true)
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
                    SelfPoint = sum,
                    UpdateDate = DateTime.Now,
                    ClassId = _context.Students.FirstOrDefault(x => x.Id == student.UserName).ClassId
                };
                _context.SumaryOfPoints.Add(sumaryOfPoints);
                _context.AccountStudents.FirstOrDefault(x => x.StudentId == student.UserName).IsActive = 1;
                _context.SaveChanges();
                ViewBag.Id = student.Id;
                return RedirectToAction(nameof(Index1));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

