using System;
using System.Threading.Tasks;
using Demo.Data;
using Demo.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        private readonly ILogger<UserMainController> _logger;

        //private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="myDBContext"></param>
        /// <param name="logger"></param>
        public UserMainController(MyDBContext myDBContext, ILogger<UserMainController> logger)
        {
            _myDBContext = myDBContext;
            _logger = logger;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var userList = await _myDBContext.UserMains.AsNoTracking().ToListAsync();

            string ipStr = HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation($"客户端ip为：{ipStr}的访问了GetAll方法");

            return Ok(userList);
        }
        
        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            _myDBContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;//如果有多张表查询，这里统一设置不跟踪行为
            var userModel = await _myDBContext.UserMains.FirstOrDefaultAsync(x => x.Id == userId);

            string ipStr = HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation($"客户端ip为：{ipStr}的访问了GetUserById方法");


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

                string ipStr = HttpContext.Connection.RemoteIpAddress.ToString();

                _logger.LogInformation($"客户端ip为：{ipStr}的访问了AddOrEdit方法，并且修改了用户Id 为{model.Id}的用户信息");
            }
            else
            {
                var oldUser = await _myDBContext.UserMains.AsNoTracking().FirstOrDefaultAsync(x => x.UserName.Equals(model.UserName.Trim()));
                if (oldUser != null)
                {
                    return Ok($"已经存在名为'{oldUser.UserName}'的用户");
                }
                await _myDBContext.UserMains.AddAsync(model);

                string ipStr = HttpContext.Connection.RemoteIpAddress.ToString();

                _logger.LogInformation($"客户端ip为：{ipStr}的访问了AddOrEdit方法，并且添加了用户Id 为{model.Id}的用户");
            }
            var result = await _myDBContext.SaveChangesAsync();
            return Ok("修改成功");
        }


        /// <summary>
        /// 获取日志信息,获取指定日期的日志文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLog")]
        public async Task<IActionResult> GetLog(DateTime dt)
        {
            string uri = $"logs/log{dt:yyyyMMdd}.txt";
            string txtContent = "";
            if (System.IO.File.Exists(uri))
            {
                txtContent = await System.IO.File.ReadAllTextAsync(uri);
            }
            else
            {
                txtContent = "该日期下没有日志";
            }
            return Ok(txtContent);
        }
    }
}