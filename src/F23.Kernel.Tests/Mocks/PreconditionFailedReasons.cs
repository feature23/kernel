using F23.Kernel.Results;

namespace F23.Kernel.Tests.Mocks;

public static class PreconditionFailedReasons
{
    public static TheoryData<PreconditionFailedReason> All()
    {
        var data = new TheoryData<PreconditionFailedReason>();

        foreach (var reason in Enum.GetValues<PreconditionFailedReason>())
        {
            data.Add(reason);
        }

        return data;
    }
}
