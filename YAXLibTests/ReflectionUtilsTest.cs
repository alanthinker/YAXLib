﻿// Copyright (C) Sina Iravanian, Julian Verdurmen, axuno gGmbH and other contributors.
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using YAXLib;
using YAXLibTests.SampleClasses;

namespace YAXLibTests;

[TestFixture]
public class ReflectionUtilsTest
{
    [Test]
    public void IsArrayTest()
    {
        Assert.That(ReflectionUtils.IsArray(typeof(int[])), Is.True);
        Assert.That(ReflectionUtils.IsArray(typeof(int[,])), Is.True);
        Assert.That(ReflectionUtils.IsArray(typeof(Array)), Is.True);
        Assert.That(ReflectionUtils.IsArray(typeof(List<int>)), Is.False);
        Assert.That(ReflectionUtils.IsArray(typeof(List<>)), Is.False);
        Assert.That(ReflectionUtils.IsArray(typeof(Dictionary<,>)), Is.False);
        Assert.That(ReflectionUtils.IsArray(typeof(Dictionary<int, string>)), Is.False);
        Assert.That(ReflectionUtils.IsArray(typeof(string)), Is.False);
    }

    [Test]
    public void IsCollectionTypeTest()
    {
        Assert.That(ReflectionUtils.IsCollectionType(typeof(int[])), Is.True);
        Assert.That(ReflectionUtils.IsCollectionType(typeof(Array)), Is.True);
        Assert.That(ReflectionUtils.IsCollectionType(typeof(List<int>)), Is.True);
        Assert.That(ReflectionUtils.IsCollectionType(typeof(List<>)), Is.True);
        Assert.That(ReflectionUtils.IsCollectionType(typeof(Dictionary<,>)), Is.True);
        Assert.That(ReflectionUtils.IsCollectionType(typeof(Dictionary<int, string>)), Is.True);
        Assert.That(ReflectionUtils.IsCollectionType(typeof(IEnumerable)), Is.True);
        Assert.That(ReflectionUtils.IsCollectionType(typeof(IEnumerable<>)), Is.True);
        Assert.That(ReflectionUtils.IsCollectionType(typeof(IEnumerable<int>)), Is.True);
        Assert.That(ReflectionUtils.IsCollectionType(typeof(string)), Is.False);
    }

    [Test]
    public void GetCollectionItemTypeTest()
    {
        Assert.That(ReflectionUtils.GetCollectionItemType(typeof(IEnumerable<int>)) == typeof(int), Is.True);
        Assert.That(ReflectionUtils.GetCollectionItemType(typeof(double[])) == typeof(double), Is.True);
        Assert.That(ReflectionUtils.GetCollectionItemType(typeof(float[][])) == typeof(float[]), Is.True);
        Assert.That(ReflectionUtils.GetCollectionItemType(typeof(string[,])) == typeof(string), Is.True);
        Assert.That(ReflectionUtils.GetCollectionItemType(typeof(List<char>)) == typeof(char), Is.True);
        Assert.That(
            ReflectionUtils.GetCollectionItemType(typeof(Dictionary<int, char>)) == typeof(KeyValuePair<int, char>),
            Is.True);
        Assert.That(
            ReflectionUtils.GetCollectionItemType(typeof(Dictionary<Dictionary<int, double>, char>)) ==
            typeof(KeyValuePair<Dictionary<int, double>, char>), Is.True);

        //Assert.That(ReflectionUtils.GetCollectionItemType(typeof(IEnumerable<>)) == typeof(object), Is.True);
    }

