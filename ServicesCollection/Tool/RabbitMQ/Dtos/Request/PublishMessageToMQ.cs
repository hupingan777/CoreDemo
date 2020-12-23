using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesCollection.Tool.RabbitMQ.Dtos.Request
{
    public class PublishMessageToMQ
    {
        /// <summary>
        /// 消息推送Id
        /// </summary>
        public long AppMsgPushId { get; set; }
    }
}
