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
    public string? NoSetter
    {
        get
        {
            return "NoSetter";
        }
    }
}
