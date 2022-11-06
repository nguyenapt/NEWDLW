namespace Dlw.EpiBase.Content
{
    public interface IAppSettings
    {
        bool IsLocal { get; }

        bool IsProduction { get; }
    }
}