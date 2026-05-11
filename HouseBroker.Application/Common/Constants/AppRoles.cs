namespace HouseBroker.Application.Common.Constants
{
    public static class AppRoles
    {
        public const string HouseSeeker = "HouseSeeker";
        public const string Broker = "Broker";

        public static readonly string[] All =
        {
            HouseSeeker,
            Broker
        };
    }
}