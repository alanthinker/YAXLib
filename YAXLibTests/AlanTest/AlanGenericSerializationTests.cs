// Copyright (C) Sina Iravanian, Julian Verdurmen, axuno gGmbH and other contributors.
// Licensed under the MIT license.

using System;
using NUnit.Framework;
using NUnit.Framework.Internal;
using YAXLib;
using YAXLib.Options;
using YAXLib.Pooling.YAXLibPools;
using YAXLibTests.SampleClasses;
using YAXLibTests.TestHelpers;

namespace YAXLibTests.AlanTest;

[TestFixture]
public class AlanGenericSerializationTests
{
    [OneTimeSetUp]
    public void TestFixtureSetup()
    {
        // Clear the pool for tests in here
        SerializerPool.Instance.Clear();
    }

    [OneTimeTearDown]
    public void TestFixtureFinalize()
    {
        Console.WriteLine(
            $"{nameof(SerializerPool.Instance.Pool.CountAll)}: {SerializerPool.Instance.Pool.CountAll}");
        Console.WriteLine(
            $"{nameof(SerializerPool.Instance.Pool.CountActive)}: {SerializerPool.Instance.Pool.CountActive}");
        Console.WriteLine(
            $"{nameof(SerializerPool.Instance.Pool.CountInactive)}: {SerializerPool.Instance.Pool.CountInactive}");
    }

    static Student GetTestStudent()
    {
        return new Student() { MathTeacher = new Teacher() };
    }

    static Student GetTestStudent2()
    {
        return new Student() { MathTeacher = new Teacher(), Address = new Address() };
    }

    static Student GetTestStudent3()
    {
        return new Student() { MathTeacher = new Teacher(), Address = new Address() { Name="addr" } };
    }

    [Test]
    public void GenericSerializationTest()
    {
        const string result =
            """
                <Student>
                  <Id>0</Id>
                  <Name>Jim</Name>
                  <MathTeacher>
                    <Id>0</Id>
                    <Name>Lucy</Name>
                  </MathTeacher>
                  <Address />
                </Student>
                """;
        var serializer = GetMyYAXSerializer();
        var got = serializer.Serialize(GetTestStudent());
        Assert.That(got, Is.EqualTo(result));
    }

    private static YAXSerializer<Student> GetMyYAXSerializer()
    {
        return new YAXSerializer<Student>(MyYAXSerializerHelper.DefaultSerializerOptions);
    }

    [Test]
    public void GenericSerializationTest2()
    {
        const string result =
            """
                <Student>
                  <Id>0</Id>
                  <Name>Jim</Name>
                  <MathTeacher>
                    <Id>0</Id>
                    <Name>Lucy</Name>
                  </MathTeacher>
                  <Address>
                    <Id>1</Id>
                    <Name />
                  </Address>
                </Student>
                """;
        var serializer = GetMyYAXSerializer();
        var got = serializer.Serialize(GetTestStudent2());
        Assert.That(got, Is.EqualTo(result));
    }

    [Test]
    public void GenericDeserializationTest()
    {
        const string xml =
            """
                <Student>
                  <Id>0</Id>
                  <Name>Jim</Name>
                  <MathTeacher>
                    <Id>0</Id>
                    <Name>Lucy</Name>
                  </MathTeacher>
                  <Address />
                </Student>
                """;
        var serializer = GetMyYAXSerializer();
        var got = serializer.Deserialize(xml);
        Assert.That(got, Is.Not.Null);
        var areEqual = MyTestHelper.AreAllPropertiesEqual(got, GetTestStudent());
        Assert.IsTrue(areEqual);
    }

    [Test]
    public void GenericDeserializationTest2()
    {
        const string xml =
            """
                <Student>
                  <MathTeacher>
                  </MathTeacher>
                  <Address>
                  </Address>
                </Student>
                """;
        var serializer = GetMyYAXSerializer();
        var got = serializer.Deserialize(xml);
        Assert.That(got, Is.Not.Null);
        var areEqual = MyTestHelper.AreAllPropertiesEqual(got, GetTestStudent2());
        Assert.IsTrue(areEqual);
    }

    [Test]
    public void GenericDeserializationTest3()
    {
        const string xml =
            """
                <Student>
                  <MathTeacher>
                  </MathTeacher>
                  <Address>
                    <Name>addr</Name>
                  </Address>
                </Student>
                """;
        var serializer = GetMyYAXSerializer();
        var got = serializer.Deserialize(xml);
        Assert.That(got, Is.Not.Null);
        var areEqual = MyTestHelper.AreAllPropertiesEqual(got, GetTestStudent3());
        Assert.IsTrue(areEqual);
    }

    [Test]
    public void DefaultPropertyValueTest()
    {
        const string xml =
            """
                <Student>
                  <Id>0</Id>
                  <MathTeacher>
                    <Id>0</Id>
                  </MathTeacher>
                </Student>
                """;
        var serializer = new YAXSerializer<Student>(MyYAXSerializerHelper.DefaultSerializerOptions);
        var got = serializer.Deserialize(xml);
        Assert.That(got, Is.Not.Null);
        Assert.IsTrue(got.Name == "Jim");
        Assert.IsTrue(got?.MathTeacher?.Name == "Lucy");
    }
}
