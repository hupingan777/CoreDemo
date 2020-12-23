using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesCollection.Tool.RabbitMQ
{
    /// <summary>
    /// 业务消息队列服务
    /// </summary>
    public class OrderMessageQueueClient
    {
        /// <summary>
        /// 路由key，通过路由key把消息从路由器路由到队列中去
        /// </summary>
        protected const string RouteKey = "OrderBusinessMessage";

        //消息队列连接工厂，由此创建连接对象
        ConnectionFactory _connectionFactory;

        //连接对象，通过此对象连接到RabbitMQ服务器
        IConnection _connection;

        //信道，消费者和生产者都要通过信道才能与RabbitMQ服务器进行数据交互 
        IModel _channel;

        //路由名称（可以理解为路由器）
        string _exchangeName;

        //消息队列名称，唯一标识队列
        string _queueName;

        /// <summary>
        /// 连接到RabbitMQ服务器
        /// </summary>
        protected void Connect()
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
        protected void Close()
        {
            _connection.Dispose();
            _channel.Dispose();
            _connectionFactory = null;
        }

        /// <summary>
        /// 生产者发布消息到队列
        /// </summary>
        /// <param name="args">单个对象，不能是集合</param>
        protected void PublishMessage(object args)
        {
            if (_connectionFactory == null)
            {
                throw new Exception("请先调用Connect方法连接服务器");
            }
            _channel.QueueDeclare(_queueName, true, false, false);
            _channel.QueueBind(_queueName, _exchangeName, RouteKey);

            string message = args.ToJson();
            var body = Encoding.UTF8.GetBytes(message);

            var props = _channel.CreateBasicProperties();
            props.Persistent = true;

            _channel.BasicPublish(_exchangeName, RouteKey, props, body);
        }

        /// <summary>
        /// 消费者从队列消费消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        protected virtual void ConsumeMessage<T>(Func<T,Task> func)
        {
            if (this._connectionFactory == null)
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
    }
}
