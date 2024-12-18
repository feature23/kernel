namespace F23.Kernel.Results;

public class ValidationPassedResult() : ValidationResult(true)
{
    public override string Message => "Validation passed";
}

public class ValidationPassedResult<T>() : ValidationResult<T>(true)
{
    public override string Message => "Validation passed";
}
