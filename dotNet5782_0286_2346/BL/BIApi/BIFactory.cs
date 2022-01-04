
namespace BlApi
{
    public static class BlFactory
    {
        public static BlApi.IBL GetBl() => BL.BL.Instance;
    }
}