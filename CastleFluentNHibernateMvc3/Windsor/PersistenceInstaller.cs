using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CastleFluentNHibernateMvc3.Windsor
{
    public class PersistenceInstaller : IWindsorInstaller
    {
        public void Install( IWindsorContainer container, IConfigurationStore store )
        {
            container.AddFacility<PersistenceFacility>();
        }
    }
}