using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RegularExpressionConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                string pattern = @"^1[35789][0-9]{9}$";
                string inputValue = "19922933747";
                var result = GetMatchValue(pattern, inputValue);
            }
            {
                string pattern = @"(?<={)[a-z]+[a-z0-9]*(?=})";//(?<=[{｛])[^{}｛｝]*(?=[｝}])
                string inputValue = "亲！${branch}于${time}已退款${money}元，退换货单号：${return1code}，{ttest}{123456}{{777}}快去查sdfsf看吧！";
                var result = GetMatchValues(pattern, inputValue);
            }

            {
                var pattern = @"<.*?>"; //.*会用贪婪的方式匹配，即尽可能多的匹配字符串（会取inputValue的第一个'<'和最后一个'>'之间的字符，包括<>符号）
                string inputValue = "<h1>rundoo</h1><h3></h3><>";
                var result = GetMatchValueStr(pattern, inputValue);
            }

            {
                //?:表示0个或1个字符   *:表示0个或者多个字符
                var regex = new Regex(@"[0-9]+");
                string str = "abc123abc456abc";
                var match = regex.Matches(str);
            }
            {
                string inputValue = "13abc";
                string pattern = @"^[0-9]+abc$";//false
                var result = GetMatchValue(pattern, inputValue);

                //^表示为匹配输入字符串的开始位置
                //[0-9]+表示1个或者多个数字，+表示1个或者多个（至少一个）
                //abc$匹配字母abc并以abc结尾，$为匹配输入字符串的结束位置
            }

            {
                string pattern = @"^[a-z0-9_-]{3,15}$";
                string inputValue = "_hupinganpp";
                var result = GetMatchValue(pattern, inputValue);

                //表示字符串中只能包含a-z的字母、0-9的数字、下划线（_）、横线（-）
            }
            {
                string pattern = @"runoo?[a-z]{1,5}";
                string inputValue = "zzzzpprunoo";
                var result = GetMatchValue(pattern, inputValue);
            }
            {
                //   \s表示空白字符   \S表示非空白字符
                string pattern = @"[\s]";
                string inputValue = "Google Runoob Taobao";
                var result = GetMatchValues(pattern, inputValue, RegexOptions.None);
                var replaceStr = Regex.Replace(inputValue, pattern, "");
            }

            {
                //   \w表示大写字母、小写字母、数字、下划线   等价[a-zA-Z0-9_]
                //   \W表示除了大写字母、小写字母、数字、下划线以外的字符   等价[^a-zA-Z0-9_]
                string pattern = @"[^a-zA-Z0-9_]";
                string inputValue = "abc_@@@@_123_%%%%9";
                var result = GetMatchValueStr(pattern, inputValue, RegexOptions.None);
            }
            {
                //对特殊字符的匹配要进行转义，例如：*转\*   [转\[   ?转\?
                string pattern = @"^runo\*b";
                string inputValue = "runo*bhrhdf";
                var result = GetMatchValue(pattern, inputValue, RegexOptions.None);
            }
            {
                //元组的初步使用，括号中要匹配多个项时用“|”分开，例如([a-z]+|[\d]+)
                string pattern = "([a-zA-z0-9]{3})([a-zA-Z0-9]*)([a-zA-Z0-9]{3})";
                string inputValue = "CJ852365254586977";
                var resultTuple = Regex.Replace(inputValue, pattern, "$1****$3");

            }

            {
                //通过在*、+或？限定符之后放置?，该表达式会从“贪婪”表达式转换为“非贪婪”表达式或者最小匹配。
                string pattern = @"<.+?>";//.*   .*?   .表示除开\n的所有字符

                string inputValue = "<h1>RUNOOB-菜鸟教程</h1><p></p><hhhhhhh>123</hhhhhhh><><>";
                var result = GetMatchValueStr(pattern, inputValue, RegexOptions.None);
            }

            {
                // \b匹配单词的边界，如果放在匹配字符的前面就是以这个字符开始，如果放在匹配字符的后面就是以这个字符结尾，每个单词之间要有空格
                string pattern = @"er\b";//\bChapter
                string inputValue = "Chapitaler Chapter er er er";
                var result = GetMatchValues(pattern, inputValue);
            }

            {
                //选择（元组），子表达式用()括起来
                string pattern = @"[0-9][a-z]+";//([0-9])([a-z]+)
                string inputValue = "123456runoob123runoob456";
                var result = GetMatchValues(pattern, inputValue);
            }

            {
                //在元组中“表达式A（?=表达式B）”，?=表示获取表达式B前面的表达式A（表达式B与表达式A中间不能有其他表达式，必须是相邻）
                string pattern = @"\d+(?=[}]+)";
                string inputValue = "runoob{123}runoob{efgggggg777}";
                var result = GetMatchValues(pattern, inputValue);

                //在元组中“(?<=表达式B)表达式A”，?<=表示获取表达式B后面的表达式A（表达式B与表达式A中间不能有其他表达式，必须是相邻）
                string pattern1 = @"(?<=\d{3})[a-zA-Z]+";
                string inputValue1 = "123abc@@@777FF17FFF";
                var result1 = GetMatchValues(pattern1, inputValue1);

                //在元组中“表达式A(?!表达式B)”，?!表示查找后面不是表达式B的表达式A
                string pattern2 = @"[a-zA-Z]+(?!\d+)";//获取字母后面不是数字的字母
                string inputValue2 = "runoob123abcd_ppphhh@";
                var result2 = GetMatchValues(pattern2, inputValue2);

                //在元组中“(?<!表达式B)表达式A”，?<!表示查找前面不是表达式B的表达式A
                string pattern3 = @"(?<![0-9_]+)runoob";
                string inputValue3 = "123runoob_runoob_google-runoob";
                var result3 = GetMatchValues(pattern3, inputValue3);
            }

            {
                //string str = "   哥哥: GG   妹妹:  MM   ";
                string str = "   哥哥:123   妹妹:456   ";
                //string pattern = @"(哥哥|妹妹)+:\s*(GG|MM)+";
                //(?<=[:]{1})[\s]*.+(?=[\s]{1})?
                string pattern = @"(?<=[:]{1})[\s]*.+(?=[\s]{1})?";
                var result3 = GetMatchValues(pattern, str);
            }

            {
                var result = GetZYKillValue(62.2);

                foreach (var item in result)
                {
                    Console.WriteLine($"索引：{item.Index}，值：{item.KillValue.ToString("F2")}%");
                }
                Console.WriteLine($"总值：{result.Sum(x => x.KillValue).ToString("F2")}%");
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 获得正则表达式的所有匹配项
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        /// <param name="inputValue">源字符串</param>
        /// <param name="regexOptions">匹配规则,默认不区分大小写</param>
        /// <returns></returns>
        public static List<string> GetMatchValues(string pattern, string inputValue, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            var result = new List<string>();
            var regex = new Regex(pattern, regexOptions);
            MatchCollection match = regex.Matches(inputValue);
            foreach (Match item in match)
            {
                result.Add(item.Value);
            }
            return result;
        }

        /// <summary>
        /// 获得与正则表达式匹配的字符串
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="inputValue"></param>
        /// <param name="regexOptions"></param>
        /// <returns></returns>
        public static string GetMatchValueStr(string pattern, string inputValue, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            var result = "";
            var regex = new Regex(pattern, regexOptions);
            MatchCollection match = regex.Matches(inputValue);
            foreach (Match item in match)
            {
                result += item;
            }
            return result.Trim();
        }
        /// <summary>
        /// 是否与正则表达式匹配
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        /// <param name="inputValue">源字符串</param>
        /// <param name="regexOptions">匹配规则,默认不区分大小写</param>
        /// <returns></returns>
        public static bool GetMatchValue(string pattern, string inputValue, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            var regex = new Regex(pattern, regexOptions);
            var match = regex.Match(inputValue);
            Console.WriteLine($"pattern:{pattern}\tinputValue:{inputValue}\tresult:{match.Success}");
            return match.Success;
        }

        public static List<ZY> GetZYKillValue(double baseValue, int count = 1)
        {
            var result = new List<ZY>();
            var killValue = baseValue + baseValue * 0.07;
            result.Add(new ZY() { Index = count, KillValue = killValue });
            count++;
            if (count <= 6)
            {
                result.AddRange(GetZYKillValue(result[result.Count - 1].KillValue, count));
                return result;
            }
            else
            {
                return result;
            }
        }
    }

    public class ZY
    {
        /// <summary>
        /// 从1开始，小于等于6
        /// </summary>
        public int Index { get; set; }

        public double KillValue { get; set; }
    }
}
