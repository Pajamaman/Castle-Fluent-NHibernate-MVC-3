using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace CastleFluentNHibernateMvc3.Windsor
{
    public class PersistenceFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.Register(
                Component.For<ISessionFactory>()
                    .UsingFactoryMethod( _ => CreateSessionFactory() ),
                Component.For<ISession>()
                    .UsingFactoryMethod( k => k.Resolve<ISessionFactory>().OpenSession() )
                    .LifestylePerWebRequest() );
        }

        // Returns our session factory
        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database( CreateDbConfig )
                .Mappings( m => m
                    .AutoMappings.Add( CreateMappings() ) )
                .ExposeConfiguration( UpdateSchema )
                .CurrentSessionContext<WebSessionContext>()
                .BuildSessionFactory();
        }

        // Returns our database configuration
        private static MsSqlConfiguration CreateDbConfig()
        {
            return MsSqlConfiguration
                .MsSql2008
                .ConnectionString( c => c
                    .FromConnectionStringWithKey( "testConn" ) );
        }
        
        // Returns our mappings
        private static AutoPersistenceModel CreateMappings()
        {
            return AutoMap
                .Assembly( System.Reflection.Assembly.GetCallingAssembly() )
                .Where( t => t
                    .Namespace == "CastleFluentNHibernateMvc3.Models" )
                .Conventions.Setup( c => c
                    .Add( DefaultCascade.SaveUpdate() ) );
        }
        
        // Updates the database schema if there are any changes to the model,
        // or drops and creates it if it doesn't exist
        private static void UpdateSchema( Configuration cfg )
        {
            new SchemaUpdate( cfg )
                .Execute( false, true );
        }
    }
}