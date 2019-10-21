
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using sample02.Dtos.ClassroomDtos;
using sample02.IService;
using sample02.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sample02.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly IClassroomService _classroomService;
        private readonly IMapper _mapper;

        public ClassroomController(IClassroomService classroomService,IMapper mapper)
        {
            _classroomService = classroomService;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加教室信息
        /// </summary>
        /// <param name="createClassroomDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddClassroom([FromBody] CreateClassroomDto createClassroomDto)
        {
            if(createClassroomDto==null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cls =await _classroomService.GetClassroomsByClassName(createClassroomDto.Name);
            var cls1 =await _classroomService.GetClassroomsByClassNum(createClassroomDto.ClassRoomNum);
            if(cls.Count>0||cls1.Count>0)
            {
                return BadRequest("教室名称或教室编号已存在");
            }
            var res = _mapper.Map<Classroom>(createClassroomDto);
            _classroomService.AddT(res);
            if(!await _classroomService.Save())
            {
                return StatusCode(500, "添加教室信息时出错");
            }
            return Created("", createClassroomDto);
        }


        /// <summary>
        /// 获取所有教室信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetClassroom()
        {
            var classroom = await _classroomService.GetAll();
            if(classroom.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<ClassroomDto>>(classroom);
            return Ok(res);
        }

        /// <summary>
        /// 获取全部教室的详细信息
        /// </summary>
        /// <returns></returns>
       [HttpGet]
       public async Task<IActionResult> GetAllClassroomDetail()
        {
            var cls = await _classroomService.GetClassroomsDetail();
            if(cls.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<ClassroomDetailDto>>(cls);
            return Ok(res);
        }
    }
}