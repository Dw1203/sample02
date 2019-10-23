using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sample02.Dtos.PositionDtos;
using sample02.IService;
using sample02.Model;

namespace sample02.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;
        private readonly IMapper _mapper;
        public PositionController(IPositionService positionService,IMapper mapper)
        {
            _positionService = positionService;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加职位
        /// </summary>
        /// <param name="createPosition"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddPosition([FromBody]CreatePositionDto createPosition)
        {
            if(createPosition==null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(await _positionService.PosNameIsEixst(createPosition.Name))
            {
                return BadRequest("职业名称已存在");
            }
            if(await _positionService.PosNumIsEixst(createPosition.PositionNum))
            {
                return BadRequest("职业编号已存在");
            }
            var pos = _mapper.Map<Position>(createPosition);
            _positionService.AddT(pos);
            if(!await _positionService.Save())
            {
                return StatusCode(500, "添加职位信息时出错");
            }
            return Created("", createPosition);
        }

        /// <summary>
        /// 获取所有职位信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPosition()
        {
            var pos = await _positionService.GetAll();
            if(pos.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<PositionDto>>(pos);
            return Ok(res);
        }

        /// <summary>
        /// 获取所有职位详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPositionDetail()
        {
            var pos = await _positionService.GetAllPositionsDetail();
            if(pos.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<PositionDetailDto>>(pos);
            return Ok(res);
        }

        /// <summary>
        /// 根据职位名称获取职位信息
        /// </summary>
        /// <param name="PosName">职位名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPosByPosName(string PosName)
        {
            var pos = await _positionService.GetPositionsByPosName(PosName);
            if(pos.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<PositionDetailDto>>(pos);
            return Ok(res);
        }

        /// <summary>
        /// 根据职位编号获取职位名称
        /// </summary>
        /// <param name="PosNum">职位编号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPosByPosNum(string PosNum)
        {
            var pos = await _positionService.GetPositionsByPosNum(PosNum);
            if (pos.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<PositionDetailDto>>(pos);
            return Ok(res);
        }

        /// <summary>
        /// 修改职位信息
        /// </summary>
        /// <param name="id">位置id</param>
        /// <param name="modifyPosition"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdatePosition(int id,[FromBody]ModifyPositionDto modifyPosition)
        {
            if(modifyPosition==null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pos = await _positionService.GetTById(id);
            if(pos==null)
            {
                return NotFound();
            }
            if(pos.Name != modifyPosition.Name)
            {
                if(await _positionService.PosNameIsEixst(modifyPosition.Name))
                {
                    return BadRequest("职位名称已存在");
                }
            }
            if(pos.PositionNum!=modifyPosition.PositionNum)
            {
                if(await _positionService.PosNumIsEixst(modifyPosition.PositionNum))
                {
                    return BadRequest("职位编号已存在");
                }
            }
            pos.Name = modifyPosition.Name;
            pos.PositionNum = modifyPosition.PositionNum;
            pos.Remark = modifyPosition.Remark;
            _mapper.Map(pos, pos);
            if(! await _positionService.Save())
            {
                return StatusCode(500, "修改职位信息时出错");
            }
            return Created("", modifyPosition);
        }

        /// <summary>
        /// 删除职位信息
        /// </summary>
        /// <param name="id">职位id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var pos = await _positionService.GetTById(id);
            if(pos==null)
            {
                return NotFound();
            }
            _positionService.DeleteT(pos);
            if(!await _positionService.Save())
            {
                return StatusCode(500, "删除职位信息时出错");
            }
            return NoContent();
        }
    }
}