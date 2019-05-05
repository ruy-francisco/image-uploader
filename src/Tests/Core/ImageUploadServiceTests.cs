using System;
using System.Threading.Tasks;
using Core.Classes;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Tests.Core
{
    public class ImageUploadServiceTests
    {
        private readonly Mock<IImageRepository> _repository;

        public ImageUploadServiceTests()
        {
            _repository = new Mock<IImageRepository>();
        }

        [Fact]
        public async void UploadImage_ShouldNot_TranspassMaxImages(){
            //Arrange
            _repository.SetupGet(r => r.AmountOfFiles).Returns(8);

            //Act
            var service = new ImageUploadService(_repository.Object);

            //Assert
            await Assert.ThrowsAnyAsync<CustomUploadException>(() => service.UploadImage(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public async void UploadImage_WithNoPngFormat_Should_ThrowCustomUploadException(){
            //Arrange
            _repository.SetupGet(r => r.AmountOfFiles).Returns(1);

            var inputImage = new Mock<IFormFile>();
            inputImage.SetupGet(i => i.ContentType).Returns("image/jpeg");

            //Act
            var service = new ImageUploadService(_repository.Object);

            //Assert
            await Assert.ThrowsAsync<CustomUploadException>(() => service.UploadImage(inputImage.Object, It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public async void UploadImage__Should_Success(){
            //Arrange
            _repository.SetupGet(r => r.AmountOfFiles).Returns(1);

            var inputImage = new Mock<IFormFile>();
            inputImage.SetupGet(i => i.ContentType).Returns("image/png");

            //Act
            var service = new ImageUploadService(_repository.Object);
            var result = await service.UploadImage(inputImage.Object, It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.Equal("Upload feito com sucesso", result);
        }
    }
}