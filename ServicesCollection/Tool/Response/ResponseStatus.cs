using System.Collections.Generic;
using System.Linq;

namespace ServicesCollection.Tool.Response
{
    /// <summary>
    /// 相应数据
    /// </summary>
    public class ResponseStatus: IResponseStatus
    {
        private readonly IList<string> _messageList = new List<string>();
        
        public virtual bool Success => !_messageList.Any();

        public virtual int Code { get; set; } = 200;

        public virtual string SuccessMessage { get; set; }

        public virtual IList<string> MessageList => _messageList;

        public virtual void AddMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
                _messageList.Add(message);
        }

        public virtual void AddMessage(IEnumerable<string> messages)
        {
            if (messages != null && messages.Any())
                foreach (var message in messages)
                {
                    if (!string.IsNullOrWhiteSpace(message))
                        _messageList.Add(message);
                }
        }
    }

    public class ResponseStatus<T> : ResponseStatus, IResponseStatus<T>
    {
        public ResponseStatus()
        {
        }

        public ResponseStatus(IResponseStatus status)
        {
            if (status != null)
            {
                this.Code = status.Code;
                this.SuccessMessage = status.SuccessMessage;
                if (status.MessageList != null)
                    this.AddMessage(status.MessageList);
            }
        }

        public virtual T Data { get; set; }
    }

    public class ResponseBankStatus<T>: ResponseStatus<T>
    {
        public new string MessageList { get; set; }

        public new void AddMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
                MessageList = (string.IsNullOrWhiteSpace(MessageList) ? string.Empty : MessageList+",") + message;
        }

        public new void AddMessage(IEnumerable<string> messages)
        {
            if (messages != null && messages.Any())
                foreach (var message in messages)
                {
                    if (!string.IsNullOrWhiteSpace(message))
                        MessageList = (string.IsNullOrWhiteSpace(MessageList) ? string.Empty : MessageList + ",") + message;
                }
        }
    }
}
