﻿namespace Udemy.Tests.Common;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PriorityAttribute(int priority) : Attribute
{
    public int Priority { get; } = priority;
}