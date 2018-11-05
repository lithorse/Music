
using Autofac;
using Music.DataAccess;
using Music.UI.Data;
using Music.UI.Data.Lookups;
using Music.UI.Data.Repositories;
using Music.UI.ViewModel;
using Prism.Events;

namespace Music.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MusicDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<SongDetailViewModel>().As<ISongDetailViewModel>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<SongRepository>().As<ISongRepository>();

            return builder.Build();
        }
    }
}
