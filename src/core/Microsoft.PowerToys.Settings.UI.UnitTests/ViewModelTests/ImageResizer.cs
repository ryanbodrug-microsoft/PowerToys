﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.PowerToys.Settings.UI.Lib;
using Microsoft.PowerToys.Settings.UI.Lib.Utilities;
using Microsoft.PowerToys.Settings.UI.Lib.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ViewModelTests
{
    [TestClass]
    public class ImageResizer
    {
        public const string Module = "ImageResizer";


        //Stubs out empty values for imageresizersettings and general settings as needed by the imageresizer viewmodel
        private Mock<ISettingsUtils> GetStubSettingsUtils()
        {
            var settingsUtils = new Mock<ISettingsUtils>();
            settingsUtils.Setup(x => x.GetSettings<GeneralSettings>(It.IsAny<string>(), It.IsAny<string>())).Returns(new GeneralSettings());
            settingsUtils.Setup(x => x.GetSettings<ImageResizerSettings>(It.IsAny<string>(), It.IsAny<string>())).Returns(new ImageResizerSettings());
            return settingsUtils;
        }

        [TestMethod]
        public void IsEnabled_ShouldEnableModule_WhenSuccessful()
        {
            var mockSettingsUtils = GetStubSettingsUtils();

            // Assert
            Func<string, int> SendMockIPCConfigMSG = msg =>
            {
                OutGoingGeneralSettings snd = JsonSerializer.Deserialize<OutGoingGeneralSettings>(msg);
                Assert.IsTrue(snd.GeneralSettings.Enabled.ImageResizer);
                return 0;
            };

            // arrange
            ImageResizerViewModel viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);

            // act
            viewModel.IsEnabled = true;
        }

        [TestMethod]
        public void JPEGQualityLevel_ShouldSetValueToTen_WhenSuccessful()
        {
            // arrange
            var mockSettingsUtils = GetStubSettingsUtils();
            Func<string, int> SendMockIPCConfigMSG = msg => { return 0; };
            ImageResizerViewModel viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);

            // act
            viewModel.JPEGQualityLevel = 10;

            // Assert
            viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);
            Assert.AreEqual(10, viewModel.JPEGQualityLevel);
        }

        [TestMethod]
        public void PngInterlaceOption_ShouldSetValueToTen_WhenSuccessful()
        {
            // arrange
            var mockSettingsUtils = GetStubSettingsUtils();
            Func<string, int> SendMockIPCConfigMSG = msg => { return 0; };
            ImageResizerViewModel viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);

            // act
            viewModel.PngInterlaceOption = 10;

            // Assert
            viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);
            Assert.AreEqual(10, viewModel.PngInterlaceOption);
        }

        [TestMethod]
        public void TiffCompressOption_ShouldSetValueToTen_WhenSuccessful()
        {
            // arrange
            var mockSettingsUtils = GetStubSettingsUtils();
            Func<string, int> SendMockIPCConfigMSG = msg => { return 0; };
            ImageResizerViewModel viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);

            // act
            viewModel.TiffCompressOption = 10;

            // Assert
            viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);
            Assert.AreEqual(10, viewModel.TiffCompressOption);
        }

        [TestMethod]
        public void FileName_ShouldUpdateValue_WhenSuccessful()
        {
            // arrange
            var mockSettingsUtils = GetStubSettingsUtils();
            Func<string, int> SendMockIPCConfigMSG = msg => { return 0; };
            ImageResizerViewModel viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);
            string expectedValue = "%1 (%3)";

            // act
            viewModel.FileName = expectedValue;

            // Assert
            viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);
            Assert.AreEqual(expectedValue, viewModel.FileName);
        }

        [TestMethod]
        public void KeepDateModified_ShouldUpdateValue_WhenSuccessful()
        {
            // arrange
            var settingUtils = new Mock<ISettingsUtils>();

            var expectedSettingsString = new ImageResizerSettings() { Properties = new ImageResizerProperties() { ImageresizerKeepDateModified = new BoolProperty() { Value = true } } }.ToJsonString();
            settingUtils.Setup(x => x.SaveSettings(
                                        It.Is<string>(content => content.Equals(expectedSettingsString, StringComparison.Ordinal)),
                                        It.Is<string>(module => module.Equals(Module, StringComparison.Ordinal)),
                                        It.IsAny<string>()))
                                     .Verifiable();

            settingUtils.Setup(x => x.GetSettings<GeneralSettings>(It.IsAny<string>(), It.IsAny<string>())).Returns(new GeneralSettings());
            settingUtils.Setup(x => x.GetSettings<ImageResizerSettings>(It.IsAny<string>(), It.IsAny<string>())).Returns(new ImageResizerSettings());
            Func<string, int> SendMockIPCConfigMSG = msg => { return 0; };
            ImageResizerViewModel viewModel = new ImageResizerViewModel(settingUtils.Object, SendMockIPCConfigMSG);

            // act
            viewModel.KeepDateModified = true;

            // Assert
            settingUtils.Verify();
        }

        [TestMethod]
        public void Encoder_ShouldUpdateValue_WhenSuccessful()
        {
            // arrange
            var mockSettingsUtils = GetStubSettingsUtils();
            Func<string, int> SendMockIPCConfigMSG = msg => { return 0; };
            ImageResizerViewModel viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);

            // act
            viewModel.Encoder = 3;

            // Assert
            viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);
            Assert.AreEqual("163bcc30-e2e9-4f0b-961d-a3e9fdb788a3", viewModel.GetEncoderGuid(viewModel.Encoder));
            Assert.AreEqual(3, viewModel.Encoder);
        }

        [TestMethod]
        public void AddRow_ShouldAddEmptyImageSize_WhenSuccessful()
        {
            // arrange
            var mockSettingsUtils = GetStubSettingsUtils();
            Func<string, int> SendMockIPCConfigMSG = msg => { return 0; };
            ImageResizerViewModel viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);
            int sizeOfOriginalArray = viewModel.Sizes.Count;

            // act
            viewModel.AddRow();

            // Assert
            Assert.AreEqual(viewModel.Sizes.Count, sizeOfOriginalArray + 1);
        }

        [TestMethod]
        public void DeleteImageSize_ShouldDeleteImageSize_WhenSuccessful()
        {
            // arrange
            var mockSettingsUtils = GetStubSettingsUtils();
            Func<string, int> SendMockIPCConfigMSG = msg => { return 0; };
            ImageResizerViewModel viewModel = new ImageResizerViewModel(mockSettingsUtils.Object, SendMockIPCConfigMSG);
            int sizeOfOriginalArray = viewModel.Sizes.Count;
            ImageSize deleteCandidate = viewModel.Sizes.Where<ImageSize>(x => x.Id == 0).First();

            // act
            viewModel.DeleteImageSize(0);

            // Assert
            Assert.AreEqual(viewModel.Sizes.Count, sizeOfOriginalArray - 1);
            Assert.IsFalse(viewModel.Sizes.Contains(deleteCandidate));
        }

        [TestMethod]
        public void UpdateWidthAndHeight_ShouldUpateSize_WhenCorrectValuesAreProvided()
        {
            // arrange
            ImageSize imageSize = new ImageSize()
            {
                Id = 0,
                Name = "Test",
                Fit = (int)ResizeFit.Fit,
                Width = 30,
                Height = 30,
                Unit = (int)ResizeUnit.Pixel,
            };

            double negativeWidth = -2.0;
            double negativeHeight = -2.0;

            // Act
            imageSize.Width = negativeWidth;
            imageSize.Height = negativeHeight;

            // Assert
            Assert.AreEqual(0, imageSize.Width);
            Assert.AreEqual(0, imageSize.Height);

            // Act
            imageSize.Width = 50;
            imageSize.Height = 50;

            // Assert
            Assert.AreEqual(50, imageSize.Width);
            Assert.AreEqual(50, imageSize.Height);
        }

        /// <summary>
        /// This method mocks an IO provider to validate tests wich required saving to a file, and then reading the contents of that file, or verifying it exists
        /// </summary>
        /// <returns></returns>
        Mock<IIOProvider> GetMockIOProviderForSaveLoadExists()
        {
            string savePath = string.Empty;
            string saveContent = string.Empty;
            var mockIOProvider = new Mock<IIOProvider>();
            mockIOProvider.Setup(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                          .Callback<string, string>((path, content) =>
                          {
                              savePath = path;
                              saveContent = content;
                          });
            mockIOProvider.Setup(x => x.ReadAllText(It.Is<string>(x => x.Equals(savePath, StringComparison.Ordinal))))
                          .Returns(() => saveContent);

            mockIOProvider.Setup(x => x.FileExists(It.Is<string>(x => x.Equals(savePath, StringComparison.Ordinal))))
                          .Returns(true);
            mockIOProvider.Setup(x => x.FileExists(It.Is<string>(x => !x.Equals(savePath, StringComparison.Ordinal))))
                          .Returns(false);

            return mockIOProvider;
        }
    }
}
