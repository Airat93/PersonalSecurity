namespace PersonalSecurity.Wpf
{
    using System.Windows;
    using Microsoft.Practices.Unity;
    using PersonalSecurity.DataAccess;
    using PersonalSecurity.Files;
    using PersonalSecurity.PII.Builder;
    using PersonalSecurity.PII.Deserializing;
    using PersonalSecurity.Wpf.Annotations;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly string ConnectionString =
            "Data Source=.; Integrated Security=True; Database=PersonalSecurityLocal;";

        [NotNull]
        public static readonly IUnityContainer Container = new UnityContainer();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Container.RegisterInstance(typeof(ICloudRepository), new CloudRepository(ConnectionString));
            Container.RegisterInstance(typeof(IFileInfoRepository), new FileInfoRepository(ConnectionString));
            Container.RegisterInstance(
                typeof(IFileManager),
                new FileManager(Container.Resolve<IFileInfoRepository>(), "DATA_ENCRYPTED", "DATA"));

            Container.RegisterType<IPiiDeserializer, PiiDeserializer>();
            Container.RegisterType<IPiiBuilder, PiiBuilder>();
        }
    }
}
