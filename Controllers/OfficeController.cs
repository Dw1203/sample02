using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sample02.Dtos.OfficeDtos;
using sample02.IService;
using sample02.Model;

namespace sample02.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        private readonly IMapper _mapper;
        public OfficeController(IOfficeService officeService,IMapper mapper)
        {
            _officeService = officeService;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加办公室信息
        /// </summary>
        /// <param name="createOfficeDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddOffice([FromBody] CreateOfficeDto createOfficeDto)
        {
            if(createOfficeDto ==null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(await _officeService.OfficeNameIsExist(createOfficeDto.Name))
            {
                return BadRequest("办公室名称已存在");
            }
            if(await _officeService.OfficeNumIsExist(createOfficeDto.OfficeNum))
            {
                return BadRequest("办公室编号已存在");
            }
            var ofs= _mapper.Map<Offices>(createOfficeDto);
            _officeService.AddT(ofs);
            if(!await _officeService.Save())
            {
                return StatusCode(500, "添加办公室信息失败");
            }
            return Created("", createOfficeDto);
        }

        /// <summary>
        /// 获取所有办公室信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllOffice()
        {
            var ofs = await _officeService.GetAll();
            if(ofs.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<OfficeDto>>(ofs);
            return Ok(res);
        }

        /// <summary>
        /// 根据办公室名称获取办公室信息
        /// </summary>
        /// <param name="OfficeName">办公室名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOfficeByOfficeName(string OfficeName)
        {
            var ofs = await _officeService.GetOfficesByOfficeName(OfficeName);
            if (ofs.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<OfficeDto>>(ofs);
            return Ok(res);
            
        }

        /// <summary>
        /// 根据办公室编号获取办公室信息
        /// </summary>
        /// <param name="OfficeNum"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOfficeByOfficeNum(string OfficeNum)
        {
            var ofs = await _officeService.GetOfficesByOfficeNum(OfficeNum);
            if (ofs.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<OfficeDto>>(ofs);
            return Ok(res);
        }

        /// <summary>
        /// 修改办公室信息
        /// </summary>
        /// <param name="id">办公室id</param>
        /// <param name="modifyOfficeDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateOffice(int id,[FromBody]ModifyOfficeDto modifyOfficeDto)
        {
            if(modifyOfficeDto==null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ofs = await _officeService.GetTById(id);
            if(ofs==null)
            {
                return NotFound();
            }
            if(ofs.Name!=modifyOfficeDto.Name)
            {
                if(await _officeService.OfficeNameIsExist(modifyOfficeDto.Name))
                {
                    return BadRequest("办公室名称已存在");
                }
            }
            if(ofs.OfficeNum!=modifyOfficeDto.OfficeNum)
            {
                if(await _officeService.OfficeNumIsExist(modifyOfficeDto.OfficeNum))
                {
                    return BadRequest("办公室编号已存在");
                }
            }
            ofs.Name = modifyOfficeDto.Name;
            ofs.OfficeNum = modifyOfficeDto.OfficeNum;
            ofs.Address = modifyOfficeDto.Address;
            _mapper.Map(ofs,ofs);
            if(!await _officeService.Save())
            {
                return StatusCode(500, "修改办公室信息时出错");
            }
            return Created("", modifyOfficeDto);
        }

        /// <summary>
        /// 删除办公室
        /// </summary>
        /// <param name="id">办公室id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteOffice(int id)
        {
            var ofs =await _officeService.GetTById(id);
            if(ofs==null)
            {
                return NotFound();
            }
            _officeService.DeleteT(ofs);
            if(!await _officeService.Save())
            {
                return StatusCode(500, "删除办公室信息时出错");
            }
            return NoContent();
        }

    }
}