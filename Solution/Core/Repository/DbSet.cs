
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


        #region Invitecode

        /// <summary>
        /// 取回邀请码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        internal static InvitecodeInfo GetInvitecodeInfo(string code)
        {
            var collection = GetCollection<InvitecodeInfo>();
            var c = collection.AsQueryable().Where(x => x.Invitecode == code).FirstOrDefault();
            return c;
        }

        internal static bool UseInvitecode(MongoDB.Bson.ObjectId userOpenId, string invitecode)
        {
            var q = Query<InvitecodeInfo>
                .EQ<string>(x => x.Invitecode, invitecode);

            var u = Update<InvitecodeInfo>
                .Set<InvitecodeStatus>(t => t.State, InvitecodeStatus.Used)
                .Set<MongoDB.Bson.ObjectId>(t => t.UsedUserId, userOpenId)
                .Set<DateTime>(t => t.UsedOn, DateTime.Now);

            return true;
        }


        #endregion

        #region UserInfo

        internal static UserInfo GetUser(string openId)
        {
            var collection = GetCollection<UserInfo>();
            var u = collection.AsQueryable().Where(x => x.WeixinOpenID == openId).FirstOrDefault();
            return u;
        }

        internal static MongoDB.Bson.ObjectId GetUserId(string openId)
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
                    .Set<CurrentScope>(t => t.CurrentContext, CurrentScope.NULL)
                    .Set<DateTime>(t => t.LastActivitytime, DateTime.Now)
                    .Inc(x => x.SubscribeCount, 1);

                collection.Update(q, u);
            }
            else
            {
                var u = new UserInfo
                {
                    // 女的才有
                    FirstYaer = 0,

                    CurrentContext = CurrentScope.NULL,
                    LastActivitytime = DateTime.Now,

                    LasterCycleStart = DateTimeEx.NullDate,
                    CycleTypically = 0,
                    CycleVaries = 0,
                    PeriodTypically = 0,
                    PeriodVaries = 0,
                    PmsTypically = 0,
                    PmsVaries = 0,

                    CheckInvitecode = false,
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
                .Set<DateTime>(t => t.UnsubscribeOn, DateTime.Now)
                .Set<bool>(t => t.Unsubscribe, true);

            collection.Update(q, u);
        }



        /// <summary>
        /// 更新 姓别
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="gender"></param>
        internal static void SetGender(string openId, Gender gender)
        {
            var collection = GetCollection<UserInfo>();

            var q = Query<UserInfo>
                .EQ<string>(x => x.WeixinOpenID, openId);

            var u = Update<UserInfo>
                .Set<Gender>(t => t.Gender, gender);

            collection.Update(q, u);
        }

        // 更新 各周期时间
        internal static void WomanInit(string openId,
            DateTime birthDay,
            DateTime lasterCycleStart,
            int cycleTypically, int cycleVaries,
            int periodTypically, int periodVaries,
            int pmsTypically, int pmsVaries)
        {
            var collection = GetCollection<UserInfo>();

            var q = Query<UserInfo>
                .EQ<string>(x => x.WeixinOpenID, openId);

            var u = Update<UserInfo>
                .Set<DateTime>(t => t.BirthDay, birthDay)
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

        internal static MongoDB.Bson.ObjectId FindShe(string email)
        {
            var collection = GetCollection<UserInfo>();

            var q = Query<UserInfo>.EQ<string>(x => x.Email, email);

            var u = collection.FindOne(q);
            if (u != null && u.Gender == Gender.Woman)
            {
                return u.Id;
            }
            return MongoDB.Bson.ObjectId.Empty;
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

        /// <summary>
        /// 是否存在历史请求朋友的记录
        /// </summary>
        /// <param name="applyerId">申请(发起)人Id</param>
        /// <param name="friendId">接受（被邀请）人Id</param>
        /// <returns>若存在，则返回之前的邀请记录Id</returns>
        internal static bool AnyInvitation(MongoDB.Bson.ObjectId applyerId, MongoDB.Bson.ObjectId friendId, ref MongoDB.Bson.ObjectId invId)
        {
            var collection = GetCollection<FriendInvitation>();

            var res = collection.AsQueryable().Any(x => x.FriendId == friendId && x.UserId == applyerId);
            if (res)
            {
                invId = collection.AsQueryable().Where(x => x.FriendId == friendId && x.UserId == applyerId).Select(x => x.Id).Single();
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


        #endregion


        #region record


        /// <summary>
        /// 取得指定sheId 某天的所有记录，不存在则创建一条空的。
        /// </summary>
        /// <param name="sheId"></param>
        /// <param name="dateticks"></param>
        /// <returns></returns>
        internal static RecordInfo Get(MongoDB.Bson.ObjectId editorId, MongoDB.Bson.ObjectId sheId, ushort dateticks)
        {
            var collection = GetCollection<RecordInfo>();
            var info = collection.AsQueryable().Where(x => x.SheId == sheId && x.DateTicks == dateticks).SingleOrDefault();
            if (info==null)
            {
                info = new RecordInfo()
                {
                    DateTicks = dateticks,
                    SheId = sheId,
                    BeginMark = false,
                    CreateOn = DateTime.Now,
                    CreaterId = editorId,
                    EditedOn = DateTime.Now,
                    EditorId = editorId,
                    EndMark = false,
                    Fluid = Options.NULL,
                    Mood = Options.NULL,
                    Pain = Options.NULL,
                    Period = Options.NULL,
                    Pill = Options.NULL,
                    Reliable = false,
                    Sex = Options.NULL,
                    Tags = string.Empty,
                    Temperature = 0,
                    Id = MongoDB.Bson.ObjectId.GenerateNewId()
                };
                collection.Insert(info);
            }
            return info;
        }

        internal static void Set(MongoDB.Bson.ObjectId editorId, MongoDB.Bson.ObjectId recordId,FieldName fName, Options opt)
        {
            var collection = Repository.DbSet.GetCollection<RecordInfo>();


            var q = MongoDB.Driver.Builders.Query<RecordInfo>
                .EQ<MongoDB.Bson.ObjectId>(x => x.Id, recordId);

            var u = MongoDB.Driver.Builders.Update<RecordInfo>
                .Set<DateTime>(t => t.EditedOn, DateTime.Now)
                .Set<MongoDB.Bson.ObjectId>(t => t.EditorId, editorId);

            switch (fName)
            {
                case FieldName.Period:
                    u = u.Set<Options>(t => t.Period, opt);
                    break;
                case FieldName.Pain:
                    u = u.Set<Options>(t => t.Pain, opt);
                    break;
                case FieldName.Sex:
                    u = u.Set<Options>(t => t.Sex, opt);
                    break;
                case FieldName.Mood:
                    u = u.Set<Options>(t => t.Mood, opt);
                    break;
                case FieldName.Fluid:
                    u = u.Set<Options>(t => t.Fluid, opt);
                    break;
                case FieldName.Pill:
                    u = u.Set<Options>(t => t.Pill, opt);
                    break;
                case FieldName.Temperature:
                    break;
                case FieldName.Tags:
                    break;
                default:
                    break;
            }
            collection.Update(q, u);
        }



        internal static void Set(MongoDB.Bson.ObjectId editorId, MongoDB.Bson.ObjectId recordId,short temperature, bool reliable)
        {
            var collection = Repository.DbSet.GetCollection<RecordInfo>();


            var q = MongoDB.Driver.Builders.Query<RecordInfo>
                .EQ<MongoDB.Bson.ObjectId>(x => x.Id, recordId);

            var u = MongoDB.Driver.Builders.Update<RecordInfo>
                .Set<DateTime>(t => t.EditedOn, DateTime.Now)
                .Set<MongoDB.Bson.ObjectId>(t => t.EditorId, editorId)
                .Set<short>(t => t.Temperature, temperature)
                .Set<bool>(t => t.Reliable, reliable);

            collection.Update(q, u);

        }

        #endregion

    }
}
