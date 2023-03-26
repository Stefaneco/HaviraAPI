using System;
namespace HaviraApi.Exceptions;


public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}

