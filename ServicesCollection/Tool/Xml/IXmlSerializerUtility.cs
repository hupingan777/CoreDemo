
using ServicesCollection.Tool.AutoFac;

namespace ServicesCollection.Tool.Xml
{
    public interface IXmlSerializerUtility : IDependency
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        string Serializer(object obj);


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="xml">xml数据</param>
        /// <returns></returns>
        T Deserialize<T>(string xml);
    }
}
