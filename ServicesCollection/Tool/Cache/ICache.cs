using ServicesCollection.Tool.AutoFac;
using System;
using System.Collections.Generic;

namespace ServicesCollection.Tool.Cache
{
    /// <summary>
    /// 缓存
    /// </summary>
    public interface ICache : IDependency
    {
        /// <summary>
        /// 设置redis缓存
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值名</param>
        /// <param name="expireTime">过期时间</param>
        void Set(string key, string value, DateTime expireTime);

        /// <summary>
        /// 移除指定key值的redis缓存
        /// </summary>
        /// <param name="key">键名</param>
        void Remove(string key);

        /// <summary>
        /// 获取当前应用程序指定key的Cache值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 获取所有的hash键
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        List<string> GetHashKeys(string hashId);

        /// <summary>
        /// 同时获取hash的键和值
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        Dictionary<string, string> GetAllHashKeysAndValues(string hashId);
    }
}
