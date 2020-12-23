namespace ServicesCollection.Tool.GeneralNumber
{
    /// <summary>
    /// 生成数据
    /// </summary>
    public class GeneralNumberUtility : IGeneralNumberUtility
    {
        /// <summary>
        /// 获取主键
        /// </summary>
        /// <returns></returns>
        public long GetPrimaryKey()
        {
            return GeneralNumberUtilityBase.Instance.GetPrimaryKey();
        }
        
    }

}


