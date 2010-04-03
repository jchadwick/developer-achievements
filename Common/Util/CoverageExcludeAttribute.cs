using System;

public class CoverageExcludeAttribute : Attribute
{
    private readonly string _reason;

    public CoverageExcludeAttribute(string reason)
    {
        _reason = reason;
    }
}
