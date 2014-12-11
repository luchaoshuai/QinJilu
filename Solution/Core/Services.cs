﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//  Cache 缓存，Asynchronous 异步，Concurrent 并行
namespace QinJilu.Core
{
    public class Services
    {
        #region config
        public static bool NeedInvitationCode()
        {
            var o = RedisHelper.GetObject("NeedInvitationCode");
            if (o != null)
            {
                return (bool)o;
            }

            bool res = false;
            RedisHelper.AddObject("NeedInvitationCode", res);
            return res;
        }
        public static void NeedInvitationCode(bool need)
        {
            RedisHelper.AddObject("NeedInvitationCode", need);
        }
        #endregion


        #region subscribe
        /// <summary>
        /// 订阅
        /// </summary>
        public UserInfo Subscribe(string openId)
        {
            Repository.DbSet.UserRegister(openId);
            return GetUser(openId);
        }

        private UserInfo GetUser(string openId)
        {
            return Repository.DbSet.GetUser(openId);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        public void UnSubscribe(string openId)
        {
            Repository.DbSet.UserUnregister(openId);
        }
        #endregion

        /// <summary>
        /// 输入昵称，选择性别
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="nickname"></param>
        /// <param name="gender"></param>
        public void SetBaseInfo(string openId, string nickname, Gender gender)
        {
            Repository.DbSet.SetBaseInfo(openId, nickname, gender);
        }

        // 若为女生，则录入 经期基本信息
        public void IamWoman(string openId, DateTime lasterCycleStart,
            int cycleTypically, int cycleVaries,
            int periodTypically, int periodVaries,
            int pmsTypically, int pmsVaries)
        {
            Repository.DbSet.IamWoman(openId, lasterCycleStart,
                cycleTypically, cycleVaries,
                periodTypically, periodVaries,
                pmsTypically, pmsVaries);
        }


        public bool AnyEmail(string email)
        {
            return Repository.DbSet.AnyEmail(email);
        }



        // 通过邮箱查找那个她，只能返回女生
        public string FindShe(string email)
        {
            string sheId = Repository.DbSet.FindShe(email);
            return sheId;
        }


        #region 女神

        // 若为男生，则录入她的邮箱，请求设置为闺蜜并默认启用编辑。
        // 邀请 女神  ，我来帮你记录
        public void InvitationGoddess(string openId, string sheId, string notename, string invitationNote)
        {
            FriendInvitation fi = new FriendInvitation
            {
                UserId = GetUserId(openId),
                FriendId = MongoDB.Bson.ObjectId.Parse(sheId),
                Operations = Operation.Editable,
                Notename = notename,
                InvitationNote = invitationNote,
                CreateOn = DateTime.Now,

                OpinionOn = DateTimeEx.NullDate,
                Opinions = Opinion.Untreated,

                SheIsGoddess = true
            };
            Repository.DbSet.GetCollection<FriendInvitation>().Insert(fi);
        }
        /// <summary>
        /// 女神 审核 男生的邀请,并设置相应的备注，权限
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="invId"></param>
        /// <param name="opi"></param>
        /// <param name="oper"></param>
        /// <param name="notename"></param>
        /// <param name="msg"></param>
        public void GoddessOpinion(string openId,
            string invId, Opinion opi,
            Operation oper, string notename, string msg)
        {
            var fi = Repository.DbSet.GetCollection<FriendInvitation>().FindOneById(MongoDB.Bson.ObjectId.Parse(invId));
            if (fi == null)
            {
                return;
            }
            var cId = GetUserId(openId);
            if (fi.FriendId != cId)
            {
                // 当前用户不是女神(被邀请人)。 不能处理。
                return;
            }

            Repository.DbSet.InvitationOpinion(fi.Id, opi);

            if (opi == Opinion.Agreed)
            {
                if (fi.SheIsGoddess)
                {
                    Repository.DbSet.SetShe(fi.UserId, fi.FriendId);
                }


                // add one friend ??  
                var fs = new List<Friends>(2);
                //申请人增加一个朋友  >>> 申请人 在朋友列表中可以看到，并对其进行权限设置 
                fs.Add(new Friends
                {
                    UserId = fi.UserId,
                    FriendId = fi.FriendId,
                    Notename = fi.Notename,// 申请时给女神取的备注名
                    Operations = Operation.ReadOnly //默认只给读的权限。
                });// 男号这边没有日志，不会发表文章，总是使用的她号，所以这个朋友列表无意义？

                // 给女神那边也增加一个朋友
                fs.Add(new Friends
                 {
                     UserId = fi.FriendId,
                     FriendId = fi.UserId,
                     Notename = notename,// 审核时，女神给男生取的备注名
                     Operations = oper// 以女神最后给的权限为准
                 });

                Repository.DbSet.GetCollection<Friends>().InsertBatch(fs);
            }


        }
        #endregion


        #region 朋友

        /// <summary>
        /// 邀请 朋友
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="FriendId"></param>
        /// <param name="notename"></param>
        /// <param name="invitationNote"></param>
        public void InvitationFriend(string openId, string FriendId, Operation oper, string notename, string invitationNote)
        {
            FriendInvitation fi = new FriendInvitation
            {
                UserId = GetUserId(openId),
                FriendId = MongoDB.Bson.ObjectId.Parse(FriendId),
                Operations = oper,
                Notename = notename,
                InvitationNote = invitationNote,
                CreateOn = DateTime.Now,

                OpinionOn = DateTimeEx.NullDate,
                Opinions = Opinion.Untreated,

                SheIsGoddess = false
            };

            Repository.DbSet.GetCollection<FriendInvitation>().Insert(fi);
        }
        // 对方朋友 审核 是否可以添加为朋友
        public void FriendOpinion(string openId,
            string invId, Opinion opi,
            Operation oper, string notename, string msg)
        {
            var fi = Repository.DbSet.GetCollection<FriendInvitation>().FindOneById(MongoDB.Bson.ObjectId.Parse(invId));
            if (fi == null)
            {
                return;
            }
            var cId = GetUserId(openId);
            if (fi.FriendId != cId)
            {
                // 当前用户不是(被邀请人)。 不能处理。
                return;
            }

            Repository.DbSet.InvitationOpinion(fi.Id, opi);

            if (opi == Opinion.Agreed)
            {
                // add one friend ??  
                var fs = new List<Friends>(2);
                //申请人增加一个朋友  >>> 申请人 在朋友列表中可以看到，并对其进行权限设置 
                fs.Add(new Friends
                {
                    UserId = fi.UserId,
                    FriendId = fi.FriendId,
                    Notename = fi.Notename,// 申请时给“他”取的备注名
                    Operations = Operation.ReadOnly //默认只给读的权限。
                });

                // 给“他”那边也增加一个朋友
                fs.Add(new Friends
                {
                    UserId = fi.FriendId,
                    FriendId = fi.UserId,
                    Notename = notename,// 审核时，“他”给男生取的备注名
                    Operations = oper// 以“他”最后给的权限为准
                });

                Repository.DbSet.GetCollection<Friends>().InsertBatch(fs);
            }


        }

        #endregion

        public MongoDB.Bson.ObjectId GetUserId(string openId)
        {
            var o = RedisHelper.GetObject("GetUserId" + openId);
            if (o != null)
            {
                return (MongoDB.Bson.ObjectId)o;
            }

            var t = Repository.DbSet.GetUserId(openId);
            RedisHelper.AddObject("GetUserId" + openId, t);
            return t;

        }


        public void ApplyFriend(string openId, string sheId)
        {
        }











        #region record

        public void Begin(string openId, int dateticks)
        {
            // befor

            // ing
            // 队列缓存 Redis
            // 所有的数据库操作，先暂存在缓存队列中，集中存储。
            // 减少数据库交互次数,多个字段都更新完成以后一起提交。
            // 


            // sql  无索引的操作，总是通过主键
            // 写总是使用sql，成功后再到mongodb
            // 读总是使用mongodb
            // 定期做二者之前的比较同步。


            //  after

        }
        public void End(string openId, int dateticks)
        {

        }

        public void Set(string openId, FieldName fName, Options opt, int dateticks)
        {
            switch (fName)
            {
                case FieldName.Period:
                    break;
                case FieldName.Pain:
                    break;
                case FieldName.Sex:
                    break;
                case FieldName.Mood:
                    break;
                case FieldName.Fluid:
                    break;
                case FieldName.Pill:
                    break;
                case FieldName.Temperature:
                    break;
                case FieldName.Tags:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 设置温度
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="tagId"></param>
        public void Temperature(string openId, Int16 temperature, bool unreliable, int dateticks)
        {

        }

        #region tag

        /// <summary>
        /// 输入一个标签
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="tag"></param>
        public void Add(string openId, string tag, int dateticks)
        {

        }
        /// <summary>
        /// 选择一个标签
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="tagId"></param>
        public void Add(string openId, int tagId, int dateticks)
        {

        }
        /// <summary>
        /// 删除一个标签
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="tagId"></param>
        public void Del(string openId, int tagId, int dateticks)
        {

        }

        #endregion

        #endregion

        /// <summary>
        /// 收到 位置 请求后的响应
        /// </summary>
        public void Today()
        {

        }

    }
}
