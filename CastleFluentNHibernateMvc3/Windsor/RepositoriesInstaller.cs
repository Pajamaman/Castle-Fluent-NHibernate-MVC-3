using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CastleFluentNHibernateMvc3.Models;
using CastleFluentNHibernateMvc3.Repositories;

namespace CastleFluentNHibernateMvc3.Windsor
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install( IWindsorContainer container, IConfigurationStore store )
        {
            container.Register( Classes.FromThisAssembly()
                                .Where( Component.IsInSameNamespaceAs<Repository<Store>>() )
                                .WithService.DefaultInterfaces()
                                .LifestyleTransient() );
        }
    }
}