using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sample02.Dtos.AchievementDtos
{
    public  class CreateAchievementDto
    {
        [Display(Name ="学生姓名"),Required(ErrorMessage ="学生姓名不能为空")]
        public string StuName { get; set; }

        [Display(Name = "课程名称"), Required(ErrorMessage = "课程名称不能为空")]
        public string CourseName { get; set; }

        [Display(Name="分数"),Required(ErrorMessage ="分数不能为空"),Range(0,100,ErrorMessage ="分数的范围是0到100之间")]
        public int Score { get; set; }
    }
}
