using Castle.Core.Logging;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ServicesCollection.Tool.RabbitMQ
{
    /// <summary>
    /// 消息消费
    /// </summary>
    public static class OrderMessageQueueConsumer
    {
        private static readonly IMessageCenterAppService _messageCenterAppService = IocManager.Instance.Resolve<IMessageCenterAppService>();

        /// <summary>
        /// 日志
        /// </summary>
        private static readonly ILogger _logger = NullLogger.Instance;

        // <summary>
        /// 路由key，通过路由key把消息从路由器路由到队列中去
        /// </summary>
        const string RouteKey = "OrderBusinessMessage";

        //消息队列连接工厂，由此创建连接对象
        static ConnectionFactory _connectionFactory;

        //连接对象，通过此对象连接到RabbitMQ服务器
        static IConnection _connection;

        //信道，消费者和生产者都要通过信道才能与RabbitMQ服务器进行数据交互 
        static IModel _channel;

        //路由名称（可以理解为路由器）
        static string _exchangeName;

        //消息队列名称，唯一标识队列
        static string _queueName;

        /// <summary>
        /// 连接到RabbitMQ服务器
        /// </summary>
        static void Connect()
        {
            if (_connectionFactory == null)
            {
                _connectionFactory = new ConnectionFactory()
                {
                    HostName = ZvqbConsts.RabbitHost,
                    Port = ZvqbConsts.RabbitPort,
                    UserName = ZvqbConsts.RabbitUserName,
                    Password = ZvqbConsts.RabbitPassword
                    //VirtualHost = "howdyVirtualHost"
                };
            }
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _exchangeName = "order.business.exchange";
            _queueName = "order.business.queue";

            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, true, false, null);
        }

        /// <summary>
        /// 关闭与RabbitMQ服务器
        /// </summary>
        static void Close()
        {
            _connection.Dispose();
            _channel.Dispose();
            _connectionFactory = null;
        }

        //public OrderMessageQueueConsumer()
        //{
        //    _messageCenterAppService = IocManager.Instance.Resolve<IMessageCenterAppService>();

        //    _logger = NullLogger.Instance;
        //}

        /// <summary>
        /// 消费队列，向客户端发送消息
        /// </summary>
        /// <returns></returns>
        public static void UseSendMsg(this IAppBuilder app)
        {
            try
            {
                BindAuditingPassMessage(_messageCenterAppService.PushMsgToSingleDevice);//_messageCenterAppService.PushMsgToSingleDevice
            }
            catch (Exception ex)
            {
                CreateRabbitMQLog(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                _logger.Error(ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex);
            }

        }

        /// <summary>
        /// 消费者从队列消费消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        static void ConsumeMessage<T>(Func<T, Task> func)
        {
            if (_connectionFactory == null)
            {
                throw new Exception("请先调用Connect方法连接服务器");
            }

            _channel.QueueDeclare(_queueName, true, false, false);
            _channel.QueueBind(_queueName, _exchangeName, RouteKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                T args = JsonConvert.DeserializeObject<T>(message);
                
                await func(args);
                //如果是自动应答，就不要下面这句代码
                //if (func(args))
                //{
                //    _channel.BasicAck(ea.DeliveryTag, false);
                //}
            };
            _channel.BasicConsume(_queueName, true, consumer);//false true表示自动应答
        }

        /// <summary>
        /// 发布审核通过消息
        /// </summary>
        public static void BindAuditingPassMessage(Func<PushMsgReqDto, Task> action)
        {
            Connect();
            ConsumeMessage(action);
        }

        public static void CreateRabbitMQLog(string ex)
        {
            var lst = new List<string>() { $"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}：{ex}" };

            string url = AppDomain.CurrentDomain.BaseDirectory + "Log/LogRabbitMQ/";
            if (!Directory.Exists(url))
            {
                Directory.CreateDirectory(url);//不存在则创建文件夹 
            }
            url += $"{DateTime.Now.ToString("yyyyMMdd")}.txt";

            File.AppendAllLines(url, lst);
        }
    }
}
