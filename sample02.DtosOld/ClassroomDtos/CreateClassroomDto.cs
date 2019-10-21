using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sample02.Dtos.ClassroomDtos
{
   public class CreateClassroomDto
    {
        [Display(Name="教室编号"),Required(ErrorMessage ="教室编号不能为空")]
        public string ClassRoomNum { get; set; }

        [Display(Name = "教室名称"), Required(ErrorMessage = "教室名称不能为空")]
        public string Name { get; set; }

        [Display(Name = "教室地址"), Required(ErrorMessage = "教室地址不能为空")]
        public string Address { get; set; }
    }
}
