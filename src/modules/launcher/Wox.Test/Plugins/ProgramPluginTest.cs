﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Wox.Infrastructure;
using Wox.Plugin;
using Microsoft.Plugin.Program.Programs;
using Moq;
using System.IO;
using Microsoft.Plugin.Program.Storage;
using Wox.Infrastructure.Storage;

namespace Wox.Test.Plugins
{
    [TestFixture]
    public class ProgramPluginTest
    {
      
		
		[Test]
        public void StorageRepository_ShouldContainItem_WhenInitializedWithItem()
        {
            //Arrange
            var itemName = "originalItem1";
            var mockStorage = new Mock<IStorage<IList<string>>>();
            IRepository<string> repository = new ListRepository<string>(mockStorage.Object) { itemName };

            //Act
            var result = repository.Contains(itemName);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void StorageRepository_ShouldContainItem_AfterAdd()
        {
            //Arrange
            var mockStorage = new Mock<IStorage<IList<string>>>();
            IRepository<string> repository = new ListRepository<string>(mockStorage.Object);

            //Act
            var itemName = "newItem";
            repository.Add(itemName);
            var result = repository.Contains(itemName);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void StorageRepository_ShouldRemoveAnItem_WhenItExistsInTheList()
        {
            //Arrange
            var itemName = "originalItem1";
            var mockStorage = new Mock<IStorage<IList<string>>>();
            IRepository<string> repository = new ListRepository<string>(mockStorage.Object) { itemName };

            //Act
            repository.Remove(itemName);
            var result = repository.Contains(itemName);

            //Assert
            Assert.IsFalse(result);
        }
    }
}
