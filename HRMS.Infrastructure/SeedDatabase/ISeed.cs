namespace HRMS.Infrastructure.SeedDatabase
{
    public interface ISeed
    {
        Task InitialiseAsync();
        Task SeedAsync();
        Task TrySeedAsync();
    }
}