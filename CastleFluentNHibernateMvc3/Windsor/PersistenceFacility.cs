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

        // Returns our NHibernate session factory
        private static ISessionFactory CreateSessionFactory()
        {
            var mappings = CreateMappings();

            return Fluently
                .Configure()
                .Database( MsSqlConfiguration.MsSql2008
                    .ConnectionString( c => c
                        .FromConnectionStringWithKey( "testConn" ) ) )
                .Mappings( m => m
                    .AutoMappings.Add( mappings ) )
                .ExposeConfiguration( c =>
                    {
                        BuildSchema( c );
                        c.CurrentSessionContext<WebSessionContext>();
                    } )
                .BuildSessionFactory();
        }

        // Returns our NHibernate auto mapper
        private static AutoPersistenceModel CreateMappings()
        {
            return AutoMap
                .Assembly( System.Reflection.Assembly.GetCallingAssembly() )
                .Where( t => t.Namespace == "CastleFluentNHibernateMvc3.Models" )
                .Conventions.Setup( c =>
                    {
                        c.Add( DefaultCascade.SaveUpdate() );
                    } );
        }

        // Drops and creates the database schema
        // private static void BuildSchema( Configuration cfg )
        // {
        //     new SchemaExport( cfg )
        //         .Create( false, true );
        // }

        // Updates the database schema if there are any changes to the model
        private static void BuildSchema( Configuration cfg )
        {
            new SchemaUpdate( cfg );
        }
    }
}