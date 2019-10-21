using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sample02.Dtos.StudentCourseDtos
{
   public class CreateStudentCourseDto
    {
        [Display(Name ="学生姓名"),Required(ErrorMessage ="学生姓名是必须的")]
        public string StudentName { get; set; }

        [Display(Name = "课程名称"), Required(ErrorMessage = "课程名称是必须的")]
        public string CourseName { get; set; }
    }
}
