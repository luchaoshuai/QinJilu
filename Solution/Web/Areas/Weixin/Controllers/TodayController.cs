using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QinJilu.Web.Areas.Weixin.Controllers
{
    public class TodayController : Models.QinjiluController
    {
        //
        // GET: /Weixin/Today/

        public ActionResult Index(string date="")
        {
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToShortDateString();
            }


            DateTime dt = DateTime.Parse(date);
            QinJilu.Core.RecordInfo info = new Core.Services().Get(OpenId, null, dt);



            var sel_tags = new QinJilu.Core.Services().GetRecordTags(info.Id);
            var cur_notes = new QinJilu.Core.Services().GetNotes(OpenId, null, info.DateTicks);

            ViewBag.sel_tags = sel_tags;
            ViewBag.cur_notes = cur_notes;

            return View(info);
        }

        public ActionResult NewNote(string note, ushort dateticks)
        {
            if (string.IsNullOrEmpty(note))
            {
                return Content("nul");
            }
            new Core.Services().AddNote(OpenId, null, note, dateticks);
            return Content("ok");
        }

    }
}