    [Test]
    public void IsTypeEqualOrInheritedFromTypeTest()
    {
        Assert.That(ReflectionUtils.IsTypeEqualOrInheritedFromType(typeof(int), typeof(object)), Is.True);
        Assert.That(ReflectionUtils.IsTypeEqualOrInheritedFromType(typeof(string), typeof(object)), Is.True);
        Assert.That(ReflectionUtils.IsTypeEqualOrInheritedFromType(typeof(Array), typeof(IEnumerable)), Is.True);
        Assert.That(
            ReflectionUtils.IsTypeEqualOrInheritedFromType(typeof(Dictionary<string, int>), typeof(Dictionary<,>)),
            Is.True);
        Assert.That(
            ReflectionUtils.IsTypeEqualOrInheritedFromType(typeof(Dictionary<string, int>), typeof(ICollection)),
            Is.True);
        Assert.That(
            ReflectionUtils.IsTypeEqualOrInheritedFromType(typeof(Dictionary<string, int>), typeof(IDictionary)),
            Is.True);
        Assert.That(
            ReflectionUtils.IsTypeEqualOrInheritedFromType(typeof(Dictionary<string, int>), typeof(IDictionary<,>)),
            Is.True);
        Assert.That(
            ReflectionUtils.IsTypeEqualOrInheritedFromType(typeof(Dictionary<string, int>),
                typeof(IDictionary<string, int>)), Is.True);
        Assert.That(
            ReflectionUtils.IsTypeEqualOrInheritedFromType(typeof(Dictionary<string, int>),
                typeof(IDictionary<int, string>)), Is.False);
        Assert.That(
            ReflectionUtils.IsTypeEqualOrInheritedFromType(typeof(Dictionary<string, int[]>),
                typeof(IDictionary<int, Array>)), Is.False);
        Assert.That(ReflectionUtils.IsTypeEqualOrInheritedFromType(typeof(ICollection), typeof(IEnumerable)),
            Is.True);
    }

    [Test]
    public void EqualsOrIsNullableOfTest()
    {
        Assert.That(typeof(int).EqualsOrIsNullableOf(typeof(int)), Is.True);
        Assert.That(typeof(int?).EqualsOrIsNullableOf(typeof(int)), Is.True);
        Assert.That(typeof(int).EqualsOrIsNullableOf(typeof(int?)), Is.True);
        Assert.That(typeof(double).EqualsOrIsNullableOf(typeof(double?)), Is.True);
        Assert.That(typeof(double?).EqualsOrIsNullableOf(typeof(Nullable<>)), Is.False);
        Assert.That(typeof(double?).EqualsOrIsNullableOf(typeof(double)), Is.True);
        Assert.That(typeof(char?).EqualsOrIsNullableOf(typeof(char?)), Is.True);
        Assert.That(typeof(char?).EqualsOrIsNullableOf(typeof(char?)), Is.True);
        Assert.That(typeof(int[]).EqualsOrIsNullableOf(typeof(Array)), Is.False);
    }

#if NETFRAMEWORK
    [TestCase("mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")] // NETFRAMEWORK2.x
    [TestCase("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")] // NETFRAMEWORK4.x
#elif NETCOREAPP3_1
        [TestCase("System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e")] // NETSTANDARD
#else
        [TestCase("System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e")] // NETSTANDARD
        [TestCase("System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e")] // NET5.0
#endif
    public void GetTypeByNameTest(string coreLibName)
    {
        var type1 = ReflectionUtils.GetTypeByName(
            $"System.Collections.Generic.List`1[[System.Int32, {coreLibName}]]");
        var type2 = ReflectionUtils.GetTypeByName("System.Collections.Generic.List`1[[System.Int32]]");

        Assert.That(type1, Is.Not.Null);
        Assert.That(type2, Is.Not.Null);
        Assert.That(type2, Is.EqualTo(type1));
    }

