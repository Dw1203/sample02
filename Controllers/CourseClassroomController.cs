using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sample02.Dtos.CourseClassroomDtos;
using sample02.IService;
using sample02.Model;

namespace sample02.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseClassroomController : ControllerBase
    {
        private readonly ICourseClassroomService _courseClassroomService;
        private readonly ICourseService _courseService;
        private readonly IClassroomService _classroomService;
        private readonly IMapper _mapper;
        public CourseClassroomController(ICourseClassroomService courseClassroomService,IMapper mapper,ICourseService courseService
            ,IClassroomService classroomService)
        {
            _courseClassroomService = courseClassroomService;
            _mapper = mapper;
            _courseService = courseService;
            _classroomService = classroomService;
        }

        /// <summary>
        /// 给课程添加教室
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddCourseClassroom([FromBody] CreateCourseClassroomDto create)
        {
            if(create==null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var crs =await _courseService.GetCourseByCourseNameDetail(create.CourseName);
            var cls =await _classroomService.GetClassroomsByClassName(create.ClassroomName);
            if(crs.Count==0 || cls.Count==0)
            {
                return BadRequest("课程名称或者教室名称不存在");
            }
            CourseClassroom classroom = new CourseClassroom()
            {
                Course = crs.FirstOrDefault(),
                Classroom = cls.FirstOrDefault()
            };
            _courseClassroomService.AddT(classroom);
            if(!await _courseClassroomService.Save())
            {
                return StatusCode(500, "添加数据时错误");
            }
            return Created("", create);
        }

        /// <summary>
        /// 获取所有课程教室信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCourseClassroom()
        {
            var courseClassroom = await _courseClassroomService.GetAllCourseClassrooms();
            if(courseClassroom.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<CourseClassroomDto>>(courseClassroom);
            return Ok(res);
        }

        /// <summary>
        /// 根据课程名称获取课程教室信息
        /// </summary>
        /// <param name="CourseName">课程名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCourseClassroomByCourseName(string CourseName)
        {
            var cls = await _courseClassroomService.GetCourseClassroomsByCourseName(CourseName);
            if(cls.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<CourseClassroomDto>>(cls);
            return Ok(res);
        }

        /// <summary>
        /// 根据教室名称获取课程教室信息
        /// </summary>
        /// <param name="ClassroomName">教室名称</param>
        /// <returns></returns>
       [HttpGet]
       public async Task<IActionResult> GetCourseClassroomByClsName(string ClassroomName)
        {
            var cls = await _courseClassroomService.GetCourseClassroomsByClassroomName(ClassroomName);
            if (cls.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<CourseClassroomDto>>(cls);
            return Ok(res);
        }

        /// <summary>
        /// 删除教室课程信息
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCourseClassroom(int id)
        {
            var cls = await _courseClassroomService.GetTById(id);
            if(cls==null)
            {
                return NotFound();
            }
            _courseClassroomService.DeleteT(cls);
            if(! await _courseClassroomService.Save())
            {
                return StatusCode(500, "删除教室课程信息时出错");
            }
            return NoContent();
        }
    }
} 