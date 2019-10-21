using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sample02.Dtos.StudentDtos;
using sample02.IService;
using sample02.Model;

namespace sample02.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly IStudentService _studentService;
        private readonly IStuClassService _stuClassService;
        private readonly IMapper _mapper;
        public StudentController(IStudentService studentService,IMapper mapper,IStuClassService stuClassService)
        {
            _studentService = studentService;
            _mapper = mapper;
            _stuClassService = stuClassService;
        }

        /// <summary>
        /// 获取所有学生信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stus= await _studentService.GetAll();
            if (stus != null)
            {
                var result = _mapper.Map<List<StudentDto>>(stus);
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// 获取所有学生详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetStudentsDetail()
        {
            var stus = await _studentService.GetStudentsDetail();
            if(stus!=null)
            {
                var result = _mapper.Map<List<StudentDetail>>(stus);
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// 根据Id获取学生详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetStudentDetailById(int id)
        {
            var stu = await _studentService.GetStudentDetailById(id);
            if(stu!=null)
            {
                var result = _mapper.Map<StudentDetail>(stu);
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// 根据姓名获取学生详细信息
        /// </summary>
        /// <param name="Name">学生姓名</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetStudentDetailByStuName(string Name)
        {
            var stu = await _studentService.GetStudentDetailByStuName(Name);
            if(stu!=null)
            {
                var result = _mapper.Map<List<StudentDetail>>(stu);
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// 根据学号查询学生详细信息
        /// </summary>
        /// <param name="Number">学号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetStudentDetailByStuNumber(string Number)
        {
            var stu = await _studentService.GetStudentDetailByStuNumber(Number);
            if(stu!=null)
            {
                var result = _mapper.Map<StudentDetail>(stu);
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// 新增一条数据
        /// </summary>
        /// <param name="studentDto"></param>
        /// <returns></returns>
       [HttpPost]
       public async Task<IActionResult> AddStudent([FromBody]CreateStudentDto studentDto)
       {
            if(studentDto==null)
            {
                return BadRequest();
            }
            var stu = await _studentService.GetStudentDetailByStuNumber(studentDto.StuNum);
            if(stu!=null)
            {
                ModelState.AddModelError("StuNum", "该学号已存在");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var studto = _mapper.Map<Student>(studentDto);
            _studentService.AddT(studto);
            if(! await _studentService.Save())
            {
                return StatusCode(500, "添加学生信息出错");
            }
            return Created("",studentDto);
       }

        /// <summary>
        /// 更新学生信息
        /// </summary>
        /// <param name="id">学生id</param>
        /// <param name="modifyStudent"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateStudent(int id,[FromBody]ModifyStudentDto  modifyStudent)
        {
            if(modifyStudent==null)
            {
                return BadRequest();
            }           
            var stu = await _studentService.GetStudentDetailByStuNumber(modifyStudent.StuNum);
            if(stu.StuNum!=modifyStudent.StuNum&&stu!=null)
            {
                ModelState.AddModelError("StuNum", "该学号已存在");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stuMo = await _studentService.GetTById(id);
            if (stuMo == null)
            {
                return NotFound("未查找到该学生的信息");
            }
            var studto = _mapper.Map(modifyStudent,stuMo);
            if(! await _studentService.Save())
            {
                return StatusCode(500, "更新数据时失败");
            }
            return Created("", _mapper.Map<StudentDto>(stuMo));
        }
        
        /// <summary>
        /// 删除学生信息
        /// </summary>
        /// <param name="id">学生id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var stu = await _studentService.GetTById(id);
            if(stu==null)
            {
                return NotFound("未找到该学生");
            }
            _studentService.DeleteT(stu);
            if(!await _studentService.Save())
            {
                return StatusCode(500, "删除学生信息时出错");
            }
            return NoContent();
        }

        /// <summary>
        /// 添加学生到班级
        /// </summary>
        /// <param name="StuId">学生id</param>
        /// <param name="StuClassId">班级id</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> AddStudentToClass(int StuId,int StuClassId)
        {
            var stu = await _studentService.GetStudentDetailById(StuId);
            if(stu==null)
            {
                return BadRequest("该学生不存在");
            }
            var cls = await _stuClassService.GetTById(StuClassId);
            if(cls==null)
            {
                return BadRequest("该班级不存在");
            }
            if(stu.StuClass!=null)
            {
                return BadRequest("该学生已分班");
            }
            stu.StuClass = cls;
            var res = _mapper.Map<AddStudentToClassDto>(stu);
            var stuclass= _mapper.Map(res, stu);
            if(! await _studentService.Save())
            {
                return StatusCode(500, "添加失败");
            }
            return Created("", null);
        }
     }
}
