using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Fg2
{
    /*---------------------------
    名称：flyingGrid2
    作者：weichengdong
    邮件: weichengdong2008@foxmail.com
    ---------------------------*/
    public class Fg2Cfg
    {       
        public StringBuilder TableMode{get;set;}
        public string TcontentMode { get; set; }//内容模板
        public string Uspager { get; set; } //启用分页
        public int PageSize { get; set; }  //页大小
        public string TableName { get; set; }//表名称      
        public int ColCount { get; set; }  //表格列数
    }
}