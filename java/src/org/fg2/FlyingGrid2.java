package org.fg2;

import java.io.File;
import java.util.Iterator;
import java.util.List;
import org.dom4j.Attribute;
import org.dom4j.Document;
import org.dom4j.DocumentException;
import org.dom4j.Element;
import org.dom4j.io.SAXReader;

/**
 *
 * @author weichengdong
 */
public class FlyingGrid2 {

    private Fg2Cfg girdCfg = new Fg2Cfg();

    public int getPageSize() {
        return girdCfg.PageSize;
    }

    public String getTableName() {
        return girdCfg.TableName;
    }

    public FlyingGrid2(String tableName, String gridFullConfigFileName) throws DocumentException {
        girdCfg.TableName = tableName;
        String fullXmlConfigPath = gridFullConfigFileName;
        this.loadXmlConfig(girdCfg.TableName, fullXmlConfigPath);
    }

    public <T> String loadGrid(List<T> dataList, int pageNumber, int dataCount) throws Exception {
        if (dataCount <= 0) {
            dataCount = dataList.size();
        }
        if (pageNumber <= 0) {
            pageNumber = 1;
        }
        int pageSize = girdCfg.PageSize;
        if (pageSize <= 0) {
            pageSize = dataList.size();
        }
        try {
            return this.getGrid(dataList, pageNumber, dataCount, pageSize);
        } catch (Exception e) {
            // e.printStackTrace();
            throw new Exception("数据表格加载错误");
        }
    }

    public <T> String loadGrid(List<T> dataList) {
        try {
            int dataCount = dataList.size();
            int pageNumber = 1;
            int pageSize = dataList.size();
            return this.getGrid(dataList, pageNumber, dataCount, pageSize);
        } catch (Exception e) {
            return "表格加载错误";
        }
    }

    private <T> String getGrid(List<T> dataList, int pageNumber, int dataCount, int pageSize) throws Exception {
        this.appendContendData(dataList);
        this.appendTail(dataCount, pageNumber, pageSize);
        return girdCfg.TableMode;
    }

    private void loadXmlConfig(String tableName, String xmlfilepath) throws DocumentException {
        SAXReader reader = new SAXReader();
        Document xmlDoc = reader.read(new File(xmlfilepath));
        Element rootElm = xmlDoc.getRootElement();
        List tables = rootElm.elements("table");
        for (Iterator it = tables.iterator(); it.hasNext();) {
            Element itemTable = (Element) it.next();
            Attribute tableId=itemTable.attribute("id");
            if (tableId.getValue().equals(tableName)) {
                Attribute usePager = itemTable.attribute("usePager");
                girdCfg.Uspager = usePager.getValue();

                Attribute pageSize = itemTable.attribute("pageSize");
                girdCfg.PageSize = Integer.valueOf(pageSize.getValue());

                List trs = itemTable.elements("tr");
                for (Iterator it2 = trs.iterator(); it2.hasNext();) {
                    Element itemTr = (Element) it2.next();
                    Attribute trTemplate = itemTr.attribute("templateName");
                    if (trTemplate.getValue().equals("head")) {
                        girdCfg.ColCount = itemTr.elements("th").size();
                        itemTr.remove(trTemplate);
                    } else if (trTemplate.getValue().equals("content")) {
                         itemTr.remove(trTemplate);
                        girdCfg.TcontentMode = itemTr.asXML();                      

                        itemTr.clearContent();
                        itemTr.addText("@flyingGrid");                     
                    }
                }
                girdCfg.TableMode = itemTable.asXML();
                break;
            }
        }
    }

    private <T> void appendContendData(List<T> dataList) throws Exception {
        StringTemple st = new StringTemple(girdCfg.TcontentMode, "{", "}");
        String tableMode=st.render(dataList);
        girdCfg.TableMode = girdCfg.TableMode.replaceFirst("<tr>@flyingGrid</tr>", tableMode);
    }

    private void appendTail(int dataCount, int pageNumber, int pageSize) {
        int pageCount = dataCount / pageSize ;
        if(dataCount % pageSize!=0 ){
            pageCount++;
        }
        String tableMode = girdCfg.TableMode;
        String replaceMod = "pageNumber=@pageNumber  pageCount=@pageCount colCount=@colCount dataCount=@dataCount";
        if (girdCfg.Uspager.equals("true")) {
            tableMode = tableMode.replaceFirst("usePager=\"true\"", "usePager=true " + replaceMod);
        } else {
            tableMode = tableMode.replaceFirst("usePager=\"false\"", "usePager=false " + replaceMod);
        }
        tableMode = tableMode.replaceFirst("@pageNumber", String.valueOf(pageNumber));
        tableMode = tableMode.replaceFirst("@pageCount", String.valueOf(pageCount));
        tableMode = tableMode.replaceFirst("@colCount", String.valueOf(girdCfg.ColCount));
        tableMode = tableMode.replaceFirst("@dataCount", String.valueOf(dataCount));
        girdCfg.TableMode = tableMode;
    }
}
