// Copyright (C) Sina Iravanian, Julian Verdurmen, axuno gGmbH and other contributors.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAXLibTests.AlanTest;

class Student
{
    public int Id { get; set; }
    public string? Name { get; set; } = "Jim";
    public Teacher? MathTeacher { get; set; }
    public Address? Address { get; set; }
}

class Teacher
{
    public int Id { get; set; }
    public string? Name { get; set; } = "Lucy";
}

class Address
{
    public int Id { get; set; } = 1;
    public string? Name { get; set; }
    public AddressType AddressType { get; set; } = AddressType.Type2;
    public string? NoSetter
    {
        get
        {
            return "NoSetter";
        }
    }
}

enum AddressType
{
    Type0, Type1, Type2, Type3, Type4, Type5,
}
