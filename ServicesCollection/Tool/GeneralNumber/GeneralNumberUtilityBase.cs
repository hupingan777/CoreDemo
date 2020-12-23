using ServicesCollection.Tool.DanLi;
using System;

namespace ServicesCollection.Tool.GeneralNumber
{
    //绝对单例，避免重复
    /// <summary>
    /// 生成唯一值
    /// </summary>
    public class GeneralNumberUtilityBase : Singleton<GeneralNumberUtilityKernel>
    {
        private GeneralNumberUtilityBase() { }
    }

    //核心部分
    /// <summary>
    /// 生成唯一值
    /// </summary>
    public class GeneralNumberUtilityKernel
    {
        private GeneralNumberUtilityKernel() { }

        #region 生成主键Long

        /// <summary>
        /// 获取主键
        /// </summary>
        /// <returns></returns>
        public long GetPrimaryKey()
        {
            var items = AutoIncrement_PrimaryKey_Singleton.Instance.CreateCode(4);
            var code = items[0];//如果需要转化为long，最高这里只能设为4
            return long.Parse(string.Format("{0}{1}", items[1], code));
        }

        #region 产码器

        /// <summary>
        /// 设备码
        /// </summary>
        private class AutoIncrement_Terminal
        {
            /// <summary>
            /// 产码终端总数量
            /// </summary>
            private int _AllCount { get; set; } = 0;//99

            /// <summary>
            /// 当前产码终端编号（0-产码终端总数量）
            /// </summary>
            private int _ThisNum { get; set; } = 0;//5

            /// <summary>
            /// 产码终端总数量
            /// </summary>
            /// <returns></returns>
            private int _GetAllCount()
            {
                var num = _AllCount;
                if (num > 0)
                {
                    return num;
                }

                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["TerminalsCount"]?.ToString(), out num);
                if (num <= 0)
                {
                    throw new ArgumentOutOfRangeException("TerminalsCount", "终端总数量设置错误(AppSettings:TerminalsCount)");
                }
                _AllCount = num;
                return num;
            }

            /// <summary>
            /// 当前产码终端编号（0-产码终端总数量）
            /// </summary>
            /// <returns></returns>
            private int _GetThisNum()
            {
                var num = _ThisNum;
                if (num > 0)
                {
                    return num;
                }

                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["TerminalsNumber"]?.ToString(), out num);
                if (num <= 0)
                {
                    throw new ArgumentOutOfRangeException("TerminalsNumber", "当前终端编号设置错误(AppSettings:TerminalsNumber)");
                }
                _ThisNum = num;
                return num;
            }

