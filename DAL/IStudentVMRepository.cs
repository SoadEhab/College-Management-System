using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using DataModel;

namespace DAL
{
    interface IStudentVMRepository
    {
        bool Add(StudentVM stu);
        bool Update(StudentVM stu);
        StudentVM GetById(int id);
        IEnumerable<StudentVM> GetAll();
    }
    public class StudentVMRepository : IStudentVMRepository
    {
        SoadEntities db = new SoadEntities();
        public bool Add(StudentVM stu)
        {
            try
            {
                if(stu != null)
                {
                    Student obj = new Student();
                    obj.firstName = stu.firstName;
                    obj.middelName = stu.middelName;
                    obj.lastName = stu.lastName;
                    obj.surName = stu.surName;
                    obj.mobile = stu.mobile;
                    obj.dept_Id = stu.deptId;
                    obj.genderId = stu.genderId;
                    obj.image = stu.image;
                    obj.levelId = stu.levelId;
                    obj.nationalId = stu.nationalId;
                    obj.password = stu.password;
                    obj.image = "Capture.PNG";
                    db.Student.Add(obj);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<Level_VM> getAllLevels()
        {
            var gen = db.Lev().Select(x => new Level_VM { levelid = x.levelId, levelname = x.level }).ToList();
            return gen;
        }
        public IList<DepartmentVM> getAllDepartments()
        {
            var dept = db.Depts().Select(x => new DepartmentVM { deptId = x.deptId, department1 = x.department }).ToList();
            return dept;
        }
        public IList<GenderVM> getAllGenders()
        {
            var gen = db.Gend().Select(x => new GenderVM { genderId = x.genderId, gender1 = x.gender }).ToList();
            return gen;
        }
        public IEnumerable<StudentVM> GetAll()
        {
            throw new NotImplementedException();
        }

        public StudentVM GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(StudentVM stu)
        {
            try
            {
                if (stu != null)
                {
                    Student obj = new Student();
                    obj.firstName = stu.firstName;
                    obj.middelName = stu.middelName;
                    obj.lastName = stu.lastName;
                    obj.surName = stu.surName;
                    obj.mobile = stu.mobile;
                    obj.dept_Id = stu.deptId;
                    obj.genderId = stu.genderId;
                    obj.image = stu.image;
                    obj.levelId = stu.levelId;
                    obj.nationalId = stu.nationalId;
                    obj.password = stu.password;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool sign(decimal national , string password)
        {
            var check = db.Student.Where(x => x.nationalId == national && x.password == password).FirstOrDefault();
            int stuID = check.studentId;
            if (check == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IList<SubjectVM> getAllSubjects()
        {
            //var sub = db.StuSub().Where(x=>x.isActive == true && x.isPassed == true).Select(x => new SubjectVM { subject1 = x.subject , subjectId = x.subjectId , creditHours = x.creditHours , day = x.day , timeFrom = x.timeFrom.ToString() , timeTo = x.timeTo.ToString() , isActive = x.isActive }).Distinct().ToList();
            var sub = (from subject in db.Subject
                       
                       where (subject.isActive == true)
                       select new SubjectVM { subject1 = subject.subject1, subjectId = subject.subjectId, creditHours = subject.creditHours, day = subject.day, timeFrom = subject.timeFrom.ToString(), timeTo = subject.timeTo.ToString(), isActive = subject.isActive , preSubjectId=subject.preSubjectId }).Distinct().ToList();
            foreach (var item in sub)
            {
                item.ProfNames = db.ProfessorSubjects.Where(y => y.subject_Id == item.subjectId).Select(y => y.Professor.name).ToList();
                item.ProfName = item.ProfNames.Aggregate((s1, s2) => s1 + "-" + s2);
            }
            return sub;
        }
        public List<SubjectVM> filterSubjects(int id)
        {
            var sub = getAllSubjects();
            List<SubjectVM> arr = new List<SubjectVM>();
            foreach (var item in sub)
            {
                if (item.preSubjectId == null)
                {
                    var Subject = db.StudentSubjects.Where(x => x.student_Id == id && x.subject_Id == item.subjectId && x.status == 1).FirstOrDefault();
                    if (Subject == null)
                    {
                        arr.Add(item);
                    }
                }
                else
                {
                    var Subject = db.StudentSubjects.Where(x => x.student_Id == id && item.preSubjectId == x.subject_Id && x.status == 1).FirstOrDefault();
                    if (Subject != null)
                    {
                        arr.Add(item);
                    }
                }
            }
            return arr;
        }


    }

}
