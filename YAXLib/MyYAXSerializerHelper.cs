// Copyright (C) Sina Iravanian, Julian Verdurmen, axuno gGmbH and other contributors.
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YAXLib;
using YAXLib.Enums;
using YAXLib.Options;

namespace YAXLib;
public static class MyYAXSerializerHelper
{
    public static SerializerOptions DefaultSerializerOptions = new SerializerOptions
    {
        // 如果老版本的xml文本缺失了部分属性, 不要 Throw Error, 继续反序列化.
        ExceptionBehavior = YAXExceptionTypes.Warning, //将异常设置为警告,
        ExceptionHandlingPolicies = YAXExceptionHandlingPolicies.ThrowErrorsOnly, // YAXExceptionTypes 为 Error 级别时, 才 Throw, (不 Throw YAXExceptionTypes.Warning)
        UseOriginalPropertyValue = true,
        // 注意, 不要设置 DontSerializeNullObjects 和 DoNotSerializeDefaultValues   
        //      因为, 如果部分属性没有被序列化, 且反序列化时候提供的基础对象包含了一个非默认值 (比如 int 的 3, bool 的 true), 那么当序列化 0, false的时候, 就会被错误的反序列化为 3, true.
        SerializationOptions =
                YAXSerializationOptions.SerializeNullObjects
                | YAXSerializationOptions.DontSerializePropertiesWithNoSetter // 如果一个属性只有 Get, 没有Set, 则不序列化. (这样的字段通常只是显示数据, 而不是存储数据.)

        // ps:
        // YAXSerializationOptions.DontSerializeNullObjects // Null 值不序列化, Xml 中缺失的字段和属性也不导致反序列化失败. 
        // YAXSerializationOptions.DoNotSerializeDefaultValues  // 如果字段或者属性值是该类型的默认值 (比如 null, 0, 枚举的默认值 等), 则不序列化,
        //      (注意, DoNotSerializeDefaultValues 指的 DefaultValue, 是类型的 DefaultValue, 比如 int 的 DefaultValue 为 0, bool 的 DefaultValue 为 false, 并不是属性定义的 EnableSound {get;set;} = false 的 DefaultValue)

    };

    public static void LogError(YAXParsingErrors errors, string exMsg)
    {
        //这里只记录错误信息, 不抛出异常. 因为老配置文件如果缺失了部分属性, 也要能正常启动程序.
        if (errors.Count > 0)
        {
            // YAXParsingErrors 自带的 toString 方法 使用的pool导致了bug, 这里自己实现输出.
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(exMsg);
            for (int i = 0; i < errors.Count; i++)
            {
                var error = errors[i];
                sb.AppendLine(error.Value + ": " + error.Key.Message);
            }
            //_log.Error(sb.ToString());
            Console.Error.WriteLine(sb.ToString());
        }
    }
}
