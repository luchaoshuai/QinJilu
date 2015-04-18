
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace QinJilu.Core.Repository
{
    internal class DbSet
    {

        public static readonly string connectionString_Default = "mongodb://" + System.Configuration.ConfigurationManager.ConnectionStrings["qinMongoDb"].ConnectionString;
        public static readonly string database_Default = "QinJilu";

        private static Dictionary<string, object> dict = new Dictionary<string, object>();
        public static MongoCollection<T> GetCollection<T>()
        {
            string collectionName = typeof(T).Name;
            if (!dict.ContainsKey(collectionName))
            {
                var client = new MongoClient(connectionString_Default);
                var server = client.GetServer();
                var database = server.GetDatabase(database_Default);
                var collection = database.GetCollection<T>(collectionName);
                dict[collectionName] = collection;
            }
            return dict[collectionName] as MongoCollection<T>;
        }

        #region UserInfo

        public static UserInfo GetUser(string openId)
        {
            var collection = GetCollection<UserInfo>();
            var u = collection.AsQueryable().Where(x => x.WeixinOpenID == openId).FirstOrDefault();
            return u;
        }

        public static MongoDB.Bson.ObjectId GetUserId(string openId)
        {
            var uId = GetCollection<UserInfo>().AsQueryable()
                .Where(x => x.WeixinOpenID == openId)
                .Select(x => x.Id)
                .FirstOrDefault();
            return uId;
        }

        /// <summary>
        /// 用户注册，若存在，则更新。
        /// </summary>
        /// <param name="openId"></param>
        internal static void UserRegister(string openId)
        {
            var collection = GetCollection<UserInfo>();
            var e = collection.AsQueryable().Any(x => x.WeixinOpenID == openId);
            if (e)
            {
                //var query1 = Query<Student>.GTE<Int32>(t => t.Age, 10);
                //var query2 = Query<Student>.LTE<Int32>(t => t.Age, 15);
                ////var query = Query.And(Query.GTE("Age", 10), Query.LTE("Age", 15));
                //var query = Query.And(query1, query2);

                var q = Query<UserInfo>
                    .EQ<string>(x => x.WeixinOpenID, openId);

                var u = Update<UserInfo>
                    .Set<bool>(t => t.Unsubscribe, false)
                    .Inc(x => x.SubscribeCount, 1);

                collection.Update(q, u);
            }
            else
            {
                var u = new UserInfo
                {
                    // 女的才有
                    FirstYaer = 0,

                    LasterCycleStart = DateTimeEx.NullDate,
                    CycleTypically = 0,
                    CycleVaries = 0,
                    PeriodTypically = 0,
                    PeriodVaries = 0,
                    PmsTypically = 0,
                    PmsVaries = 0,

                    Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                    SheId = MongoDB.Bson.ObjectId.Empty,

                    Nickname = string.Empty,
                    WeixinOpenID = openId,
                    CreateOn = DateTime.Now,
                    Gender = Gender.Unknow,
                    BirthDay = DateTimeEx.NullDate,
                    Email = string.Empty,
                    Passcoed = string.Empty,
                    SubscribeCount = 1,
                    Unsubscribe = false,
                    UnsubscribeOn = DateTimeEx.NullDate
                };
                collection.Insert(u);
            }
            return;
        }


        /// <summary>
        /// 用户退订,直接更新。
        /// </summary>
        /// <param name="openId"></param>
        internal static void UserUnregister(string openId)
        {
            var collection = GetCollection<UserInfo>();

            var q = Query<UserInfo>
                .EQ<string>(x => x.WeixinOpenID, openId);

            var u = Update<UserInfo>
                .Set<bool>(t => t.Unsubscribe, true);

            collection.Update(q, u);
        }



        // 更新 姓别，昵称
        internal static void SetBaseInfo(string openId, string nickname, Gender gender)
        {
            var collection = GetCollection<UserInfo>();

            var q = Query<UserInfo>
                .EQ<string>(x => x.WeixinOpenID, openId);

            var u = Update<UserInfo>
                .Set<string>(t => t.Nickname, nickname)
                .Set<Gender>(t => t.Gender, gender);

            collection.Update(q, u);
        }

        // 更新 各周期时间
        internal static void IamWoman(string openId, DateTime lasterCycleStart,
            int cycleTypically, int cycleVaries,
            int periodTypically, int periodVaries,
            int pmsTypically, int pmsVaries)
        {
            var collection = GetCollection<UserInfo>();

            var q = Query<UserInfo>
                .EQ<string>(x => x.WeixinOpenID, openId);

            var u = Update<UserInfo>
                .Set<DateTime>(t => t.LasterCycleStart, lasterCycleStart)

                .Set<int>(t => t.CycleTypically, cycleTypically)
                .Set<int>(t => t.CycleVaries, cycleVaries)

                .Set<int>(t => t.PeriodTypically, periodTypically)
                .Set<int>(t => t.PeriodVaries, periodVaries)

                .Set<int>(t => t.PmsTypically, pmsTypically)
                .Set<int>(t => t.PmsVaries, pmsVaries);

            collection.Update(q, u);
        }

        internal static void SetShe(MongoDB.Bson.ObjectId userId, MongoDB.Bson.ObjectId sheId)
        {
            var collection = GetCollection<UserInfo>();

            var q = Query<UserInfo>
                .EQ<MongoDB.Bson.ObjectId>(x => x.Id, userId);

            var u = Update<UserInfo>
                .Set<MongoDB.Bson.ObjectId>(t => t.SheId, sheId);

            collection.Update(q, u);
        }

        internal static string FindShe(string email)
        {
            var collection = GetCollection<UserInfo>();

            var q = Query<UserInfo>.EQ<string>(x => x.Email, email);

            var u = collection.FindOne(q);
            if (u != null && u.Gender == Gender.Woman)
            {
                return u.Id.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 是否存在某个邮箱地址
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal static bool AnyEmail(string email)
        {
            var collection = GetCollection<UserInfo>();

            return collection.AsQueryable().Any(x => x.Email == email.ToLower());
        }

        #endregion


        #region friend

        /// <summary>
        /// 发送朋友请求,自动判断是否已经发送过
        /// </summary>
        /// <param name="applyerId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        internal static void SendInvitation(FriendInvitation info)
        {
            var collection = GetCollection<FriendInvitation>();

            MongoDB.Bson.ObjectId invId = MongoDB.Bson.ObjectId.Empty;
            // 判断前面是否存在请求,若不存在，则总是创建一条。
            while (!AnyInvitation(info.UserId, info.FriendId, ref invId))
            {
                collection.Insert(info);
            }

            var c = Query<FriendInvitation>
                .EQ<MongoDB.Bson.ObjectId>(x => x.Id, invId);

            var u = Update<FriendInvitation>

                .Set<DateTime>(t => t.CreateOn, DateTime.Now)

                .Set<string>(t => t.Notename, info.Notename)
                .Set<string>(t => t.InvitationNote, info.InvitationNote)

                .Set<Operation>(t => t.Operations, info.Operations)

                .Set<DateTime>(t => t.OpinionOn, DateTimeEx.NullDate)
                .Set<Opinion>(t => t.Opinions, Opinion.Untreated)
                    .Inc(x => x.SendCount, 1);

            collection.Update(c, u);
        }

        /// <summary>
        /// 是否存在历史请求朋友的记录
        /// </summary>
        /// <param name="applyerId">申请(发起)人Id</param>
        /// <param name="friendId">接受（被邀请）人Id</param>
        /// <returns>若存在，则返回之前的邀请记录Id</returns>
        internal static bool AnyInvitation(MongoDB.Bson.ObjectId applyerId, MongoDB.Bson.ObjectId friendId,ref MongoDB.Bson.ObjectId invId)
        {
            var collection = GetCollection<FriendInvitation>();

            var res = collection.AsQueryable().Any(x=>x.FriendId==friendId && x.UserId==applyerId);
            if (res)
            {
                invId = collection.AsQueryable().Where(x => x.FriendId == friendId && x.UserId == applyerId).Select(x => x.Id).First();
            }
            return res;
        }


        /// <summary>
        /// 更新 朋友邀请 的审核状态
        /// </summary>
        /// <param name="fi"></param>
        internal static void InvitationOpinion(MongoDB.Bson.ObjectId invId, Opinion opi)
        {
            var collection = GetCollection<FriendInvitation>();

            var q = Query<FriendInvitation>
                .EQ<MongoDB.Bson.ObjectId>(x => x.Id, invId);

            var u = Update<FriendInvitation>
                .Set<DateTime>(t => t.OpinionOn, DateTime.Now)
                .Set<Opinion>(t => t.Opinions, opi);

            collection.Update(q, u);
        }


        /// <summary>
        ///  是否存在朋友记录
        /// </summary>
        /// <param name="sheId">主人号(仅能为she)</param>
        /// <param name="friendId">朋友号</param>
        /// <returns></returns>
        internal static bool AnyFriend(MongoDB.Bson.ObjectId sheId, MongoDB.Bson.ObjectId friendId)
        {
            var collection = GetCollection<Friends>();
            var res = collection.AsQueryable().Any(x => x.FriendId == friendId && x.UserId == sheId);
            return res;
        }

        #endregion

    }
}
