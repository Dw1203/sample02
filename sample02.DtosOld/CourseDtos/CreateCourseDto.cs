using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sample02.Dtos.CourseDtos
{
    public class CreateCourseDto
    {
        [Display(Name="课程编号"),Required(ErrorMessage ="课程编号不能为空")]
        public string CourseNum { get; set; }

        [Display(Name = "课程名称"), Required(ErrorMessage = "课程名称不能为空")]
        public string CourseName { get; set; }

        [Display(Name = "课程学分"), Required(ErrorMessage = "课程学分不能为空")]
        public int Score { get; set; }

        [Display(Name = "课程课时"), Required(ErrorMessage = "课程课时不能为空")]
        public double CourseTime { get; set; }

     
    }
}