    [Test]
    public void GetSetFieldValues()
    {
        // The test includes private fields from base types

        var subClass = new ClassFlaggedToIncludePrivateBaseTypeFields();

        // Initial values
        var subClassField = ReflectionUtils.GetFieldValue(subClass, "_privateFieldFromLevel0");
        var baseClassField1 = ReflectionUtils.GetFieldValue(subClass, "_privateFieldFromBaseLevel1");
        var baseClassField2 = ReflectionUtils.GetFieldValue(subClass, "_privateFieldFromBaseLevel2");

        // Change initial values
        ReflectionUtils.SetFieldValue(subClass, "_privateFieldFromLevel0", 3);
        ReflectionUtils.SetFieldValue(subClass, "_privateFieldFromBaseLevel1", 13);
        ReflectionUtils.SetFieldValue(subClass, "_privateFieldFromBaseLevel2", 23);

        // Get changed values
        var subClassFieldAfterSet = ReflectionUtils.GetFieldValue(subClass, "_privateFieldFromLevel0");
        var baseClassField1AfterSet = ReflectionUtils.GetFieldValue(subClass, "_privateFieldFromBaseLevel1");
        var baseClassField2AfterSet = ReflectionUtils.GetFieldValue(subClass, "_privateFieldFromBaseLevel2");

        // Initial values
        Assert.That(subClassField, Is.EqualTo(2));
        Assert.That(baseClassField1, Is.EqualTo(12));
        Assert.That(baseClassField2, Is.EqualTo(22));
        Assert.That(
            // private base field not found
            code: () => { ReflectionUtils.GetFieldValue(subClass, "_privateFieldFromBaseLevel1", false); },
            Throws.Exception);

        // Changed values
        Assert.That(subClassFieldAfterSet, Is.EqualTo(3));
        Assert.That(baseClassField1AfterSet, Is.EqualTo(13));
        Assert.That(baseClassField2AfterSet, Is.EqualTo(23));
    }

    [Test]
    public void GetSetPropertyValues()
    {
        // The test includes private properties from base types

        var subClass = new ClassFlaggedToIncludePrivateBaseTypeFields();

        // Initial values
        var subClassProperty = ReflectionUtils.GetPropertyValue(subClass, "PublicPropertyFromLevel0");
        var baseClassProperty1 = ReflectionUtils.GetPropertyValue(subClass, "PrivatePropertyFromBaseLevel1");
        var baseClassProperty2 = ReflectionUtils.GetPropertyValue(subClass, "PrivatePropertyFromBaseLevel2");

        // Change initial values
        ReflectionUtils.SetPropertyValue(subClass, "PublicPropertyFromLevel0", 111);
        ReflectionUtils.SetPropertyValue(subClass, "PrivatePropertyFromBaseLevel1", 113);
        ReflectionUtils.SetPropertyValue(subClass, "PrivatePropertyFromBaseLevel2", 123);

        // Get changed values
        var subClassPropertyAfterSet = ReflectionUtils.GetPropertyValue(subClass, "PublicPropertyFromLevel0");
        var baseClassProperty1AfterSet =
            ReflectionUtils.GetPropertyValue(subClass, "PrivatePropertyFromBaseLevel1");
        var baseClassProperty2AfterSet =
            ReflectionUtils.GetPropertyValue(subClass, "PrivatePropertyFromBaseLevel2");

        // Initial values
        Assert.That(subClassProperty, Is.EqualTo(1));
        Assert.That(baseClassProperty1, Is.EqualTo(13));
        Assert.That(baseClassProperty2, Is.EqualTo(23));
        Assert.That(
            // private base property not found
            code: () => { ReflectionUtils.GetPropertyValue(subClass, "PrivatePropertyFromBaseLevel1", false); },
            Throws.Exception);

        // Changed values
        Assert.That(subClassPropertyAfterSet, Is.EqualTo(111));
        Assert.That(baseClassProperty1AfterSet, Is.EqualTo(113));
        Assert.That(baseClassProperty2AfterSet, Is.EqualTo(123));
    }

    [Test]
    public void GetDefaultValueTest()
    {
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(string)), null));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(bool)), default(bool)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(char)), default(char)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(sbyte)), default(sbyte)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(byte)), default(byte)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(short)), default(short)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(ushort)), default(ushort)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(int)), default(int)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(uint)), default(uint)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(long)), default(long)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(ulong)), default(ulong)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(float)), default(float)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(double)), default(double)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(decimal)), default(decimal)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(DateTime)), default(DateTime)));
        Assert.That(Equals(ReflectionUtils.GetDefaultValue(typeof(DBNull)), DBNull.Value));
    }
}
