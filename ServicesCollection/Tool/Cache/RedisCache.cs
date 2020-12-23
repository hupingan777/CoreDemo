using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace ServicesCollection.Tool.Cache
{
    public class RedisCache : ICache
    {
        /// <summary>
        /// 主机地址
        /// </summary>
        private readonly string Host;

        /// <summary>
        /// 端口
        /// </summary>
        private readonly int Port;

        /// <summary>
        /// 连接密码
        /// </summary>
        private readonly string Password;

        /// <summary>
        /// 存储库索引
        /// </summary>
        private readonly long DBIndex;

        public IConfiguration _configuration;

        public RedisCache(IConfiguration configuration)
        {
            _configuration = configuration;
            if (_configuration != null)
            {
                Host = _configuration["Redis:Host"]?.ToString();
                Port = int.Parse(_configuration["Redis:Port"]?.ToString());
                Password = _configuration["Redis:Password"]?.ToString();
                DBIndex = long.Parse(_configuration["Redis:DBIndex"]?.ToString());
            }
        }

        /// <summary>
        /// 设置redis缓存
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值名</param>
        /// <param name="expireTime">过期时间</param>
        public void Set(string key, string value, DateTime expireTime)
        {
            using (var redisClient = new RedisClient(Host, Port, Password, DBIndex))
            {
                redisClient.Set(key, value, expireTime);
            }
        }

        /// <summary>
        /// 移除指定key值的redis缓存
        /// </summary>
        /// <param name="key">键名</param>
        public void Remove(string key)
        {
            using (var redisClient = new RedisClient(Host, Port, Password, DBIndex))
            {
                redisClient.Remove(key);
            }
        }

        /// <summary>
        /// 获取当前应用程序指定key的Cache值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            using (var redisClient = new RedisClient(Host, Port, Password, DBIndex))
            {
                return redisClient.Get<T>(key);
            }
        }

        /// <summary>
        /// 获取所有的hash键
        /// </summary>
        /// <param name="hashId">就是值哪个hash。Redis查看Key的类型用：type key</param>
        /// <returns></returns>
        public List<string> GetHashKeys(string hashId)
        {
            using (var redisClient = new RedisClient(Host, Port, Password, DBIndex))
            {
                return redisClient.GetHashKeys(hashId);
            }
        }

        //获取所有的hash值同理

        /// <summary>
        /// 同时获取hash的键和值
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public Dictionary<string,string> GetAllHashKeysAndValues(string hashId)
        {
            using (var redisClient = new RedisClient(Host, Port, Password, DBIndex))
            {
                return redisClient.GetAllEntriesFromHash(hashId);
            }
        }

    }
}
