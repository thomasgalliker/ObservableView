﻿[assembly: Preserve(AllMembers = true)]

[AttributeUsage(
    AttributeTargets.Assembly
    | AttributeTargets.Class
    | AttributeTargets.Struct
    | AttributeTargets.Enum
    | AttributeTargets.Constructor
    | AttributeTargets.Method
    | AttributeTargets.Property
    | AttributeTargets.Field
    | AttributeTargets.Event
    | AttributeTargets.Interface
    | AttributeTargets.Delegate,
    AllowMultiple = true)]
public sealed class PreserveAttribute : Attribute
{

    public bool AllMembers;
    public bool Conditional;

    public PreserveAttribute()
    {
    }
}