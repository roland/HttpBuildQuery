using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Web;

namespace HttpBuildQuery
{
    public class HttpBuildQueryHelper
    {

        public static string Format(object value, string prefix = "q")
        {
            if (value == null) return String.Format("{0}=", prefix);

            var parts = new List<string>();
            HandleItem(value, parts, prefix);
            return String.Join("&", parts);
        }

        public static string FormatValue(object value, string prefix = "q")
        {
            string strValue = HttpUtility.UrlEncode(value.ToString());
            if (value is bool)
            {
                strValue = (bool)value ? "1" : "0";
            }
            return String.IsNullOrEmpty(strValue) ? String.Empty : String.Format("{0}={1}", prefix, strValue);
        }

        public static string FormatList(IList obj, string prefix = "q")
        {
            int count = obj.Count;
            var parts = new List<string>();

            for (int i = 0; i < count; i++)
            {
                string newPrefix = String.Format("{0}[{1}]", prefix, i);
                HandleItem(obj[i], parts, newPrefix);
            }
            return String.Join("&", parts);
        }

        public static string FormatDictionary(IDictionary<string, object> obj, string prefix = "")
        {
            var parts = new List<string>();
            foreach (var entry in obj)
            {
                string newPrefix = string.IsNullOrEmpty(prefix) ?
                    String.Format("{0}{1}", prefix, entry.Key) :
                    String.Format("{0}[{1}]", prefix, entry.Key);
                HandleItem(entry.Value, parts, newPrefix);
            }
            return String.Join("&", parts);
        }

        private static void HandleItem(object value, List<string> parts, string prefix)
        {
            if (value == null) return;

            if (IsStringable(value))
            {
                parts.Add(FormatValue(value, prefix));
            }
            else if (value is IList)
            {
                parts.Add(FormatList((IList)value, prefix));
            }
            else if (value is IDictionary<string, object>)
            {
                parts.Add(FormatDictionary((IDictionary<string, object>)value, prefix));
            }
            else
            {
                parts.Add(FormatValue(value, prefix));
            }
        }

        public static object Convert(object obj, int depthLimit = 0)
        {
            if (depthLimit > 5) return obj; // prevent recursion from not ending
            if (obj == null) return obj;
            Type type = obj.GetType();
            if (IsStringable(obj) ||
                IsStringableArray(obj) ||
                type.GetProperties().Any(p => p.GetIndexParameters().Length > 0)) return obj;

            if (type.IsArray)
            {
                var parts = new List<object>();
                foreach (object e in (IList)obj)
                {
                    parts.Add(Convert(e));
                }
                return parts.ToArray();
            }

            var dict = new Dictionary<string, object>();
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                dict.Add(prop.Name, Convert(prop.GetValue(obj, null), depthLimit + 1));
            }
            return dict;
        }

        private static bool IsStringable(object o)
        {
            return (o is bool) || (o is byte) || (o is char) || (o is decimal) ||
                    (o is double) || (o is float) || (o is int) || (o is long) ||
                    (o is sbyte) || (o is short) || (o is uint) || (o is ulong) ||
                    (o is ushort) || (o is string);
        }

        private static bool IsStringableArray(object o)
        {
            return (o is bool[]) || (o is byte[]) || (o is char[]) || (o is decimal[]) ||
                    (o is double[]) || (o is float[]) || (o is int[]) || (o is long[]) ||
                    (o is sbyte[]) || (o is short[]) || (o is uint[]) || (o is ulong[]) ||
                    (o is ushort[]) || (o is string[]);
        }
    }
}
