using System;

public class PlugAlreadyInUseException : Exception
{
    public PlugAlreadyInUseException()
    {
    }

    public PlugAlreadyInUseException(string message)
        : base(message)
    {
    }

    public PlugAlreadyInUseException(string message, Exception inner)
        : base(message, inner)
    {
    }
}