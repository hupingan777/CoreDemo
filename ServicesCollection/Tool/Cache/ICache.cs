using ServicesCollection.Tool.AutoFac;

namespace ServicesCollection.Tool.Cache
{
    public interface ICache : IDependency
    {
        object Get();
    }
}
