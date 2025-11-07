namespace F23.Kernel.Tests.Mocks;

public class UnknownResult() : Result<TestResultContent>(false)
{
    public override string Message => "Unknown result";

    public override Result Map()
    {
        throw new NotImplementedException();
    }
}
