namespace Infrastructure.UnitTests.Common;

public abstract class AbstractDatabaseTest : IDisposable
{
    protected AppDbContext DbContext;

    protected AbstractDatabaseTest()
    {
        DbContext = Task.Run(
            async () => await DatabaseTestTool.GetTestDbContext()
        ).Result;
    }

    public async void Dispose()
    {
        await DatabaseTestTool.Cleanup(DbContext);
    }
}