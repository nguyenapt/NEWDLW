namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services
{
    public interface ICipher
    {
        string Encipher(string value, int shift);
        string Decipher(string value, int shift);
    }
}
