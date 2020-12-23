using System.Collections.Generic;

namespace ServicesCollection.Tool.Response
{
    public interface IResponseStatus
    {
        bool Success { get; }
        int Code { get; set; }
        string SuccessMessage { get; set; }
        IList<string> MessageList { get; }

        void AddMessage(string message);

        void AddMessage(IEnumerable<string> messages);
    }

    public interface IResponseStatus<T>: IResponseStatus
    {
        T Data { get; set; }
    }
}
