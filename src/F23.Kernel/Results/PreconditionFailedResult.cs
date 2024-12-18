namespace F23.Kernel.Results;

public class PreconditionFailedResult(PreconditionFailedReason reason, string? message) : Result(false)
{
    public PreconditionFailedReason Reason { get; } = reason;

    public override string Message => message ?? Reason.ToMessage();
}

public class PreconditionFailedResult<T>(PreconditionFailedReason reason, string? message) : Result<T>(false)
{
    public PreconditionFailedReason Reason { get; } = reason;

    public override string Message => message ?? Reason.ToMessage();
}
