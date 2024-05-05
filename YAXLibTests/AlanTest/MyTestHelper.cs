// Copyright (C) Sina Iravanian, Julian Verdurmen, axuno gGmbH and other contributors.
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YAXLibTests.AlanTest;
internal class MyTestHelper
{
    //注意, 不能有循环引用, 且此方法仅应当用于测试用例, 因为并没有考虑很多特殊情况, 也不适合 AOT 之后的代码.
    public static bool AreAllPropertiesEqual(object obj1, object obj2)
    {
        if (obj1 == null && obj2 == null) return true;
        if (obj1 == null || obj2 == null) return false;

        Type type = obj1.GetType();
        if (type != obj2.GetType()) return false;

        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            object value1 = property.GetValue(obj1);
            object value2 = property.GetValue(obj2);

            bool areEqual;
            if (property.PropertyType == typeof(string))
            {
                areEqual = value1 as string == value2 as string;
            }
            else if (value1 is IEnumerable && value2 is IEnumerable) // 处理数组或集合类型
            {
                areEqual = CollectionsEqual((IEnumerable) value1, (IEnumerable) value2);
            }
            else if (value1 is IDictionary && value2 is IDictionary) // 处理字典类型
            {
                areEqual = DictionariesEqual((IDictionary) value1, (IDictionary) value2);
            }
            // 如果属性是其他引用类型，递归调用此方法
            else if (property.PropertyType?.IsClass == true)
            {
                areEqual = AreAllPropertiesEqual(value1, value2);
            }
            else
            {
                areEqual = Equals(value1, value2);
            }

            if (!areEqual)
                return false;
        }

        return true;
    }

    private static bool CollectionsEqual(IEnumerable collection1, IEnumerable collection2)
    {
        var enumerator1 = collection1.GetEnumerator();
        var enumerator2 = collection2.GetEnumerator();

        while (enumerator1.MoveNext() && enumerator2.MoveNext())
        {
            if (!AreAllPropertiesEqual(enumerator1.Current, enumerator2.Current))
                return false;
        }

        // 如果长度不同，或者都走完了还没返回false，说明不相等
        return !enumerator1.MoveNext() && !enumerator2.MoveNext();
    }

    private static bool DictionariesEqual(IDictionary dict1, IDictionary dict2)
    {
        if (dict1.Count != dict2.Count)
            return false;

        foreach (DictionaryEntry entry in dict1)
        {
            object key = entry.Key;
            if (!dict2.Contains(key) || !AreAllPropertiesEqual(entry.Value, dict2[key]))
                return false;
        }

        return true;
    }
}
