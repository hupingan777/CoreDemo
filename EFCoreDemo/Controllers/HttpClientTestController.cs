using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EFCoreDemo.Controllers
{
    /// <summary>
    /// HttpClient接口调用测试
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HttpClientTestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public HttpClientTestController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        /// <summary>
        /// 发送Post请求 
        /// </summary>
        /// <returns></returns>
        [Route("SendRequest")]
        [HttpPost]
        public async Task<IActionResult> SendRequest(string url)
        {
            using (var client = new HttpClient())
            {
                var userMain = new UserMain();
                userMain.UserName = "HttpClientPost测试";
                userMain.Address = "微配互联网";
                userMain.CreateTime = DateTime.Now;
                userMain.RoleId = 0;

                var contentStr = Newtonsoft.Json.JsonConvert.SerializeObject(userMain);

                var sc = new StringContent(contentStr, Encoding.UTF8, "application/json");
                var postResult = await client.PostAsync(url, sc);//http://118.126.109.247:8088/api/UserMain/AddOrEdit

                #region 通过流方式获取数据
                //var resultStream = await postResult.Content.ReadAsStreamAsync();
                //using (var reader = new StreamReader(resultStream, Encoding.UTF8))
                //{
                //    var result = reader.ReadToEnd();
                //    return Ok(result);
                //}
                #endregion

                #region 直接获取数据
                //var resultStr = await postResult.Content.ReadAsStringAsync();
                //return Ok(resultStr);
                #endregion

                #region byte数组
                var resultByte = await postResult.Content.ReadAsByteArrayAsync();
                //var resultStr = Encoding.GetEncoding("UTF-8").GetString(resultByte);
                var resultStr = Encoding.UTF8.GetString(resultByte);
                return Ok(resultStr);
                #endregion


            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [Route("GetTokenTest")]
        [HttpPost]
        public async Task<IActionResult> GetTokenTest(UserLoginReqDto model)
        {
            var url = "http://app.yundingdan.com.cn/api/services/app/userMain/Login";
            //string param = @"{\"userName\": \""13527330751\"",\""password\"": \""e10adc3949ba59abbe56e057f20f883e\"",\""userBranchId\"": 1711181900438064,\""channelId\"": \""4033753001137996875\""}";
            UserLoginReqDto userLoginReqDto = new UserLoginReqDto();
            userLoginReqDto.UserName = model.UserName;
            userLoginReqDto.Password = "e10adc3949ba59abbe56e057f20f883e";
            userLoginReqDto.UserBranchId = model.UserBranchId;
            userLoginReqDto.ChannelId = model.ChannelId;

            string param = JsonConvert.SerializeObject(userLoginReqDto);

            StringContent stringContent = new StringContent(param, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var postResult = await client.PostAsync(url, stringContent);

                var strResult = await postResult.Content.ReadAsStringAsync();
                return Ok(strResult);
            }
        }

        /// <summary>
        /// 根据token访问数据
        /// </summary>
        /// <returns></returns>
        [Route("PostByToken")]
        [HttpPost]
        public async Task<IActionResult> PostByToken(string token)
        {
            var url = "http://app.yundingdan.com.cn/api/services/app/branchAddressList/GetBranchAddressList";
            StringContent stringContent = new StringContent("", Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer {token}");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            //stringContent.Headers.Add("Authorization", "Bearer " + token);

            var postResult = await client.PostAsync(url, stringContent);

            var strResult = await postResult.Content.ReadAsStringAsync();
            return Ok(strResult);
        }
    }

    public class UserLoginReqDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 企业Id
        /// </summary>
        public long UserBranchId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public string ChannelId { get; set; }
    }
}