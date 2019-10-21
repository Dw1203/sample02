
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

        /// <summary>
        /// 更新教室信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifyClassroom"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateClassroom(int id,[FromBody] ModifyClassroom modifyClassroom)
        {
            if(modifyClassroom==null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cls =await _classroomService.GetTById(id);
            if(cls==null)
            {
                return NotFound();
            }
            if(cls.Name!=modifyClassroom.Name)
            {
                var cls1 =await _classroomService.GetClassroomsByClassName(modifyClassroom.Name);
                if(cls1.Count>0)
                {
                    return BadRequest("教室名称已存在");
                }
            }
            if(cls.ClassRoomNum!=modifyClassroom.ClassRoomNum)
            {
                var cls2 = await _classroomService.GetClassroomsByClassNum(modifyClassroom.ClassRoomNum);
                if(cls2.Count>0)
                {
                    return BadRequest("教室编号已存在");
                }
            }
            cls.Name = modifyClassroom.Name;
            cls.ClassRoomNum = modifyClassroom.ClassRoomNum;
            cls.Address = modifyClassroom.Address;
            _mapper.Map(cls, cls);
            if(!await _classroomService.Save())
            {
                return StatusCode(500, "更新数据时出错");
            }
            return Created("", modifyClassroom);
        }

        /// <summary>
        /// 删除教室信息
        /// </summary>
        /// <param name="id">教室id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            var cls = await _classroomService.GetTById(id);
            if(cls==null)
            {
                return NotFound();
            }
            _classroomService.DeleteT(cls);
            if(!await _classroomService.Save())
            {
                return StatusCode(500, "删除数据时出错");
            }
            return NoContent();
        }
    }
}