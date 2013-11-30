using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Fg2
{
    /*---------------------------
     名称：flyingGrid2
     作者：weichengdong
     邮件: weichengdong2008@foxmail.com
     ---------------------------*/
    public class FlyingGrid2
    {
        private Fg2Cfg girdCfg = new Fg2Cfg();

        /// <summary>
        /// 分页每页大小
        /// </summary>
        public int PageSize { get { return girdCfg.PageSize; } }
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get { return girdCfg.TableName; } }

        /// <summary>
        /// 表格初始化
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <param name="gridFullConfigFileName">xml配置文件名称</param>
        public FlyingGrid2(string tableName, String gridFullConfigFileName)
        {
            girdCfg.TableName = tableName;
            string fullXmlConfigPath = gridFullConfigFileName;
            this.LoadxmlConfig(girdCfg.TableName, fullXmlConfigPath);
        }

        //------------------------------------------------------------

        /// <summary>
        /// 执行表格化
        /// </summary>
        /// <param name="dataList">list data</param>
        /// <param name="pageNumber">DataCount|PageSize|PageNumber</param>
        ///  <param name="dataCount">数据条数</param>
        /// <returns></returns>
        public string LoadGrid<T>(List<T> dataList, int pageNumber, int dataCount = 0)
        {
            try
            {
                if (dataCount <= 0) { dataCount = dataList.Count; }
                if (pageNumber <= 0) { pageNumber = 1; }
                int pageSize = girdCfg.PageSize;
                if (pageSize <= 0) { pageSize = dataList.Count; }
                return GetGrid(dataList, pageNumber, dataCount, pageSize);
            }
            catch
            {
                throw new ApplicationException("数据表格加载错误");
            }
        }

        //------------------------------------------------------------

        /// <summary>
        /// 执行表格化
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public string LoadGrid<T>(List<T> dataList)
        {
            int dataCount = dataList.Count;
            int pageNumber = 1;
            int pageSize = dataList.Count;
            return GetGrid(dataList, pageNumber, dataCount, pageSize);
        }

        //------------------------------------------------------------

        private string GetGrid<T>(List<T> dataList, int pageNumber, int dataCount, int pageSize)
        {
            this.ApendContentData(dataList);
            this.AppendTail(dataCount, pageNumber, pageSize);
            return girdCfg.TableMode.ToString();
        }

        /// <summary>
        /// 加载配置模板
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="xmlfilepath"></param>
        private void LoadxmlConfig(string tableName, string xmlfilepath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlfilepath);
            XmlNodeList tables = xmlDoc.SelectNodes("flyingGrid2/table");

            foreach (XmlNode itemTable in tables)
            {
                if (itemTable.Attributes["id"].Value == tableName)
                {
                    XmlAttribute usePager = itemTable.Attributes["usePager"];
                    girdCfg.Uspager = usePager.Value;

                    XmlAttribute pageSize = itemTable.Attributes["pageSize"];
                    girdCfg.PageSize = int.Parse(pageSize.Value);

                    XmlNodeList trs = itemTable.SelectNodes("tr");
                    foreach (XmlNode itemTr in trs)
                    {
                        XmlAttribute trTemplate = itemTr.Attributes["templateName"];
                        if (trTemplate.Value == "head")
                        {
                            girdCfg.ColCount = itemTr.SelectNodes("th").Count;
                            itemTr.Attributes.Remove(trTemplate);
                        }
                        else if (trTemplate.Value == "content")
                        {
                            itemTr.Attributes.Remove(trTemplate);
                            girdCfg.TcontentMode = itemTr.OuterXml;//获取内容模板

                            itemTr.RemoveAll();
                            itemTr.InnerXml = "@flyingGrid";
                        }
                    }
                    girdCfg.TableMode = new StringBuilder(itemTable.OuterXml);
                    break;
                }
            }
        }

        /// <summary>
        /// 把list数据填充到表格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <returns></returns>
        private void ApendContentData<T>(List<T> dataList)
        {
            StringTemple st = new StringTemple(girdCfg.TcontentMode, "{", "}"); ;
            girdCfg.TableMode.Replace("<tr>@flyingGrid</tr>", st.Render(dataList));
        }

        //------------------------------------------------------------

        /// <summary>
        /// 生产表格属性
        /// </summary>
        /// <returns></returns>
        private void AppendTail(int dataCount, int pageNumber, int pageSize)
        {
            int pageCount = dataCount / pageSize;
            if (dataCount % pageSize != 0)
            {
                pageCount++;
            }
            string replaceMod = "pageNumber=@pageNumber  pageCount=@pageCount colCount=@colCount dataCount=@dataCount";
            if (girdCfg.Uspager == "true")
            {
                girdCfg.TableMode.Replace("usePager=\"true\"", "usePager=true " + replaceMod);
            }
            else
            {
                girdCfg.TableMode.Replace("usePager=\"false\"", "usePager=false " + replaceMod);
            }
            girdCfg.TableMode.Replace("@pageNumber", pageNumber.ToString());
            girdCfg.TableMode.Replace("@pageCount", pageCount.ToString());
            girdCfg.TableMode.Replace("@colCount", girdCfg.ColCount.ToString());
            girdCfg.TableMode.Replace("@dataCount", dataCount.ToString());
        }
    }
}
