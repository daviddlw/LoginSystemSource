using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace David.Commons.Helper
{
	public static class StringHelper {

        /// <summary>
        /// 验证字符串长度
        /// </summary>
        /// <param name="content"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool CheckStrLength(string content, int maxLength)
        {
            bool result = true;
            int tempLength = 0;
            char[] cArray = content.ToCharArray();

            if (cArray.Length > 0)
            {
                foreach (char c in cArray)
                {
                    if (c > 128)
                    {
                        tempLength++;
                    }
                    tempLength++;
                }

                if (tempLength > maxLength)
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 验证字符串长度，ORACLE字段类型为VARCHAR(BYTE)且编码为UTF8时，每个汉字占用3个字节
        /// </summary>
        /// <param name="content"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool CheckStrLength_OracleUtf8(string content, int maxLength)
        {
            bool result = true;
            int tempLength = 0;
            char[] cArray = content.ToCharArray();

            if (cArray.Length > 0)
            {
                foreach (char c in cArray)
                {
                    if (c > 128)
                    {
                        tempLength += 2;
                    }
                    tempLength++;
                }

                if (tempLength > maxLength)
                {
                    result = false;
                }
            }

            return result;
        }

		/// <summary>
		/// 获取指定字节长度的中英文混合字符串
		/// </summary>
		public static string GetSubString(this string str, int len) {
            if (len <= 0) return str;
			string result = string.Empty;// 最终返回的结果
			if (!string.IsNullOrEmpty(str)) {
				int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
				int charLen = str.Length;// 把字符平等对待时的字符串长度
				int byteCount = 0;// 记录读取进度
				int pos = 0;// 记录截取位置
				if (byteLen > len) {
					for (int i = 0; i < charLen; i++) {
						if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
							byteCount += 2;
						else// 按英文字符计算加1
							byteCount += 1;
						if (byteCount > len)// 超出时只记下上一个有效位置
                        {
							pos = i;
							break;
						} else if (byteCount == len)// 记下当前位置
                        {
							pos = i + 1;
							break;
						}
					}

					if (pos >= 0)
						result = str.Substring(0, pos) + "..";
				} else
					result = str;
			}
			return result;
		}

		/// <summary>
		/// yyyyMMdd
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string ToImportImageDateStr(this DateTime date) {
			return date.Year + (date.Month < 10 ? "0" : "") + date.Month + "/" + (date.Day < 10 ? "0" : "") + date.Day;
			//return date.ToString("yyyyMMdd");
		}
		/// <summary>
		/// 获取左侧几位字符
		/// </summary>
		/// <param name="str"></param>
		/// <param name="Length"></param>
		/// <returns></returns>
		public static string GetLeft(this string str, int Length) {
            if (!String.IsNullOrEmpty(str))
            {
				if (Length >= str.Length)
					return str;
				else
					return str.Substring(0, Length);
			} else
				return "";
		}

        /// <summary>
        /// 清除空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveBlank(this string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                return str.Replace("\n", "").Replace("\r", "").Replace(" ", "").Replace("　", "").Trim();
            }
            else
                return "";
        }

		/// <summary>
		/// 获取右侧几位字符
		/// </summary>
		/// <param name="str"></param>
		/// <param name="Length"></param>
		/// <returns></returns>
		public static string GetRight(this string str, int Length) {
			if (!String.IsNullOrEmpty(str)) {
				if (Length >= str.Length)
					return str;
				else
					return str.Substring(str.Length - Length);
			} else
				return "";
		}
		/// <summary>
		/// 转换为md5
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ToMD5(this string str) {
			using (System.Security.Cryptography.MD5CryptoServiceProvider provider = new System.Security.Cryptography.MD5CryptoServiceProvider()) {
				return BitConverter.ToString(provider.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", "").ToLower();
			}
		}
		/// <summary>
		/// 转换为可空的bool类型
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool? ToBoolOrNull(this string str) {
			if (!String.IsNullOrEmpty(str)) {
				if (str.ToUpper() == "TRUE" || str == "1")
					return true;
				else if (str.ToUpper() == "FALSE" || str == "0")
					return false;
				else
					return null;
			} else
				return null;
		}
		public static string ToBit(this bool str) {
			if (str)
				return "1";
			else if (!str)
				return "0";
			return "0";
		}
		public static string ToBitOrEmpty(this bool? str) {
			if (str == null)
				return "";
			if (str == true)
				return "1";
			else if (!str == false)
				return "0";
			return "";
		}
		/// <summary>
		/// 转换为bool类型
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool ToBool(this string str) {
			if (str.ToUpper() == "TRUE" || str == "1")
				return true;
			else
				return false;
		}
		/// <summary>
		/// 转换为正整数
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static int ToNumber(this string str) {
			return int.Parse(str.ToNumeric());
		}
		/// <summary>
		/// 转换为纯数字
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ToNumeric(this string str) {
			if (String.IsNullOrEmpty(str))
				return "0";
			else {
				str = Regex.Replace(str, "\\D", string.Empty);
				if (str == "")
					return "0";
				else
					return str;
			}
		}

		/// <summary>
		/// 转换为整数
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static int ToInteger(this object str) {

			return (int)str.ToDouble();
			#region Obsolete
			//if (str is DBNull)
			//{
			//    return 0;
			//}
			//string s = Convert.ToString(str);
			//if (!String.IsNullOrEmpty(s))
			//{
			//    s = Regex.Replace(s, "[^0-9^-]", string.Empty);
			//    if (s != "")
			//    {
			//        s = s.GetLeft(1) + s.Substring(1).Replace('-', '\0');
			//    }
			//    int result;
			//    int.TryParse(s, out result);
			//    return result;
			//}
			//else
			//    return 0;
			#endregion
		}

        /// <summary>
        /// 将浮点型转换为整数
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ToString(this float? str,string format) {
            return (str.HasValue?str.Value:0).ToString(format);
		}

        /// <summary>
        /// 将双精度转换为整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToString(this double? str, string format)
        {
            return (str.HasValue ? str.Value : 0).ToString(format);
        }
        /// <summary>
        /// 将整数转换为字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToString(this int? str, string format)
        {
            return (str.HasValue ? str.Value : 0).ToString(format);
        }

        /// <summary>
        /// 将长整数转换为字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToString(this long? str, string format)
        {
            return (str.HasValue ? str.Value : 0).ToString(format);
        }
		
		/// <summary>
		/// 转换为long
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static long ToLong(this object str) {
			return (Int64)str.ToDouble();
		}

        /// <summary>
        /// 转换成decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
		public static decimal ToDecimal(this string str) {
			if (!string.IsNullOrEmpty(str)) {
				if (str != "") {
					str = str.GetLeft(1) + str.Substring(1).Replace('-', '\0');
				}

				decimal result;
				decimal.TryParse(str, out result);
				return result;
			}

			return 0;
		}

		/// <summary>
		/// 转换为浮点数
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static float ToFloat(this object str) {
			if (str is float) return (float)str;
			try {
				return float.Parse(str.ToString());
			}catch// (Exception ex) 
            {
				return 0;
			}
		}

		/// <summary>
		/// 转换为浮点数
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static double ToDouble(this object str) {
			if (str is double) return (double)str;
            if (str is Enum) return (int)str;
            
			try {
				return double.Parse(str.ToString());
			} catch //(Exception ex)
            {
				return 0;
			}
		}

		public static DateTime ToDateTime(this string str) {
			if (!string.IsNullOrEmpty(str)) {
				DateTime result;
				DateTime.TryParse(str, out result);
				return result;
			} else {
				return DateTime.MinValue;
			}
		}

		/// <summary>
		///  Description换行显示
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ToDescription(this string str, string type) {
			int singleRowLength = 48; //单行长度
			bool flag = type == "view" ? true : false;
			if (flag) {
				if (str.Length <= singleRowLength) {
					return str = str == string.Empty ? "--" : str;
				} else {
					for (int i = 0; i < str.Length / singleRowLength; i++) {
						bool isInsert = str.Length - str.LastIndexOf(">") - 1 > singleRowLength ? true : false;
						if (isInsert) {
							str = i == 0 ? str.Insert(singleRowLength * (i + 1), "<br/>") : str.Insert(singleRowLength * (i + 1) + 5 * i, "<br/>");
						}
					}
					return str;
				}
			}
			return str;
		}

		/// <summary>
		/// 重复字符串次数
		/// </summary>
		/// <param name="str"></param>
		/// <param name="Length"></param>
		/// <returns></returns>
		public static string GetRepeater(this string str, int Times) {
			if (Times < 1)
				return "";
			StringBuilder sb = new StringBuilder();
			for (int i = 1; i < Times; i++) {
				sb.Append(str);
			}
			return sb.ToString();
		}

		public static string ToHTML(this string str) {
			if (!String.IsNullOrEmpty(str))
				return str.Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace(" ", "&nbsp;").Replace("\n", "<br />");
			else
				return "";
		}

		public static string ToXML(this string str) {
			if (!String.IsNullOrEmpty(str))
				return str.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;").Replace("\'", "&apos;").Replace("\"", "&quot;");
			else
				return "";
		}

		public static string ToText(this string str) {
			if (!String.IsNullOrEmpty(str))
				return str.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&nbsp;", " ").Replace("<br />", "\n");
			else
				return "";
		}

		public static string ToJs(this string str) {
			if (!String.IsNullOrEmpty(str)) {
				str = str.Replace("\"", "\\\"").Replace("'", "\\\'").Replace("/", "\\/").Replace("<\\/script>", "<\\/s\'+\'cript>");
				str = Regex.Replace(str, @"\s+", " ", RegexOptions.Multiline);
				str = "document.write('" + str + "');";
				return str;
			} else
				return "";
		}

		/// <summary>
		/// 格式化空串,返回指定字符串，默认是"--"
		/// </summary>
		/// <param name="inputStr"></param>
		/// <returns></returns>
		public static string FormatEmptyStr(this object inputStr) {
			return FormatEmptyStr(inputStr == null ? "" : inputStr.ToString(), "--");
		}
		/// <summary>
		/// 格式化空串,返回指定字符串outputStr
		/// </summary>
		/// <param name="inputStr"></param>
		/// <param name="outputStr"></param>
		/// <returns></returns>
		public static string FormatEmptyStr(this string inputStr, string outputStr) {
			if (inputStr == null || inputStr.Trim() == string.Empty) {
				inputStr = outputStr;
			}
			return inputStr;
		}

        public static string FormatDoubleByStr(this double doubleValue)
        {
            if (doubleValue == 0)
            {
                return "";
            }
            else
                return doubleValue.ToString("f2");
        }

        public static string FormatIntByStr(this int? intValue)
        {
            if (intValue == 0 || intValue == null)
            {
                return "";
            }
            else
                return intValue.ToString("N0");
        }
        
		public static string LeftSubstring(this string str, int length, string ellipsis) {
			string result = "";//返回值   
			if (str.Length <= length / 2) {
				result = str;
			} else {
				int t = 0;
				char[] tmp = str.ToCharArray();
				for (int i = 0; i < str.Length; i++) {
					int c;
					c = (int)tmp[i];
					if (c < 0)
						c = c + 65536;
					if (c > 255)
						t = t + 2;
					else
						t = t + 1;
					if (t > length)
						break;
					result = result + str.Substring(i, 1);
				}
				result = result + ellipsis;
			}
			return result;

		}

        /// <summary>
        /// 将字符串数组转换成整形数组，如果转换失败的为0
        /// </summary>
        /// <param name="sArray"></param>
        /// <returns></returns>
		public static int[] ToIntergerArray(this string[] sArray) {
			int[] iArray = new int[sArray.Length];
			for (int i = 0; i < sArray.Length; i++) {
				iArray[i] = sArray[i].ToInteger();
			}
			return iArray;
		}
        /// <summary>
        /// 将字符串数组转换成长整形数组，如果转换失败的为0
        /// </summary>
        /// <param name="sArray"></param>
        /// <returns></returns>
		public static long[] ToLongArray(this string[] sArray) {
			long[] lArray = new long[sArray.Length];
			for (int i = 0; i < sArray.Length; i++) {
				lArray[i] = sArray[i].ToLong();
			}
			return lArray;
		}
        /// <summary>
        /// 查找Id列时高亮
        /// </summary>
        /// <param name="content"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string ReplaceKeywordForID(this string content, string keyword)
        {
			if (content == keyword) {
				return string.Format("<span style='color:#ff9900'>{0}</span>", content);
			}

			return content;
		}
        /// <summary>
        /// 替换关键字
        /// </summary>
        /// <param name="content"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string ReplaceKeyword(this string content, string keyword)
        {
            //content = content.ToHTML();

            if (string.IsNullOrEmpty(keyword))
            {
                return content;
            }
            else
            {
                MatchEvaluator evaluator = new MatchEvaluator(GetReplacement);
                return Regex.Replace(content, keyword, evaluator, RegexOptions.IgnoreCase);
            }
        }

        private static string GetReplacement(Match m)
        {
            return string.Format("<span style='color:#ff9900'>{0}</span>", m.Value);
        }
        /// <summary>
        /// 将字符串按照分割符分割，返回分割后的字符串列表
        /// </summary>
        /// <param name="content"></param>
        /// <param name="spiltChar"></param>
        /// <returns></returns>
        public static List<string> ToList(this string content,char spiltChar)
        {
            return content.Split(spiltChar).ToList();
        }

        /// <summary>
        /// 将字符串按照分割符分割，返回分割后的整型串列表
        /// </summary>
        /// <param name="content"></param>
        /// <param name="spiltChar"></param>
        /// <returns></returns>
        public static List<int> ToListInteger(this string content, char spiltChar)
        {
            List<int> result = new List<int>();
            List<string> contentList= content.Split(spiltChar).ToList();
            for (int i = 0; i < contentList.Count; i++)
            {
                result.Add(contentList[i].ToInteger());
            }
            return result;
        }

        /// <summary>
        /// 将目标值转换为标准字符串。
        /// </summary>
        /// <param name="value">目标值。</param>
        /// <returns>字符串。</returns>
        public static string ToStandardString(this DateTime value)
        {
            if (value == DateTime.MinValue) return string.Empty;
            return value.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// 将字符串转换成有tip的格式
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="length"></param>
        /// <returns></returns>
		public static string GetTooltip(this string content, string title,int length=0) {
			if (string.IsNullOrEmpty(content)) {
                return content.FormatEmptyStr();
			} else {
                return string.Format("<span title=\"{0}\">{1}</span>", title, length>0?content.GetSubString(length):content);
			}
		}

        /// <summary>
        /// 将FormatEmptyStr,GetSubstring和Tooltip,ReplaceKeyword合并后的方法
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string ToTooltipString(this string content,int length = 0,string keyword="",string toolTip="")
        {
            if (string.IsNullOrEmpty(content))
            {
                return content.FormatEmptyStr();
            }
            else
            {
                return string.Format("<span title=\"{0}\">{1}</span>", toolTip!=""?toolTip:content, content.GetSubString(length).ReplaceKeyword(keyword));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string JoinToString<TSource>(this IEnumerable<TSource> source)
        {
            return source.JoinToString<TSource>(",", i => i.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string JoinToString<TSource>(this IEnumerable<TSource> source, string separator)
        {
            return source.JoinToString<TSource>(separator, i => i.ToString());
        }
        /// <summary>
        /// join IEnumerable to string with ",".
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector">choose the string to be joined</param>
        /// <returns></returns>
        /// <example>sampleList.JoinToString(c => c.ToString())</example>
        public static string JoinToString<TSource>(this IEnumerable<TSource> source, Func<TSource, string> selector)
        {
            return source.JoinToString<TSource>(",", selector);
        }
        /// <summary>
        /// join IEnumerable to string with seperator.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <param name="selector">choose the string to be joined</param>
        /// <returns></returns>
        /// <example>sampleList.JoinToString(";", c => c.ToString())</example>
        public static string JoinToString<TSource>(this IEnumerable<TSource> source, string separator, Func<TSource, string> selector)
        {
            return string.Join(separator, source.Select(selector).ToArray<string>());
        }
        
        /// <summary>
        /// 格式化数字型字符串(p型，n0型)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToFormatString(this string str, string type)
        {
            string result = string.Empty;
            Regex regx = new Regex(@"^\d+(.\d*)?$");
            if (regx.IsMatch(str))
            {
                double currentObj = Convert.ToDouble(str);
                if (type.ToLower() == "p")
                {
                    result = currentObj.ToString("P");
                }
                else if(type.ToLower() == "n0")
                {
                    result = currentObj.ToString("N0");
                }
                else if (type.ToLower() == "n2")
                {
                    result = currentObj.ToString("N2");
                }
                else if (type.ToLower() == "c2")
                {
                    result = currentObj.ToString("C2");
                }
            }
            else
            {
                result = str;
            }
            return result;
        }

		/// <summary>
		/// 首字母小写
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToLowerOnFirstChar(this string value) {
			if (string.IsNullOrEmpty(value)) { 
				return "";
			}
			if(value.Length ==1){
				return value.ToLower();
			}
			return value.Substring(0,1).ToLower() + value.Substring(1);
		}

		/// <summary>
		/// 首字母大写
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToUpperOnFirstChar(this string value) {
			if (string.IsNullOrEmpty(value)) { 
				return "";
			}
			if(value.Length ==1){
				return value.ToUpper();
			}
			return value.Substring(0,1).ToUpper() + value.Substring(1);
		}

		/// <summary>
		/// 各种单词首字母大写
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToUpperOnWordFirstChar(this string value) {
			return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value);
		}

        /// <summary>
        /// 将目标值转换为数组，以半角逗号作为分隔符。
        /// </summary>
        /// <param name="value">目标值。</param>
        /// <returns>数组。</returns>
        public static string[] ToSplitArray(this string value)
        {
            if (string.IsNullOrEmpty(value)) return new string[] { };
            string[] items = null;
            if (!value.Contains(",") && !value.Contains("，"))
                items = new string[] { value };
            else if (!value.Contains('，'))
                items = value.Split(',');
            else
                items = value.Split('，');
            return items;
        }

        /// <summary>
        /// 将目标字符串转换为整型数组，以半角逗号作为分隔符
        /// </summary>
        /// <param name="value">待转换的目标字符串</param>
        /// <returns>返回一个整型数组</returns>
        public static int[] ToSplitIntArray(this string value)
        {
            if (string.IsNullOrEmpty(value)) return new int[] { };
            string[] items = null;
            if (!value.Contains('，'))
                items = value.Split(',');
            else
                items = value.Split('，');
            return items.ToIntergerArray();
        }

        /// <summary>
        /// 获取?类型的默认值，整型为0
        /// </summary>
        /// <param name="currentObject"></param>
        /// <returns></returns>
        public static Int32 GetDefaultValue(this Nullable<System.Int32> currentObject)
        {
            return currentObject.HasValue ? currentObject.Value : 0;
        }

        public static Int64 GetDefaultValue(this Nullable<System.Int64> currentObject)
        {
            return currentObject.HasValue?currentObject.Value:0;
        }

        public static Decimal GetDefaultValue(this Nullable<Decimal> currentObject)
        {
            return currentObject.HasValue ? currentObject.Value : 0;
        }

        public static double GetDefaultValue(this Nullable<double> currentObject)
        {
            return currentObject.HasValue ? currentObject.Value : 0;
        }
        /// <summary>
        /// 获取?类型的默认值，布尔型为false
        /// </summary>
        /// <param name="currentObject"></param>
        /// <returns></returns>
        public static bool GetDefaultValue(this Nullable<bool> currentObject)
        {
            return currentObject.HasValue ? currentObject.Value : false;
        }
        /// <summary>
        /// 获取?类型的默认值，时间类型为当前时间
        /// </summary>
        /// <param name="currentObject"></param>
        /// <returns></returns>
        public static DateTime GetDefaultValue(this Nullable<DateTime> currentObject)
        {
            return currentObject.HasValue ? currentObject.Value : DateTime.Now;
        }

		/// <summary>
		/// 有效的字符串
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ToEnableString(this object str) {
			if (str == null) {
				return string.Empty;
			} else {
				return str.ToString();
			}
		}
	}
}
