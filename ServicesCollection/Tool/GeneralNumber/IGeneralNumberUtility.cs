using ServicesCollection.Tool.AutoFac;

namespace ServicesCollection.Tool.GeneralNumber
{
    /// <summary>
    /// 生成数据
    /// </summary>
    public interface IGeneralNumberUtility : IDependency
    {
        /// <summary>
        /// 生成固定规则的主键
        /// </summary>
        /// <returns></returns>
        long GetPrimaryKey();

    }
}
