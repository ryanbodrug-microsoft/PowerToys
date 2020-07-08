using Microsoft.Plugin.Program.Programs;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Fakes;
using Wox.Infrastructure.Storage;

namespace Microsoft.Plugin.Program.UnitTests.Storage
{
    [TestFixture]
    class PackageRepositoryTest
    {

        [Test]
        public void PackageInstallingEvent_ShouldTriggerAdd_WhenInstallingIsComplete()
        {
            using (ShimsContext.Create())
            {

                var mockPackageCatalog = new Mock<IPackageCatalog>();
                var mockStorage = new Mock<IStorage<IList<Program.Programs.UWP.Application>>>();
                IRepository<UWP.Application> packageRepo = new Program.Storage.PackageRepository(mockPackageCatalog.Object, mockStorage.Object);

                ShimPackageInstallingEventArgs.AllInstances.IsCompleteGet = (PackageInstallingEventArgs p) => true;
                ShimPackage.AllInstances.DisplayNameGet = (Package p) => "Test";

                //Act
                mockPackageCatalog.Raise(m => m.PackageInstalling += null, new ShimPackageInstallingEventArgs());

                //Assert that the package is added
                Assert.IsTrue(packageRepo.Any());
            }
        }
    }
}
