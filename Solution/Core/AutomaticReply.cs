using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    // 自动应答
    public class AutomaticReply
    {

        public enum nodeType
        {
            /// <summary>
            /// 返回的消息节点
            /// </summary>
            msg,
            /// <summary>
            /// 判断用节点
            /// </summary>
            judge
        }
        public enum msgType
        {
            nul,
            text,
            img
        }
        public class defineItem
        {
            /// <summary>
            /// 主键，所有记录都是唯一的
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// 消息所属上级
            /// </summary>
            public string ParentId { get; set; }

            /// <summary>
            /// 记录的别名，便于人为查看
            /// </summary>
            public string alias { get; set; }

            /// <summary>
            /// 节点类型
            /// </summary>
            public nodeType nodeType { get; set; }
            /// <summary>
            /// 返回给用户的消息类型  文本，图文，链接，方法？接口调用？，重复之前的defineItem.Id
            /// </summary>
            public msgType msgType { get; set; }

            /// <summary>
            /// 返回前要处理的事情--变量准备
            /// </summary>
            public string beforTodo { get; set; }
            /// <summary>
            /// 返回后要处理的事情--更新数据库？
            /// </summary>
            public string afterTodo { get; set; }
            /// <summary>
            /// 所有需要处理好的变量名，下面的消息模板中会用。
            /// </summary>
            public List<string> Vars { get; set; }
            /// <summary>
            /// 返回给用户的消息模板  可以使用变量替换
            /// </summary>
            public string msgTpl { get; set; }

            /// <summary>
            /// 输出匹配规格 （要返因这条消息，用户的输入必须匹配该正则）--- 》2 ， 《2 各种运算也要能支持的哦。
            /// </summary>
            public string KeyRule { get; set; }

        }

        public class runItem
        {
            //上一条消息的Id， 从这个parentId下面找child item
            public int prevItemId { get; set; }
            //回复消息的用户id
            public int userId { get; set; }
            //用户输入的消息
            public string inputText { get; set; }

            // 返回给用户的消息定义
            public string outputItemId { get; set; }
            //返回给用户的消息--根据上面这条定义，变量替换过后的消息。
            public string output { get; set; }
        }


        public class test
        {

            List<defineItem> defines = new List<defineItem>();
            Dictionary<string, string> dictCurrent = new Dictionary<string, string>();

            public void run(string openId, string input)
            {
                if (!dictCurrent.ContainsKey(openId))
                {
                    dictCurrent[openId] = "0";
                }
                string cid = dictCurrent[openId];
                var childs = defines.Where(x => x.ParentId == cid).ToList();


                string msg;
                defineItem child = null;
                if (childs.Count > 0)
                {
                    child = childs.Where(x => x.KeyRule == input).First();

                }

                if (child == null)// 只有一个，那就不用管输入了，总是执行就是了。  或着条件都不符合，则使用【第一个】匹配无效输入
                {
                    child = childs.First();
                }


                if (!string.IsNullOrEmpty(child.beforTodo))// 有事件前方法
                {
                    if (child.beforTodo.StartsWith("call_method:"))
                    {
                        var methodName = child.beforTodo.Substring("call_method:".Length);
                        var res = RedisHelper.GetObject(methodName).ToString();

                        // 得到方法结果值后，再查找下一级。
                        childs = defines.Where(x => x.ParentId == child.Id).ToList();
                        var c1 = childs.Where(x => x.KeyRule == res).First();
                        if (c1.nodeType == nodeType.msg)
                        {
                            if (c1.msgType == msgType.text)
                            {
                                msg = c1.msgTpl;
                            }
                        }
                        else
                        {

                        }
                    }
                }

                if (!string.IsNullOrEmpty(child.afterTodo))
                {
                    if (child.afterTodo.StartsWith("changeCurrentTo:"))
                    {
                        var nodeId = child.beforTodo.Substring("changeCurrentTo:".Length);

                        dictCurrent[openId] = nodeId;
                    }
                }


            }

            public void init()
            {
                //  call_method:xxx
                //  changeCurrentTo:nn


                //  beforTodo = call_method:xxxMethod   方法的输入只有二个参数：openId,objectInput?(图片？文本？位置。。。)
                //  isRepeat(string openId)
                //  validInvitecode(string input)
                //  validGoddessEmail(string input)

                //  changeCurrentTo(itemId)  // 修改当前的位置为itemid，以便于收到用户响应时，知道去哪里找。  >> 找parentId=itemid的child中匹配input

                #region defines.Add

                // 邀请码  1
                defines.Add(new defineItem
                {
                    ParentId = "0",
                    Id = "1",
                    alias = "判断是否为第一次",
                    beforTodo = "call_method:isRepeat",// 调用方法 isRepeat ,返回值（string）为下一个节点的input rule
                    afterTodo = "goto_child_item",// 直接跳到下级，匹配  input rule，返回消息
                    KeyRule = "",
                    msgTpl = "",
                    msgType = msgType.nul,
                    nodeType = nodeType.judge,
                    Vars = new List<string>()

                });
                defines.Add(new defineItem
                {
                    ParentId = "1",
                    Id = "11",
                    alias = "首次关注》输入邀请码",
                    beforTodo = "",
                    afterTodo = "changeCurrentTo:111",
                    KeyRule = "isFirst",
                    msgTpl = "亲,感谢您第一次关注。请输入邀请码上传了。直接输入8位号或着扫描二维码都行的。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });
                defines.Add(new defineItem
                {
                    ParentId = "1",
                    Id = "12",
                    alias = "再次关注》输入邀请码",
                    beforTodo = "",
                    afterTodo = "changeCurrentTo:111",
                    KeyRule = "isRepeat",
                    msgTpl = "亲,感谢您再次关注。请输入邀请码上传了。直接输入8位号或着扫描二维码都行的。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });

                defines.Add(new defineItem
                {
                    ParentId = "0",
                    Id = "111",
                    alias = "判断邀请码是否有效",
                    beforTodo = "call_method:validInvitecode",// 调用方法 validInvitecode ,返回值（InvitecodeStatus.tostring）为下一个节点的input rule
                    afterTodo = "goto_child_item",
                    KeyRule = "",
                    msgTpl = "",
                    msgType = msgType.nul,
                    nodeType = nodeType.judge,
                    Vars = new List<string>()
                });



                defines.Add(new defineItem
                {
                    ParentId = "111",
                    Id = "1112",
                    alias = "邀请码未出生",
                    beforTodo = "",
                    afterTodo = "",     // 不需要更改上下文的位置，因为下次用户输入以后，还是调用一样的方法判断。
                    KeyRule = "Unborn",
                    msgTpl = "亲，这个号码我们还没有发放吧！请重新输入8位号或着扫描二维码。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });
                defines.Add(new defineItem
                {
                    ParentId = "111",
                    Id = "1113",
                    alias = "邀请码未出生",
                    beforTodo = "",
                    afterTodo = "",
                    KeyRule = "Used",
                    msgTpl = "亲，这个号码已经被别人用过了！请重新输入8位号或着扫描二维码。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });

                defines.Add(new defineItem
                {
                    ParentId = "111",
                    Id = "1114",
                    alias = "邀请码暂不可用",
                    beforTodo = "",
                    afterTodo = "",
                    KeyRule = "Disabled",
                    msgTpl = "亲，这个号码暂不可用！是不是不在有效可用期内呢？请重新输入8位号或着扫描二维码。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });
                defines.Add(new defineItem
                {
                    ParentId = "111",
                    Id = "1111",
                    alias = "邀请码有效可用并提示性别输入",
                    beforTodo = "",
                    afterTodo = "changeCurrentTo:21",    //切换到item21，性别输入判断接管
                    KeyRule = "Availabled",
                    msgTpl = "亲,邀请码有效，请输入性别：1，表男人，2为女人",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });


                // 性别  2

                defines.Add(new defineItem
                {
                    ParentId = "0",
                    Id = "2",
                    alias = "仅提示输入性别",
                    beforTodo = "",
                    afterTodo = "changeCurrentTo:21",
                    KeyRule = "Availabled",
                    msgTpl = "亲,请输入性别：1，表男人，0为女人",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });

                defines.Add(new defineItem
                {
                    ParentId = "0",
                    Id = "21",
                    alias = "判断性别输入是否有效",
                    beforTodo = "",
                    afterTodo = "goto_child_item",// 直接跳到下级，匹配  input rule，返回消息。只有这一种操作，不需要定义？再继续调用内部的方法呢？
                    KeyRule = "",
                    msgTpl = "",
                    msgType = msgType.nul,
                    nodeType = nodeType.judge,
                    Vars = new List<string>()
                });
                defines.Add(new defineItem
                {
                    ParentId = "21",
                    Id = "211",
                    alias = "确定输入男",
                    beforTodo = "",
                    afterTodo = "",
                    KeyRule = "1",
                    msgTpl = "good boy,确定输入，请按1，重新输入，请按0。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });

                defines.Add(new defineItem
                {
                    ParentId = "211",
                    Id = "2110",
                    alias = "取消确定输入男",
                    beforTodo = "",
                    afterTodo = "changeCurrentTo:2",// 跳回2，提示输入性别
                    KeyRule = "0",
                    msgTpl = "",
                    msgType = msgType.nul,
                    nodeType = nodeType.judge,
                    Vars = new List<string>()
                });
                defines.Add(new defineItem
                {
                    ParentId = "211",
                    Id = "21101",
                    alias = "二次确定输入男",
                    beforTodo = "",
                    afterTodo = "changeCurrentTo:4",
                    KeyRule = "1",
                    msgTpl = "good boy,我们就差最后一步了，请输入女神的邮箱。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });



                defines.Add(new defineItem
                {
                    ParentId = "2",
                    Id = "20",
                    alias = "确定输入女",
                    beforTodo = "",
                    afterTodo = "changeCurrentTo:201",    //切换到item2，性别输入
                    KeyRule = "0",
                    msgTpl = "good girl,确定输入，请按1，重新输入，请按0。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });





                // 女神邮箱  4

                defines.Add(new defineItem
                {
                    ParentId = "0",
                    Id = "4",
                    alias = "仅提示输入女神邮箱",
                    beforTodo = "",
                    afterTodo = "changeCurrentTo:41",
                    KeyRule = "Availabled",
                    msgTpl = "亲,请输入女神邮箱",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });

                defines.Add(new defineItem
                {
                    ParentId = "0",
                    Id = "41",
                    alias = "判断女神邮箱输入是否有效",
                    beforTodo = "call_method:validGoddessEmail",
                    afterTodo = "goto_child_item",// 直接跳到下级，匹配  input rule，返回消息。只有这一种操作，不需要定义？再继续调用内部的方法呢？
                    KeyRule = "",
                    msgTpl = "",
                    msgType = msgType.nul,
                    nodeType = nodeType.judge,
                    Vars = new List<string>()
                });

                defines.Add(new defineItem
                {
                    ParentId = "41",
                    Id = "410",
                    alias = "女神邮箱没有找到",
                    beforTodo = "",
                    afterTodo = "changeCurrentTo:41",
                    KeyRule = "false",      // validGoddessEmail return false
                    msgTpl = "good boy,没有找到女神，你是不是输错了呀，重新输入女神邮箱。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });

                defines.Add(new defineItem
                {
                    ParentId = "41",
                    Id = "411",
                    alias = "确定输入女神邮箱",
                    beforTodo = "",
                    afterTodo = "call_method:SetGoddessEmail1",    // 只是将变量先存起来，便于下次可以找回来
                    KeyRule = "true",      // validGoddessEmail return true
                    msgTpl = "good boy,确定输入，请按1，重新输入，请按0。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });

                defines.Add(new defineItem
                {
                    ParentId = "411",
                    Id = "4110",
                    alias = "取消输入女神邮箱",
                    beforTodo = "",
                    afterTodo = "changeCurrentTo:4",    // 
                    KeyRule = "0",      // 取消输入，则回到 4 节点。。提示重新输入女神邮箱 ，使用4的输出。
                    msgTpl = "",
                    msgType = msgType.nul,
                    nodeType = nodeType.judge,
                    Vars = new List<string>()
                });
                defines.Add(new defineItem
                {
                    ParentId = "411",
                    Id = "4111",
                    alias = "确定了女神邮箱",
                    beforTodo = "",
                    afterTodo = "call_method:SetGoddessEmail2",    // 给她发送女神邀请消息
                    KeyRule = "1",
                    msgTpl = "good boy,我们已经将消息转告给了女神，请等待女神的答复，若想快点，可以给她打个电话哦，只要她同意了，我就马上带你去找她。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });

                // 查看女神是否已经同意。  《菜单》  49

                defines.Add(new defineItem
                {
                    ParentId = "0",
                    Id = "49",
                    alias = "查看女神是否已经同意",
                    beforTodo = "call_method:FindGoddessEmail",  // 
                    afterTodo = "changeCurrentTo:49",    // 
                    KeyRule = "",
                    msgTpl = "",
                    msgType = msgType.nul,
                    nodeType = nodeType.judge,
                    Vars = new List<string>()
                });
                defines.Add(new defineItem
                {
                    ParentId = "49",
                    Id = "491",
                    alias = "女神已经同意",
                    beforTodo = "",
                    afterTodo = "call_method:SetGoddessEnd",    // 设置sheId
                    KeyRule = "Agreed",
                    msgTpl = "good boy,女神已经同意了。我们这就过去看她吧。。房间已经为您打开，剩下的，你懂的。。。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });
                defines.Add(new defineItem
                {
                    ParentId = "49",
                    Id = "490",
                    alias = "女神未处理",
                    beforTodo = "",
                    afterTodo = "",    // 给她发送女神邀请消息
                    KeyRule = "Untreated",
                    msgTpl = "good boy,女神未处理。打个电话给他，问候一下什么情况吧。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });
                defines.Add(new defineItem
                {
                    ParentId = "49",
                    Id = "492",
                    alias = "女神驳回",
                    beforTodo = "",
                    afterTodo = "changeCurrentTo:41",    // 给她发送女神邀请消息
                    KeyRule = "Rejected",
                    msgTpl = "good boy,女神已驳回了您的请求。重新输入邮箱，重新发起申请吧。。",
                    msgType = msgType.text,
                    nodeType = nodeType.msg,
                    Vars = new List<string>()
                });

                #endregion
            }


        }


















        //int current = 0;

        public void regiester(string openId)
        {
            // 取得用户之前是否已经注册过。。
            // 订阅量是否大于1

            //current = 1;
        }

        public void code(string openId, string code)
        {
            // 检测邀请码是否有效

            // 失败次数》3
            // 明天再来吧

            // 无效
            // 已用过
            // 暂时失效（没有持续三天）
            // 还没出来（找不到该记录的）
            // 失败次数+1

            // 有效
            // 
        }

    }
}
