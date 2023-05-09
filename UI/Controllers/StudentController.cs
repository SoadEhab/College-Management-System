using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using BLL;

using DAL;
using Newtonsoft.Json;
using System.IO;
using Microsoft.SqlServer.Management.Smo;

namespace UI.Controllers
{
    public class StudentController : Controller
    {
        StudentVMRepository repo = new StudentVMRepository();
        SoadEntities db = new SoadEntities();

        // GET: Student
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult TermStauts()
        {
            var Term = new TermStatus_VM();
            string check = string.Empty;
            var regist = db.TermStauts.Where(x => x.Id == 1).FirstOrDefault();
            var drop = db.TermStauts.Where(x => x.Id == 2).FirstOrDefault();
            var today = DateTime.Now;
            if (today >= regist.From && today <= regist.To)
            {
                Term.Stauts= "regist";
                Term.From = regist.From;
                Term.To = regist.To.ToString();
            }
            if (today >= drop.From && today <= drop.To)
            {
                
                Term.Stauts = "drop";
                Term.From = drop.From;
                Term.To = drop.To.ToString();
            }
            return Json(Term, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ValidTime(TimeSpan? start1, TimeSpan? end1, string Start2, string End2, string day1, string day2)
        {
            string[] startArray = Start2.Split(new char[] { ',' });
            string[] endArray = End2.Split(new char[] { ',' });
            string[] dayArray = day2.Split(new char[] { ',' });
            bool check = true;
            if (Start2.Count() > 0 || Start2 != null)
            {
                for (int i = 0; i < startArray.Length; i++)
                {
                    TimeSpan start2, end2;
                    TimeSpan.TryParse(startArray[i], out start2);
                    TimeSpan.TryParse(endArray[i], out end2);
                    if (day1 == dayArray[i] && ((start1 < end2) && (start2 < end1) && (start1 <= end1) && (start2 <= end2) || (start1 == start2 && end1 == end2)))
                    {
                        check = false;
                        return Json(check, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        check = true;

                    }
                }
            }

            return Json(check, JsonRequestBehavior.AllowGet);

        }
        public ActionResult LogOut()
        {
            Session["UserData"] = null;
            Session.Abandon();
            return RedirectToAction("Profile");
        }
        public ActionResult Profile()
        {
            return View();
        }
        //public JsonResult AllData()
        //{

        //    var subs = repo.getAllSubjects();
        //}
        public JsonResult LoadAllSubjects(int id)
        {
            Allsubject_VM obj = new Allsubject_VM();
            var subs = repo.getAllSubjects();
            obj.AllData = subs;

            List<SubjectVM> arr = new List<SubjectVM>();
            List<SubjectVM> arr2 = new List<SubjectVM>();
            var Subject2 = db.StudentSubjects.Where(x => x.student_Id == id && x.status == 4).FirstOrDefault();
            if (Subject2 == null)
            {
                obj.StudentSub = repo.filterSubjects(id);
            }
            else
            {
                arr = repo.filterSubjects(id);
                foreach (var item in arr)
                {
                    var Subject3 = db.StudentSubjects.Where(x => x.student_Id == id && x.subject_Id == item.subjectId && x.status == 4).FirstOrDefault();
                    if (Subject3 == null)
                    {
                        arr2.Add(item);
                    }
                }
                obj.StudentSub = arr2;


            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAllSubjects2(int id)
        {
            var sub = repo.getAllSubjects();
            List<SubjectVM> arr = new List<SubjectVM>();
            foreach (var item in sub)
            {
                var Subject = db.StudentSubjects.Where(x => x.student_Id == id && x.subject_Id == item.subjectId && x.status == 4).FirstOrDefault();
                if (Subject != null)
                {
                    arr.Add(item);
                }
            }
            return Json(arr, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SignUp()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(decimal National, string Password)
        {
            bool check = repo.sign(National, Password);
            int id = db.Student.Where(x => x.nationalId == National && x.password == Password).Select(x => x.studentId).FirstOrDefault();
            if (check == true)
            {
                Session["UserData"] = id;
            }

            return Json(check, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAllGenders()
        {
            var gen = repo.getAllGenders();
            return Json(gen, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAllDepartments()
        {
            var dept = repo.getAllDepartments();
            return Json(dept, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAllLevels()
        {
            var levels = repo.getAllLevels();
            return Json(levels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddStudent(StudentVM stud)
        {
            Student obj = new Student();
            if (!db.Student.Select(x => x.nationalId).Contains(stud.nationalId))
            {
                repo.Add(stud);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Save(int[] subArray, int ID)
        {
            StudentSubjects obj = new StudentSubjects();
            foreach (var item in subArray)
            {
                obj.student_Id = ID;
                obj.subject_Id = item;
                obj.status = (int)StudentStatus.regist;
                //obj.isPassed = true;
                db.StudentSubjects.Add(obj);
                db.SaveChanges();
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(int[] subArray, int ID)
        {
            var subs = db.StudentSubjects.Where(x => x.student_Id == ID && x.status == (int)StudentStatus.regist).ToList();
            db.StudentSubjects.RemoveRange(subs);
            db.SaveChanges();
            StudentSubjects obj = new StudentSubjects();
            foreach (var item in subArray)
            {
                obj.student_Id = ID;
                obj.subject_Id = item;
                obj.status = (int)StudentStatus.regist;
                //obj.isPassed = true;
                db.StudentSubjects.Add(obj);
                db.SaveChanges();
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Drop(int[] subArray, int ID)
        {
            bool check = false;
            foreach (var item in subArray)
            {
                var subs = db.StudentSubjects.Where(x => x.student_Id == ID && x.subject_Id == item).FirstOrDefault();
                db.StudentSubjects.Remove(subs);
                int res = db.SaveChanges();
                if (res > 0)
                {
                    check = true;
                }
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveProfileImage(int id)
        {

            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];
                HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);
                var fileName = Path.GetFileName(filebase.FileName);
                var fileUrl = Server.MapPath("~/ProfilesImg/");
                string extension = Path.GetExtension(filebase.FileName);
                string newFileName = Guid.NewGuid() + extension;
                var path = Path.Combine(fileUrl, newFileName);
                filebase.SaveAs(path);
                var stu = db.Student.FirstOrDefault(x => x.studentId == id);
                stu.image = newFileName;
                db.SaveChanges();
                return Json("File Saved Successfully.");
            }
            else { return Json("No File Saved."); }
        }

    }
}