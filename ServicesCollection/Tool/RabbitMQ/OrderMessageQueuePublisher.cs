using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Linq.Dynamic.Core;

namespace ServicesCollection.Tool.RabbitMQ
{
    /// <summary>
    /// 消息发布
    /// </summary>
    public class OrderMessageQueuePublisher : OrderMessageQueueClient, IOrderMessageQueuePublisher
    {
        /// <summary>
        /// 生成数据
        /// </summary>
        private readonly IGeneralNumberUtility _generalNumberUtility;
        private readonly IRepository<AppMsgPushMain, long> _appMsgPushMainRepository;
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="generalNumberUtility"></param>
        /// <param name="appMsgPushMainRepository"></param>
        /// <param name="logger"></param>
        public OrderMessageQueuePublisher(IGeneralNumberUtility generalNumberUtility, IRepository<AppMsgPushMain, long> appMsgPushMainRepository, ILogger logger)
        {
            _generalNumberUtility = generalNumberUtility;
            _appMsgPushMainRepository = appMsgPushMainRepository;
            _logger = logger;
        }

        /// <summary>
        /// 将需要推送的消息加入队列（单条）
        /// </summary>
        /// <param name="model"></param>
        public void PublishOrderMessageToMQ(AppMsgPushMain model)
        {
            if (model.Id == 0)
            {
                model.Id = _generalNumberUtility.GetPrimaryKey();
            }
            model.PushStatus = 100;//默认设置为未读
            model.CreateTime = DateTime.Now;
            model.PushPlatform = 0;
            try
            {
                using (var tran = new TransactionScope())
                {
                    var _dbContext = IocManager.Instance.Resolve<ZvqbDbContext>();
                    //写入数据库
                    _dbContext.AppMsgPushMain.Add(model);
                    _dbContext.SaveChanges();
                    tran.Complete();
                }

                PublishMessageToMQ(new PublishMessageToMQ(){ AppMsgPushId = model.Id });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex);
            }
        }

        /// <summary>
        /// 将需要推送的消息加入队列（多条），临时
        /// </summary>
        /// <param name="lst"></param>
        public void PublishOrderMessageToMQ(List<AppMsgPushMain> lst)
        {
            try
            {
                //var _dbContext = IocManager.Instance.Resolve<ZvqbDbContext>();
                //foreach (var model in lst)
                //{
                //    if (model.Id == 0)
                //    {
                //        model.Id = _generalNumberUtility.GetPrimaryKey();
                //    }
                //    model.PushStatus = 100;//默认设置为未读
                //    model.PushPlatform = 0;
                //    model.CreateTime = DateTime.Now;
                //    _dbContext.AppMsgPushMain.Add(model);

                //}
                //int intCount = _dbContext.SaveChanges();
                //_dbContext.Dispose();
                //if (intCount > 0)
                //{
                //    PublishMultipleMessageToMQ(lst.Where(x => x.Id != 0).Select(x => new PublishMessageToMQ() { AppMsgPushId = x.Id }).ToList());
                //}
                foreach (var model in lst)
                {
                    model.Id = _generalNumberUtility.GetPrimaryKey();
                    model.PushStatus = 100;//默认设置为未读
                    model.PushPlatform = 0;
                    model.CreateTime = DateTime.Now;
                    _appMsgPushMainRepository.Insert(model);
                }
                PublishMultipleMessageToMQ(lst.Where(x => x.Id != 0).Select(x => new PublishMessageToMQ() { AppMsgPushId = x.Id }).ToList());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex);
            }
        }

        private void PublishMessageToMQ(object org)
        {
            this.Connect();
            try
            {
                this.PublishMessage(org);
            }
            finally
            {
                this.Close();
            }
        }

        private void PublishMultipleMessageToMQ(List<PublishMessageToMQ> org)
        {
            this.Connect();
            try
            {
                foreach (var item in org)
                {
                    this.PublishMessage(item);
                }
            }
            finally
            {
                this.Close();
            }
        }
    }


}
