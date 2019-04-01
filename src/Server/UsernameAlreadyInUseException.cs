﻿using System;

public class UsernameAlreadyInUseException : Exception
{
    public UsernameAlreadyInUseException()
    {
    }

    public UsernameAlreadyInUseException(string message)
        : base(message)
    {
    }

    public UsernameAlreadyInUseException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
