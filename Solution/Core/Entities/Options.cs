using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    public class DateTimeEx
    {
        public static readonly DateTime NullDate = new DateTime(1900, 1, 1);
        public static readonly DateTime BeginDate = new DateTime(2010, 1, 1);
    }


    public enum InvitecodeStatus:byte
    {
        /// <summary>
        /// 未出生,未分配到创建者
        /// </summary>
        Unborn,
        /// <summary>
        /// 可用，新分配。或着主人持续登录了
        /// </summary>
        Availabled,
        /// <summary>
        /// 不可用，已分配过，但是主人未持续登录，失效了
        /// </summary>
        Disabled,
        /// <summary>
        /// 已用，被别人拿去使用掉了。
        /// </summary>
        Used
    }








    // 用户上下文的作用域
    // 数据库中保存完整的结果，进行中（待二次确认的），在缓存中。
    public enum CurrentScope : byte
    {
        /// <summary>
        /// 空着的
        /// </summary>
        NULL = 0,

        //检票 邀请码
        CheckInvitecode,
        
        //生成获取船票
        GenerateInvitecode,

        //设置性别
        Gender,

        //最后一次
        Lasttime,

        //各周期设置： cycle=28±2; period=4±21; pms=3±21
        Cycle,

        //昵称
        Nickname,

        //绑定邮箱
        Bindingemail




    }

    public enum Gender : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknow = 0,
        /// <summary>
        /// 男
        /// </summary>
        Male = 1,
        /// <summary>
        /// 女
        /// </summary>
        Woman = 2
    }


    public enum FieldName : byte
    {
        Period = 1,
        Pain = 2,
        Sex = 3,
        Mood = 4,
        Fluid = 5,
        Pill = 6,
        Temperature = 7,
        Tags = 8
    }


    public enum Operation : byte
    {
        /// <summary>
        /// 无权限
        /// </summary>
        NULL = 0,
        /// <summary>
        /// 只读
        /// </summary>
        ReadOnly = 10,
        /// <summary>
        /// 读+编辑
        /// </summary>
        Editable = 20
    }

    /// <summary>
    /// 审核意见
    /// </summary>
    public enum Opinion : byte
    {
        /// <summary>
        /// 未处理
        /// </summary>
        Untreated = 0,
        /// <summary>
        /// 通过
        /// </summary>
        Agreed = 10,
        /// <summary>
        /// 驳回
        /// </summary>
        Rejected = 20,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 20
    }

    [Flags]
    public enum Options
    {
        nul = 0,

        a = 1,
        b = 2,
        c = 4,
        d = 8,

        a_b = a|b,// 
        a_b_c = a|b|c,//33333
        a_b_d = a|b|d,//33333

        a_b_c_d = a|b|c|d,


        a_c = a|c,// 
        a_c_d = a|c|d,//33333

        a_d = a|d,// 

        
        b_c = b|c,//  
        b_c_d = b|c|d,//33333

        b_d = b|d,//  

        c_d = c|d//
    }

}





//[System.ComponentModel.Description("会员等级")]
//public enum MemberLevel
//{
//    [System.ComponentModel.Description("金牌会员")]
//    gold = 1,
//    [System.ComponentModel.Description("银牌会员")]
//    silver = 2,
//    [System.ComponentModel.Description("铜牌会员")]
//    copper = 3
//}


//[Flags]
//enum MemberLevel
//{
//    [System.ComponentModel.Description("二进制表示为1----0001")]
//    gold = 0x1,
//    [System.ComponentModel.Description("二进制表示为4----0010")]
//    silver = 0x04,
//    [System.ComponentModel.Description("二进制表示为16----0100")]
//    copper = 0x10
//}


//public static class EnumHelper
//{
//    /// <summary>
//    /// 返回枚举项的描述信息。
//    /// </summary>
//    /// <param name="value">要获取描述信息的枚举项。</param>
//    /// <returns>枚举想的描述信息。</returns>
//    public static string GetDescription(this Enum value, bool isTop = false)
//    {
//        Type enumType = value.GetType();
//        System.ComponentModel.DescriptionAttribute attr = null;
//        if (isTop)
//        {
//            attr = (System.ComponentModel.DescriptionAttribute)Attribute.GetCustomAttribute(enumType, typeof(System.ComponentModel.DescriptionAttribute));
//        }
//        else
//        {
//            // 获取枚举常数名称。
//            string name = Enum.GetName(enumType, value);
//            if (name != null)
//            {
//                // 获取枚举字段。
//                System.Reflection.FieldInfo fieldInfo = enumType.GetField(name);
//                if (fieldInfo != null)
//                {
//                    // 获取描述的属性。
//                    attr = Attribute.GetCustomAttribute(fieldInfo, typeof(System.ComponentModel.DescriptionAttribute), false) as System.ComponentModel.DescriptionAttribute;
//                }
//            }
//        }

//        if (attr != null && !string.IsNullOrEmpty(attr.Description))
//            return attr.Description;
//        else
//            return string.Empty;

//    }
//}

