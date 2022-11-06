namespace Dlw.EpiBase.Content.Infrastructure.Mapper
{
    public interface IMapper<in TFrom, out TTo>
    {
        TTo Map(TFrom from);
    }
}