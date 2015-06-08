namespace PersonalSecurity.DataAccess
{
    using PersonalSecurity.DataAccess.Domain;

    public interface ICloudRepository
    {
        void Save(Cloud cloud);

        Cloud[] GetAll();

        Cloud GetByType(CloudType cloudType);
    }
}
