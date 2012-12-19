using System.Collections.Generic;
using System.Linq;
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

        [SetUp]
        public void Init()
        {
            storeRepositoryMock = new Mock<IRepository<Store>>();
        }

        [Test]
        public void Index()
        {
            // Arrange
            var controller = GetHomeController();

            var stores = new List<Store>
                {
                    new Store()
                };

            storeRepositoryMock.Setup( s => s.GetAll() )
                .Returns( stores.AsQueryable() )
                .Verifiable();

            // Act
            var result = controller.Index();

            // Assert
            storeRepositoryMock.Verify();

            Assert.IsInstanceOf<ViewResult>( result );

            var view = (ViewResult)result;

            Assert.IsInstanceOf<List<Store>>( view.Model );
        }

        [Test]
        public void Index_NoStoresFound()
        {
            // Arrange
            var controller = GetHomeController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>( result );

            var view = (ViewResult)result;

            Assert.AreEqual( "Error", view.ViewName );
        }
        
        private HomeController GetHomeController()
        {
            return new HomeController( storeRepositoryMock.Object );
        }
    }
}
