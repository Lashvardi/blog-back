namespace Blog.Exceptions;
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {

    }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {

    }
}

public class AlreadyExistException : Exception
{
    public AlreadyExistException(string message) : base(message)
    {

    }
}

public class NotImplementException : Exception
{
    public NotImplementException(string message) : base(message)
    {

    }
}

public class InternalServerErrorException : Exception
{
    public InternalServerErrorException(string message) : base(message)
    {

    }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {

    }
}
