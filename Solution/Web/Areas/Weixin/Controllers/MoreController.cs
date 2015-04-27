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
        // GET: /Weixin/More/Index

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateticks">记录所属的天 从2010-1-1后面加上该天数</param>
        /// <returns></returns>
        public ActionResult Index(UInt16 dateticks = 0)
        {
            var res = new Core.Services().Get(OpenId, null, dateticks);

            var all_tags = new QinJilu.Core.Services().GetTags(OpenId);
            var sel_tagIds = new QinJilu.Core.Services().GetRecordTagIds(res.Id);
            var cur_notes = new QinJilu.Core.Services().GetNotes(OpenId,null, dateticks);

            ViewBag.all_tags = all_tags;
            ViewBag.sel_tagIds = sel_tagIds;
            ViewBag.cur_notes = cur_notes;

            return View(res);
        }


        public ActionResult Post(string recordId, Core.FieldName fieldName, Core.Options opt = Core.Options.nul)
        {
            new Core.Services().Set(OpenId, recordId, fieldName, opt);
            return Content("ok");
        }

        public ActionResult NewTag(string tag)
        {
            var tagId = new Core.Services().AddTag(OpenId, null, tag);
            return Json(new { res = tagId != MongoDB.Bson.ObjectId.Empty, tagId = tagId.ToString(), tag = tag });
        }
        public ActionResult DelTag(string tagId)
        {
            new Core.Services().DelTag(OpenId, null, tagId);
            return Content("ok");
        }


        public ActionResult SelectedTag(string recordId, string tagId, bool selected)
        {
            new Core.Services().Select(OpenId,recordId,tagId,selected);
            return Content("ok");
        }


    }
}
