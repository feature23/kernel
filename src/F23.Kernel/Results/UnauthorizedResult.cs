namespace F23.Kernel.Results;

public class UnauthorizedResult(string message) : Result(false)
{
    public override string Message => message;
}

public class UnauthorizedResult<T>(string message) : Result<T>(false)
{
    public override string Message => message;
}
