using sample02.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sample02.Dtos.StudentDtos
{
    public  class AddStudentToClassDto
    {

        [Display(Name = "学号"), Required(ErrorMessage = "学号是必填项")]
        public string StuNum { get; set; }

        [Display(Name = "姓名"), Required(ErrorMessage = "姓名是必填项"), MaxLength(10, ErrorMessage = "姓名最大长度是10")]
        public string Name { get; set; }

        [Display(Name = "年龄"), Required(ErrorMessage = "年龄是必填项"), Range(3, 6, ErrorMessage = "年龄必须在3到6岁之间")]
        public int Age { get; set; }

        [Display(Name = "性别"), Required(ErrorMessage = "性别是必填项")]
        public Gender Gender { get; set; }

        [Display(Name = "出生日期"), Required(ErrorMessage = "出生日期是必填项")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "地址"), Required(ErrorMessage = "地址是必填项")]
        public string Address { get; set; }

        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Display(Name = "入学日期"), Required(ErrorMessage = "入学日期是必填项")]
        public DateTime EnmDate { get; set; }
        public Student Student { get; set; }
    }
}
