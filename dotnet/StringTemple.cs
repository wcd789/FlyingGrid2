using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;

namespace Fg2
{
    /*---------------------------
    名称：flyingGrid2
    作者：weichengdong
    邮件: weichengdong2008@foxmail.com
    ---------------------------*/
    public class StringTemple
    {
        private string _template;
        private string _delimiterStart;
        private string _delimiterStop;

        public StringTemple(string stringTemple, string startDelimiter = "{", string stopDelimiter = "}")
        {
            this._template = stringTemple;
            this._delimiterStart = startDelimiter;
            this._delimiterStop = stopDelimiter;
        }

        private StringBuilder GetPropertyList(object obj)
        {
            StringBuilder stringTemple = new StringBuilder(_template);
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in properties)
            {
                object o = property.GetValue(obj, null);
                string strvalue = (o == null ? "" : o.ToString());
                stringTemple.Replace(_delimiterStart + property.Name + _delimiterStop, HttpUtility.HtmlEncode(strvalue));
            }
            return stringTemple;
        }
        /// <summary>
        /// 针对listdata的渲染
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public string Render<T>(List<T> dataList)
        {
            StringBuilder strTableTrs = new StringBuilder();
            foreach (T item in dataList)
            {
                strTableTrs.Append(GetPropertyList(item));
            }
            return strTableTrs.ToString();
        }

        /// <summary>
        /// 针对普通类的渲染
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Render<T>(T data)
        {
           return GetPropertyList(data).ToString();
        }
    }
}
