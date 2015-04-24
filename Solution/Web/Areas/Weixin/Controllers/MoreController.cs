using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Controllers
{
    public class MoreController : Models.QinjiluController
    {
        //
        // GET: /Weixin/More/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateticks">记录所属的天 从2010-1-1后面加上该天数</param>
        /// <returns></returns>
        public ActionResult Index(UInt16 dateticks = 0)
        {
           var res =  new Core.Services().Get(OpenId,null, dateticks);
           return View(res);
        }


        public ActionResult Post(string recordId, Core.FieldName fieldName, List<Core.Options> opts)
        {
            new Core.Services().Set(OpenId, recordId, fieldName, opts);
            return Json("");
        }


    }
}
