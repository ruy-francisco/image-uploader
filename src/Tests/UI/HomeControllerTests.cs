using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UI.Controllers;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Tests.UI
{
    public class HomeControllerTests
    {

        private readonly Mock<IImageUploader> _imageUploader;
        private readonly Mock<IImageDownloader> _imageDownloader;
        private readonly Mock<IImageManager> _imageManager;

        public HomeControllerTests()
        {
            _imageUploader = new Mock<IImageUploader>();
            _imageDownloader = new Mock<IImageDownloader>();
            _imageManager = new Mock<IImageManager>();
        }

        [Fact]
        public void Index_Should_Success(){
            //Arrange
            _imageManager
                .Setup(s => s.ListImages())
                .Returns(new List<Image>());

            var homeController = new HomeController(_imageUploader.Object, _imageDownloader.Object, _imageManager.Object);
            //Act
            var actionResult = homeController.Index(It.IsAny<string>());
            var viewResult = actionResult as ViewResult;

            //Assert
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async void DeleteImage_Should_Success() {
            //Arrange
            _imageManager
                .Setup(s => s.ListImages())
                .Returns(new List<Image>(){
                    new Image{
                        Id = 1,
                        Name = "TestImage",
                        Description = "Description",
                        Path = string.Empty
                    }
                });

            _imageManager
                .Setup(s => s.DeleteImage(It.IsAny<Image>()))
                .Returns(Task.FromResult("Result"));

            var homeController = new HomeController(_imageUploader.Object, _imageDownloader.Object, _imageManager.Object);
            //Act
            var taskResult = await homeController.DeleteImage(1);
            var redirectResult = taskResult as RedirectToActionResult;

            //Assert
            Assert.NotNull(redirectResult);
        }

        [Fact]
        public async void UploadImage_WithNoNameOrDescription_Should_ReturnsErrorMessage(){
            //Arrange
            var homeController = new HomeController(_imageUploader.Object, _imageDownloader.Object, _imageManager.Object);
            
            //Act
            var result = await homeController.UploadImage(string.Empty, "Teste");
            var redirectResult = result as RedirectToActionResult;
            var errorMessage = redirectResult.RouteValues.Values.First();
            
            //Assert
            Assert.Equal("Os campos Nome da Imagem e Descrição da Imagem são obrigatórios.", errorMessage);
        }

        [Fact]
        public async void UploadImage_WithNoFile_Should_ReturnsErrorMessage(){
            //Arrange
            var homeController = new HomeController(_imageUploader.Object, _imageDownloader.Object, _imageManager.Object){
                ControllerContext = CreateNoFileControllerContextMock()
            };
            
            //Act
            var result = await homeController.UploadImage("Imagem", "Descrição");
            var redirectResult = result as RedirectToActionResult;
            var errorMessage = redirectResult.RouteValues.Values.First();
            
            //Assert
            Assert.Equal("Necessário selecionar arquivo para upload.", errorMessage);
        }

        [Fact]
        public async void UploadImage_MoreThanOneFile_Should_ReturnsErrorMessage(){
            //Arrange
            var homeController = new HomeController(_imageUploader.Object, _imageDownloader.Object, _imageManager.Object){
                ControllerContext = CreateManyFilesControllerContextMock()
            };
            
            //Act
            var result = await homeController.UploadImage("Imagem", "Descrição");
            var redirectResult = result as RedirectToActionResult;
            var errorMessage = redirectResult.RouteValues.Values.First();
            
            //Assert
            Assert.Equal("Necessário realizar um upload de cada vez.", errorMessage);
        }

        [Fact]
        public async void DownloadImage_Should_Success(){
            //Arrange
            _imageManager
                .Setup(s => s.ListImages())
                .Returns(new List<Image>(){
                    new Image{
                        Id = 1,
                        Name = "TestImage",
                        Description = "Description",
                        Path = string.Empty
                    }
                });

            _imageDownloader
                .Setup(s => s.Download(It.IsAny<Image>()))
                .Returns(Task.Run(() => new byte[] {}));

            //Act
            var homeController = new HomeController(_imageUploader.Object, _imageDownloader.Object, _imageManager.Object);
            var result = await homeController.DownloadImage(1);
            var actionResult = result as FileResult;

            //Assert
            Assert.NotNull(actionResult);
        }

        private ControllerContext CreateNoFileControllerContextMock(){
            var formFileCollectionMock = new Mock<IFormFileCollection>();
            formFileCollectionMock.SetupGet(f => f.Count).Returns(0);

            var formCollectionMock = new Mock<IFormCollection>();
            formCollectionMock.SetupGet(f => f.Files).Returns(formFileCollectionMock.Object);

            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.SetupGet(h => h.Form).Returns(formCollectionMock.Object);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(h => h.Request).Returns(httpRequestMock.Object);

            var controllerContext = new ControllerContext {
                HttpContext = httpContextMock.Object
            };

            return controllerContext;
        }

        private ControllerContext CreateManyFilesControllerContextMock(){
            var formFileCollectionMock = new Mock<IFormFileCollection>();
            formFileCollectionMock.SetupGet(f => f.Count).Returns(2);

            var formCollectionMock = new Mock<IFormCollection>();
            formCollectionMock.SetupGet(f => f.Files).Returns(formFileCollectionMock.Object);

            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.SetupGet(h => h.Form).Returns(formCollectionMock.Object);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(h => h.Request).Returns(httpRequestMock.Object);

            var controllerContext = new ControllerContext {
                HttpContext = httpContextMock.Object
            };

            return controllerContext;
        }

    }
}