using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SubjectVM
    {
        public Nullable<int> subjectId { get; set; }
        public string subject1 { get; set; }
        public string ProfName { get; set; }
        public IList<string> ProfNames { get; set; }
        public Nullable<double> creditHours { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<int> dept_Id { get; set; }
        public Nullable<int> preSubjectId { get; set; }
        public string day { get; set; }
        public string timeFrom { get; set; }
        public string timeTo { get; set; }
    }
    public class Allsubject_VM
    {
        public IList<SubjectVM> AllData { set; get; }
        public IList<SubjectVM> StudentSub { set; get; }
    }

}
