using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PJ_DGRL.Areas.Admin.Models;
using PJ_DGRL.Models.DGRLModels;
using SelectPdf;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class ReportController : BaseController
    {
        private readonly DbDgrlContext _context;
        IWebHostEnvironment _env;
        public ReportController(DbDgrlContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IsActive _isActive = new IsActive();
        public IsDelete _isDelete = new IsDelete(); 
        public IActionResult Index(int? departmentId, int? semesterId, int? classId)
        {
            if(semesterId == null)
            {
                semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.IsActive == _isActive.HoatDong()).Id;
            }
            
            var @class = _context.Classes.Include(x => x.Department).Include(x => x.Students).ThenInclude(x => x.SumaryOfPoints).Where(x => x.IsDelete == _isDelete.Hien() && x.DepartmentId == departmentId && x.SumaryOfPoints.Any(sp => sp.ClassId == x.Id && sp.SemesterId == semesterId)).ToList();
            if(classId !=null)
            {
                @class = _context.Classes.Include(x => x.Department).Include(x => x.Students).ThenInclude(x => x.SumaryOfPoints).Where(x => x.Id == classId && x.SumaryOfPoints.Any(sp => sp.ClassId == x.Id && sp.SemesterId == semesterId)).ToList();
            }
            
            ViewData["Semester"] = _context.Semesters.OrderByDescending(x => x.Id).Where(x => x.IsActive == _isActive.HoatDong()).ToList();
            ViewBag.Student = _context.Students.Include(x => x.SumaryOfPoints).Include(x => x.Class).Where(x => x.Class.IsDelete == _isDelete.Hien()).Where(x => x.Class.DepartmentId == departmentId).ToList();
            ViewBag.Sum = _context.SumaryOfPoints.Include(x => x.Student).ThenInclude(x => x.Class).Where(x => x.SemesterId == semesterId).ToList();
            ViewBag.DepartmentId = departmentId;
            ViewBag.SemesterId = semesterId;
            ViewBag.Class = _context.Classes.Include(x => x.Department).Include(x => x.Students).ThenInclude(x => x.SumaryOfPoints).Where(x => x.DepartmentId == departmentId).ToList();
            ViewBag.Lecturer = _context.Lecturers.OrderBy(x => x.IsLeader).Include(x => x.Position).Where(x => x.DepartmentId == departmentId && x.IsDelete == _isDelete.Hien()).ToList();
            ViewBag.ClassId = classId;
            return View(@class);
        }
        public IActionResult ConvertToPDF(int? departmentId, int? semesterId, int? classId)
        {
            HtmlToPdf pdf = new HtmlToPdf();
            string template = System.IO.File.ReadAllText(_env.WebRootPath + @"\Report.html");
            string itemTemplate = System.IO.File.ReadAllText(_env.WebRootPath + @"\ReportItemTemplate.html");
            string item2Template = System.IO.File.ReadAllText(_env.WebRootPath + @"\ReportItem2Template.html");
            string item3Template = System.IO.File.ReadAllText(_env.WebRootPath + @"\ReportItem3Template.html");
            string department = _context.Departments.Include(x => x.Classes).FirstOrDefault(x => x.Id == departmentId || x.Classes.FirstOrDefault().Id == classId).Name;

            if (semesterId == null)
            {
                semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.IsActive == _isActive.HoatDong()).Id;
            }

            var @class = _context.Classes.Include(x => x.Department).Include(x => x.Students).ThenInclude(x => x.SumaryOfPoints).Where(x => x.IsDelete == _isDelete.Hien() && x.DepartmentId == departmentId && x.SumaryOfPoints.Any(sp => sp.ClassId == x.Id && sp.SemesterId == semesterId)).ToList();
            if (classId != null)
            {
                @class = _context.Classes.Include(x => x.Department).Include(x => x.Students).ThenInclude(x => x.SumaryOfPoints).Where(x => x.Id == classId && x.SumaryOfPoints.Any(sp => sp.ClassId == x.Id && sp.SemesterId == semesterId)).ToList();
            }
            // 4.1 Số lượng sinh viên đánh giá rèn luyện của Khoa
            List<ReportPDF> model = new List<ReportPDF>();
            int stt = 1;
            foreach(var item in @class)
            {
                var svdg = _context.Students.Include(x => x.SumaryOfPoints).Where(x => x.SumaryOfPoints.Any(x => x.SemesterId == semesterId) && x.ClassId == item.Id).ToList();

                var svkdg = _context.Students.Include(x => x.SumaryOfPoints).Where(x => !x.SumaryOfPoints.Any(x => x.SemesterId == semesterId) && x.ClassId == item.Id).ToList();
                var tsv = _context.Students.Where(x => x.ClassId == item.Id).ToList();
                string dssvkdg="";
                foreach(var i in svkdg)
                {
                    if(i.IsActive == _isActive.HoatDong())
                    {
                        dssvkdg += i.FullName + " - đang học </br>";
                    }else if(i.IsActive == _isActive.TamThoiDungHoatDong())
                    {
                        dssvkdg += i.FullName + " - bảo lưu </br>";
                    }
                    else
                    {
                        dssvkdg += i.FullName + " - nghỉ học </br>";
                    }
                    
                }
                model.Add(new ReportPDF()
                {
                    stt = stt++,
                    @class = item.Name,
                    svdg = svdg.Count(),
                    svkdg = svkdg.Count(),
                    tsv = tsv.Count(),
                    dssvkdg = dssvkdg
                });
                dssvkdg = "";
            }

            
            string itemsHtml = string.Empty;
            foreach (var item in model)
            {
                string currentItemHtml = itemTemplate;
                currentItemHtml = currentItemHtml.Replace("{{stt}}", item.stt.ToString())
                                                 .Replace("{{@class}}", item.@class)
                                                 .Replace("{{svdg}}", item.svdg.ToString())
                                                 .Replace("{{svkdg}}", item.svkdg.ToString())
                                                 .Replace("{{tsv}}", item.tsv.ToString())
                                                 .Replace("{{dssvkdg}}", item.dssvkdg);
                itemsHtml += currentItemHtml;
            }


            // 4.3.Tổng hợp xếp loại đánh giá rèn luyện của Khoa
            List<ReportPDF2> model2 = new List<ReportPDF2>();
            
            int stt2 = 1;
            foreach(var item in @class)
            {
                var students = _context.Students.Include(x => x.SumaryOfPoints).Where(x => x.ClassId == item.Id).ToList();
                var sum = students.Where(x => x.SumaryOfPoints.Any(x => x.SemesterId == semesterId)).ToList();

                model2.Add(new ReportPDF2()
                {
                    stt2 = stt2++,
                    class2 = item.Name + " (" + students.Count() +")",
                    xs = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >=90)).Count(),
                    t = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 80 && x.LastPoint <90)).Count(),
                    k = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 70 && x.LastPoint < 80)).Count(),
                    tbk = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 60 && x.LastPoint < 70)).Count(),
                    tb = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 50 && x.LastPoint < 60)).Count(),
                    y = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 40 && x.LastPoint < 50)).Count(),
                    kem = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint < 40)).Count(),
                    tlxs = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 90)).Count()*100.0/students.Count(),
                    tlt = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 80 && x.LastPoint < 90)).Count() * 100.0 / students.Count(),
                    tlk = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 70 && x.LastPoint < 80)).Count() * 100.0 / students.Count(),
                    tltbk = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 60 && x.LastPoint < 70)).Count() * 100.0 / students.Count(),
                    tltb = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 50 && x.LastPoint < 60)).Count() * 100.0 / students.Count(),
                    tly = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 40 && x.LastPoint < 50)).Count() * 100.0 / students.Count(),
                    tlkem = sum.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint < 40)).Count() * 100.0 / students.Count(),
                });;
            }

            string items2Html = string.Empty;
 
            foreach (var item in model2)
            {
                string currentItemHtml = item2Template;
                currentItemHtml = currentItemHtml.Replace("{{stt2}}", item.stt2.ToString())
                                                 .Replace("{{class2}}", item.class2)
                                                 .Replace("{{department}}", department)
                                                 .Replace("{{xs}}", item.xs.ToString())
                                                 .Replace("{{t}}", item.t.ToString())
                                                 .Replace("{{k}}", item.k.ToString())
                                                 .Replace("{{tbk}}", item.tbk.ToString())
                                                 .Replace("{{tb}}", item.tb.ToString())
                                                 .Replace("{{y}}", item.y.ToString())
                                                 .Replace("{{kem}}", item.kem.ToString())
                                                 .Replace("{{tlxs}}", String.Format("{0:F2}", item.tlxs) +"%")
                                                 .Replace("{{tlt}}", String.Format("{0:F2}", item.tlt) + "%")
                                                 .Replace("{{tlk}}", String.Format("{0:F2}", item.tlk) + "%")
                                                 .Replace("{{tltbk}}", String.Format("{0:F2}", item.tltbk) + "%")
                                                 .Replace("{{tltb}}", String.Format("{0:F2}", item.tltb) + "%")
                                                 .Replace("{{tly}}", String.Format("{0:F2}", item.tly) + "%")
                                                 .Replace("{{tlkem}}", String.Format("{0:F2}", item.tlkem) + "%");
                items2Html += currentItemHtml;
            }



            //Tổng xếp loại
            var student = _context.Students.Include(x => x.SumaryOfPoints).Include(x => x.Class).Where(x => x.Class.DepartmentId == departmentId).ToList();
            var sum1 = student.Where( x => x.SumaryOfPoints.Any(x => x.SemesterId == semesterId));
            var semester = _context.Semesters.Find(semesterId);
            var lecturer = _context.Lecturers.OrderBy(x => x.IsLeader).Include(x => x.Position).Where(x => x.DepartmentId == departmentId && x.IsDelete == _isDelete.Hien()).ToList();
            string lecturerString = string.Empty;

            foreach(var item in lecturer)
            {
                string isLeader = "";
                if(item.IsLeader == 0)
                {
                    isLeader = "Uỷ viên";
                }else if(item.IsLeader == 1)
                {
                    isLeader = "Chủ tịch Hội Đồng";
                }else
                {
                    isLeader = "Thư ký Hội Đồng";
                }
                lecturerString += "<p> - "+ item.Education +". " + item.FullName + ", " + item.Position.Name + " - " + isLeader  +"</p>";

            }
            //
            string items3Html = item3Template;
            items3Html = items3Html.Replace("{{items2}}", items2Html).Replace("{{t}}", student.Count().ToString())
                                       .Replace("{{txs}}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 90)).Count().ToString())
                                       .Replace("{{tt}}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 80 && x.LastPoint < 90)).Count().ToString())
                                       .Replace("{{tk}}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 70 && x.LastPoint < 80)).Count().ToString())
                                       .Replace("{{ttbk}}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 60 && x.LastPoint < 70)).Count().ToString())
                                       .Replace("{{ttb}}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 50 && x.LastPoint < 60)).Count().ToString())
                                       .Replace("{{ty}}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 40 && x.LastPoint < 50)).Count().ToString())
                                       .Replace("{{tkem}}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint < 40)).Count().ToString())
                                       .Replace("{{ttlxs}}", String.Format("{0:F2}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 90)).Count() * 100.0 / student.Count()) + "%")
                                       .Replace("{{ttlt}}", String.Format("{0:F2}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 80 && x.LastPoint < 90)).Count() * 100.0 / student.Count()) + "%")
                                       .Replace("{{ttlk}}", String.Format("{0:F2}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 70 && x.LastPoint < 80)).Count() * 100.0 / student.Count()) + "%")
                                       .Replace("{{ttltbk}}", String.Format("{0:F2}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 60 && x.LastPoint < 70)).Count() * 100.0 / student.Count()) + "%")
                                       .Replace("{{ttltb}}", String.Format("{0:F2}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 50 && x.LastPoint < 60)).Count() * 100.0 / student.Count()) + "%")
                                       .Replace("{{ttly}}", String.Format("{0:F2}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint >= 40 && x.LastPoint < 50)).Count() * 100.0 / student.Count()) + "%")
                                       .Replace("{{ttlkem}}", String.Format("{0:F2}", sum1.Where(x => x.SumaryOfPoints.Any(x => x.LastPoint < 40)).Count() * 100.0 / student.Count()) + "%");


            string finalHtml = "";
            if (classId == null)
            {
                finalHtml = template.Replace("{{department}}", department)
                          .Replace("{{semester}}", "HỌC KỲ " + semester.Name + "<br /> NĂM HỌC " + semester.SchoolYear)
                          .Replace("{{lecturer}}", lecturerString)
                          .Replace("{{items}}", itemsHtml)
                          .Replace("{{items3}}", items3Html)
                          .Replace("{{class}}", "");
                           

            }
            else
            {
                 finalHtml= template.Replace("{{department}}", department)
                                       .Replace("{{semester}}", "HỌC KỲ " + semester.Name + "<br /> NĂM HỌC " + semester.SchoolYear)
                                       .Replace("{{lecturer}}", lecturerString)
                                       .Replace("{{items}}", itemsHtml)
                                       .Replace("{{items2}}", items2Html)
                                       .Replace("{{items3}}", "")
                                       .Replace("{{class}}", "<div><p><b>Đại diện ban cán sự lớp</b></p></div>");
            }

            PdfDocument doc = pdf.ConvertHtmlString(finalHtml);
            var bytes = doc.Save();

            return File(bytes, "application/pdf", "Report.pdf");
        }
    }
}
