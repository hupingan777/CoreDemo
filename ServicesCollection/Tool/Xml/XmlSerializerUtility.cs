using ServicesCollection.Tool.Xml;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Zvqb.Application.Common
{
    public class XmlSerializerUtility : IXmlSerializerUtility
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public string Serializer(object obj)
        {
            var stream = new MemoryStream();
            var xml = new XmlSerializer(obj.GetType());
            try
            {
                //序列化对象
                xml.Serialize(stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            stream.Position = 0;
            var sr = new StreamReader(stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            stream.Dispose();


            //var regex = new Regex(" mlns:xsi=\".*\"");
            str = str.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            str = str.Replace("?>", " encoding =\"GBK\" ?>");
            return str;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="xml">xml数据</param>
        /// <returns></returns>
        public  T Deserialize<T>(string xml)
        {
            try
            {
                using (var sr = new StringReader(xml))
                {
                    var type = Activator.CreateInstance<T>().GetType();
                    var xmldes = new XmlSerializer(type);
                    return (T)xmldes.Deserialize(sr);
                }
            }
            catch(Exception)
            {
                return Activator.CreateInstance<T>();
            }
        }
    }
}
