/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package org.fg2;

import java.lang.reflect.Field;
import java.util.List;

/**
 *
 * @author weichengdong
 */
public class StringTemple {

    private String _template;
    private String _delimiterStart = "{";
    private String _delimiterStop = "}";

    public StringTemple(String stringTemple) {
        this._template = stringTemple;
    }

    public StringTemple(String stringTemple, String startDelimiter, String stopDelimiter) {
        this._delimiterStart = startDelimiter;
        this._delimiterStop = stopDelimiter;
        this._template = stringTemple;
    }

    private String getPropertyList(Object obj) throws Exception {
        String stringTemple = new String(_template);
        try {
            Field[] properties = obj.getClass().getFields();
            for (Field property : properties) {
                Object o = property.get(obj);
                String strvalue = (o == null ? "" : o.toString());
                System.out.print(stringTemple);
                stringTemple =  this.replaceAll(stringTemple,_delimiterStart + property.getName() + _delimiterStop, strvalue);            
            }
        } catch (Exception e) {
            throw e;
        }
        return stringTemple;
    }

    //重新java replaceAll
    private String replaceAll(String sourceStr,String oldStr, String newStr) {
        if (sourceStr == null || oldStr == null || newStr == null) {
            return null;
        }      
        StringBuilder sbSourceStr = new StringBuilder(sourceStr);
        int index = sbSourceStr.indexOf(oldStr);
        while (index != -1) {    
          sbSourceStr.replace(index, index+oldStr.length(), newStr);
          index = sbSourceStr.indexOf(oldStr);
        }
        return sbSourceStr.toString();
    }

    public <T> String render(T data) throws Exception {
        return this.getPropertyList(data);
    }

    public <T> String render(List<T> dataList) throws Exception {
        StringBuilder strTableTrs = new StringBuilder();
        int length = dataList.size();
        for (int i = 0; i < length; i++) {
            strTableTrs.append(this.render(dataList.get(i)));
        }
        return strTableTrs.toString();
    }
}
