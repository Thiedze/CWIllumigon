using CWIllumigon.Service;

namespace CWIllumigon;

public static class Program
{
    private static void Main(string[] args)
    {
        var configurationService = new ConfigurationService(args[0]);
        configurationService.Start(1);
    }
}