            /// <summary>
            /// 获取当前产码终端编号
            /// </summary>
            /// <param name="codeLen"></param>
            /// <returns></returns>
            protected (int ThisNum, string ThisNumFormatStr, string AllCountFormatStr, string AllCountMaxValue, int AllCount) GetTerminalNo(int codeLen)
            {
                var ac = _GetAllCount();
                var acl = ac.ToString().Length;
                if (acl >= codeLen)
                {
                    throw new Exception("终端数量超限，请扩容码长度");
                }

                var alm = codeLen - acl;
                string s1 = string.Empty, s2 = string.Empty, s3 = string.Empty;
                for (int i = 0; i < acl; i++)
                {
                    s1 += "0";
                }
                for (int i = 0; i < alm; i++)
                {
                    s2 += "0";
                    s3 += "9";
                }

                var tn = _GetThisNum();
                if (tn > ac)
                {
                    throw new Exception("终端编号超限，请修改终端数量设置或者设置正确的终端编号");
                }

                return (tn, s1, s2, s3, ac);
            }
        }

        /// <summary>
        /// 自增类(产码器)
        /// </summary>
        private class AutoIncrement_PrimaryKey : AutoIncrement_Terminal
        {
            /// <summary>
            /// 当前值
            /// </summary>
            private int _Value = 0;

            /// <summary>
            /// 当前存时
            /// </summary>
            private DateTime _Time = DateTime.Now;

            /// <summary>
            /// 锁
            /// </summary>
            private static readonly object _lockObj = new object();

            /// <summary>
            /// 产码（默认为秒级重置）
            /// </summary>
            /// <param name="codeLen">产码长度</param>
            /// <returns></returns>
            public string[] CreateCode(int codeLen = 4)
            {
                lock (_lockObj)
                {
                    if (codeLen <= 0)
                        return new string[] { string.Empty, DateTime.Now.ToString("yyMMddHHmmss") };

                    var tNo = GetTerminalNo(codeLen);

                    if (int.Parse(tNo.AllCountMaxValue) <= _Value)
                    {
                        _Value = 1;
                        _Time = _Time.AddSeconds(1);
                    }
                    else
                    {
                        _Value += 1;
                    }

                    return new string[] {
                        tNo.ThisNum.ToString(tNo.ThisNumFormatStr) + _Value.ToString(tNo.AllCountFormatStr),//终端编号+递增码
                        _Time.ToString("yyMMddHHmmss")//时间yyMMddHHmmss
                    };
                }
            }
        }

        /// <summary>
        /// 自增类(产码器)
        /// </summary>
        private class AutoIncrement_PrimaryKey_Singleton : Singleton<AutoIncrement_PrimaryKey>
        {
            private AutoIncrement_PrimaryKey_Singleton() { }
        }

        #endregion

        #endregion

        #region 生成订单号

        /// <summary>
        /// 生成单号
        /// </summary>
        /// <param name="key">类型（RK（入库单号） CK（出库单号） PD（盘点单号） TH（退货单号） CGTD（采购退单） CGDD（采购订单） XSTD（销售退单） XSDD（销售订单）  ）</param>
        /// <param name="timeFormat">时间格式</param>
        /// <returns></returns>
        public string GenerateOrderNo(string key, string timeFormat = "yyMMddHHmmssfff")
        {
            var items = AutoIncrement_Code_Singleton.Instance.CreateCode(key, timeFormat);
            return items;
        }

        #region 产码器

        /// <summary>
        /// 自增类(产码器)
        /// </summary>
        private class AutoIncrement_Code : AutoIncrement_Terminal
        {
            /// <summary>
            /// 当前值
            /// </summary>
            private int _Value = 0;

            /// <summary>
            /// 当前存时
            /// </summary>
            private DateTime _Time = DateTime.Now;

            /// <summary>
            /// 锁
            /// </summary>
            private static readonly object _lockObj = new object();

            /// <summary>
            /// 产码（默认为分钟数级重置）
            /// </summary>
            /// <param name="key">分类标识</param>
            /// <param name="timeFormat">时间格式</param>
            /// <returns></returns>
            public string CreateCode(string key, string timeFormat = "yyMMddHHmmssfff")
            {
                lock (_lockObj)
                {
                    int codeLen = 4;
                    var tNo = GetTerminalNo(codeLen);
                    //当前值自增上限
                    var n4 = int.Parse(tNo.AllCountMaxValue);

                    if (_Time <= DateTime.Now)
                    {
                        _Time = DateTime.Now;
                    }

                    if (n4 <= _Value)
                    {
                        _Value = 1;
                        _Time = _Time.AddSeconds(1);
                    }
                    else
                    {
                        _Value += 1;
                    }

                    return $"{key}{_Time.ToString(timeFormat)}{tNo.ThisNum.ToString(tNo.ThisNumFormatStr)}{_Value.ToString(tNo.AllCountFormatStr)}";
                }
            }
        }

        /// <summary>
        /// 自增类(产码器)
        /// </summary>
        private class AutoIncrement_Code_Singleton : Singleton<AutoIncrement_Code>
        {
            private AutoIncrement_Code_Singleton() { }
        }

        #endregion

        #endregion
    }
}
