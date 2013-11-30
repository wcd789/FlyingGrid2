using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Demo.Entity;
using Fg2;

namespace Demo.Controllers
{
    public class FlyingGridDevController : Controller
    {
        //默认页
        public ActionResult FlyingGridAjaxDev()
        {
            return View();
        }

        //通过页面直接绑定的方式
        public ActionResult FlyingGridDev()
        {
            //数据源
            List<fyUserInfo> uinfo = new List<fyUserInfo>();
            for (int i = 0; i < 1000; i++)
            {
                fyUserInfo useTemp = new fyUserInfo();
                useTemp.UserId = i;
                useTemp.UserName = "weichengdong";
                useTemp.Age = i + 15;
                useTemp.School = "武汉" + i.ToString();
                useTemp.No = 100000 + i;
                uinfo.Add(useTemp);
            }
            string gridConfigFileName = AppDomain.CurrentDomain.BaseDirectory + "FlyingGrid2/flyingGrid2.xml";
            FlyingGrid2 fg2 = new FlyingGrid2("Fly2Test",gridConfigFileName);
            List<fyUserInfo> pageData = uinfo.GetRange(0, fg2.PageSize);         
            string result = fg2.LoadGrid(pageData,1,1000);

            ViewBag.table = result;
            return View();
        }

        //通过ajax请求的方式
        public ActionResult AjaxCtrl(int pageNumber)
        {
            //数据源
            List<fyUserInfo> uinfo = new List<fyUserInfo>();
            for (int i = 0; i < 1000; i++)
            {
                fyUserInfo useTemp = new fyUserInfo();
                useTemp.UserId = i;
                useTemp.UserName = "weichengdong";
                useTemp.Age = i+15;
                useTemp.School = "武汉" + i.ToString();
                useTemp.No = 100000 + i;
                uinfo.Add(useTemp);
            }
            //模拟分页
            string gridConfigFileName = AppDomain.CurrentDomain.BaseDirectory + "FlyingGrid2/flyingGrid2.xml";
            FlyingGrid2 fg2 = new FlyingGrid2("Fly2Test", gridConfigFileName);
            List<fyUserInfo> pageData = uinfo.GetRange((pageNumber - 1) * fg2.PageSize, fg2.PageSize);
          
            string result = fg2.LoadGrid(pageData, pageNumber, 1000);
            return Content(result);
        }
    }
}
