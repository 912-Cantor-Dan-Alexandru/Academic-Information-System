﻿using System;
using System.Collections.Generic;

namespace AcademicInfoSysAPI.Context.Models
{
    public partial class OptionalDiscipline
    {
        public OptionalDiscipline()
        {
            OptionalDisciplineLists = new HashSet<OptionalDisciplineList>();
            OptionalGrades = new HashSet<OptionalGrade>();
            Studs = new HashSet<Student>();
        }

        public int Id { get; set; }
        public int? TeacherId { get; set; }
        public int? NoStudents { get; set; }
        public bool? IsApproved { get; set; }
        public int? CoresopondingYear { get; set; }
        public int? NoCredits { get; set; }
        public string Name { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<OptionalDisciplineList> OptionalDisciplineLists { get; set; }
        public virtual ICollection<OptionalGrade> OptionalGrades { get; set; }

        public virtual ICollection<Student> Studs { get; set; }
    }
}
