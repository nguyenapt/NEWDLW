namespace Dlw.EpiBase.Content.Convertors
{
    public interface IRegionCodeConvertor
    {
        string ConvertFromThreeToTwo(string code);

        string ConvertFromTwoToThree(string code);
    }
}