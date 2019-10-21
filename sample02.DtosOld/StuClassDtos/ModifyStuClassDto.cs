using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sample02.Dtos.StuClassDtos
{
    public class ModifyStuClassDto
    {
        [Display(Name ="班级编号"),Required(ErrorMessage ="班级编号不能为空")]
        public string ClassNum { get; set; }
        [Display(Name = "班级名称"), Required(ErrorMessage = "班级名称不能为空"),MaxLength(10,ErrorMessage ="班级名称最大长度为10")]
        public string ClassName { get; set; }
    }
}
