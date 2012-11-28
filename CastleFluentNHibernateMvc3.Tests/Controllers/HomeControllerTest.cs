using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

using CastleFluentNHibernateMvc3.Controllers;
using CastleFluentNHibernateMvc3.Models;
using CastleFluentNHibernateMvc3.Repositories;
using Moq;
using NUnit.Framework;

namespace CastleFluentNHibernateMvc3.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        private Mock<IRepository<Store>> storeRepositoryMock;

        public HomeControllerTest()
        {
            storeRepositoryMock = new Mock<IRepository<Store>>();
        }

        public HomeController GetHomeController()
        {
            var controller = new HomeController( storeRepositoryMock.Object );

            return controller;
        }

        [Test]
        public void Index()
        {
            // Arrange
            var controller = GetHomeController();

            var stores = new List<Store>();

            storeRepositoryMock.Setup( s => s.GetAll() )
                .Returns( stores.AsQueryable() )
                .Verifiable();

            // Act
            var result = controller.Index();

            // Assert
            storeRepositoryMock.Verify();

            Assert.IsInstanceOf<ViewResult>( result );
        }

        [Test]
        public void Test()
        {
            // Arrange
            var controller = GetHomeController();

            var store = new Store
            {
                Name = "Bargin Basin"
            };
            var stores = new List<Store>
            {
                store
            };
            
            storeRepositoryMock.Setup( s => s.BeginTransaction() )
                .Verifiable();

            storeRepositoryMock.Setup( s => s.Get( It.IsAny<Expression<Func<Store, bool>>>() ) )
                .Returns( stores.AsQueryable() )
                .Verifiable();
            
            storeRepositoryMock.Setup( s => s.Commit() )
                .Verifiable();

            // Act
            var result = controller.Test();

            // Assert
            storeRepositoryMock.Verify();

            Assert.IsInstanceOf<RedirectToRouteResult>( result );
            Assert.AreEqual( store.Name, "Bargain Basin" );
        }

        [Test]
        public void Test_NotFound()
        {
            // Arrange
            var controller = GetHomeController();

            var stores = new List<Store>();
            
            storeRepositoryMock.Setup( s => s.BeginTransaction() )
                .Verifiable();

            storeRepositoryMock.Setup( s => s.Get( It.IsAny<Expression<Func<Store, bool>>>() ) )
                .Returns( stores.AsQueryable() )
                .Verifiable();
            
            storeRepositoryMock.Setup( s => s.Rollback() )
                .Verifiable();

            // Act
            var result = controller.Test();

            // Assert
            storeRepositoryMock.Verify();

            Assert.IsInstanceOf<RedirectToRouteResult>( result );
        }

        [Test]
        public void Seed()
        {
            // Arrange
            var controller = GetHomeController();

            var stores = new List<Store>
            {
                new Store
                {
                    Name = "Foo Foundry"
                },
                new Store
                {
                    Name = "Bar Bohemia"
                }
            };
            
            storeRepositoryMock.Setup( s => s.BeginTransaction() )
                .Verifiable();

            storeRepositoryMock.Setup( s => s.SaveOrUpdateAll( It.IsAny<Store>(), It.IsAny<Store>() ) )
                .Returns( stores.AsEnumerable() )
                .Verifiable();
            
            storeRepositoryMock.Setup( s => s.Commit() )
                .Verifiable();

            // Act
            var result = controller.Seed();

            // Assert
            storeRepositoryMock.Verify();

            Assert.IsInstanceOf<RedirectToRouteResult>( result );
        }
    }
}
