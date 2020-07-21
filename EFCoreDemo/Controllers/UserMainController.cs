using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Data;
using Demo.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDemo.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserMainController : ControllerBase
    {
        private readonly MyDBContext _myDBContext;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="myDBContext"></param>
        public UserMainController(MyDBContext myDBContext)
        {
            _myDBContext = myDBContext;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var userList = await _myDBContext.UserMains.ToListAsync();
            return Ok(userList);
        }
        
        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var userModel = await _myDBContext.UserMains.FirstOrDefaultAsync(x => x.Id == userId);
            return Ok(userModel);
        }

        /// <summary>
        /// 用户添加或者修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddOrEdit")]
        public async Task<IActionResult> AddOrEditUser(UserMain model)
        {
            if (model.Id > 0)
            {
                var currentModel = await _myDBContext.UserMains.FirstOrDefaultAsync(x => x.Id == model.Id);
                currentModel.UserName = model.UserName;
                currentModel.RoleId = model.RoleId;
                currentModel.Address = model.Address;
                currentModel.CreateTime = model.CreateTime;
            }
            else
            {
                var oldUser = await _myDBContext.UserMains.FirstOrDefaultAsync(x => x.UserName.Equals(model.UserName.Trim()));
                if (oldUser != null)
                {
                    return Ok($"已经存在名为'{oldUser.UserName}'的用户");
                }
                await _myDBContext.UserMains.AddAsync(model);
            }
            var result = await _myDBContext.SaveChangesAsync();
            return Ok("修改成功");
        }
    }
}