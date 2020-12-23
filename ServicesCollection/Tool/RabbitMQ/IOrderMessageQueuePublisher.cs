using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesCollection.Tool.RabbitMQ
{
    public interface IOrderMessageQueuePublisher : ITransientDependency
    {
        /// <summary>
        /// 将需要推送的消息加入队列
        /// </summary>
        /// <param name="model"></param>
        void PublishOrderMessageToMQ(AppMsgPushMain model);

        /// <summary>
        /// 将需要推送的消息加入队列（多条）
        /// </summary>
        /// <param name="lst"></param>
        void PublishOrderMessageToMQ(List<AppMsgPushMain> lst);
    }
}
