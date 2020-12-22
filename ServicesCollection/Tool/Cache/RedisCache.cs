namespace ServicesCollection.Tool.Cache
{
    public class RedisCache : ICache
    {
        public object Get()
        {
            return "这是redis的get方法";
        }
    }
}
