﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Controllers
{
    public class FriendController : Models.QinjiluController
    {
        //
        // GET: /Weixin/Friend/

        public ActionResult Index()
        {
            return View();
        }

    }
}
