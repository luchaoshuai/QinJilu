﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinJilu.Core
{
    public enum Gender : byte
    {
        /// <summary>
        /// 男
        /// </summary>
        Male = 0,
        /// <summary>
        /// 女
        /// </summary>
        Woman = 1
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

    public enum Options : short
    {
        NULL = 0,

        a = 1,
        b = 2,
        c = 3,
        d = 4,

        a_b = 12,// 
        a_b_c = 123,
        a_b_d = 124,

        a_b_c_d = 1234,


        a_c = 13,// 
        a_c_d = 134,

        a_d = 14,// 

        //  
        b_c = 23,
        b_c_d = 234,

        b_d = 24,

        //  
        c_d = 34
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
