using sample02.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sample02.Dtos.StudentDtos
{
    public class CreateStudentDto
    {


        [Display(Name = "学号"), Required(ErrorMessage = "学号不能为空")]
        public string StuNum { get; set; }

        [Display(Name = "姓名"), Required(ErrorMessage = "姓名不能为空"), MaxLength(10, ErrorMessage ="最大长度不能超过10")]
        public string Name { get; set; }

        [Display(Name ="年龄"),Required(ErrorMessage ="年龄不能为空"),Range(3,6,ErrorMessage ="年龄必须在3到6岁之间")]
        public int Age { get; set; }

        [Display(Name ="性别"),Required(ErrorMessage ="性别不能为空")]
        public Gender Gender { get; set; }

        [Display(Name="出生日期"),Required(ErrorMessage ="出生日期不能为空")]
        public DateTime BirthDay { get; set; }

        [Display(Name="地址"),Required]
        public string Address { get; set; }
        
        [Display(Name="邮箱")]
        public string Email { get; set; }

        [Display(Name ="入学日期"),Required(ErrorMessage ="入学日期不能为空")]
        public DateTime EnmDate { get; set; }
    }
}
