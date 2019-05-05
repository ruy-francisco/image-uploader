using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IImageUploader _imageUploader;
        private readonly IImageDownloader _imageDownloader;
        private readonly IImageManager _imageManager;

        public HomeController(IImageUploader imageUploader, IImageDownloader imageDownloader, IImageManager imageManager)
        {
            _imageUploader = imageUploader;
            _imageDownloader = imageDownloader;
            _imageManager = imageManager;
            _imageManager.ClearAllImages();
        }

        public IActionResult Index(string errorMessage, bool returnedError = false)
        {
            ViewBag.returnedError = returnedError;
            ViewBag.errorMessage = errorMessage;
            ViewBag.images = ListImages();
            return View();
        }

        private List<Image> ListImages() => _imageManager.ListImages();

        [HttpPost]
        public async Task<IActionResult> UploadImage(string imageName, string imageDescription){
            try
            {
                if (string.IsNullOrEmpty(imageName) || string.IsNullOrEmpty(imageDescription))
                {
                    throw new Exception("Os campos Nome da Imagem e Descrição da Imagem são obrigatórios.");
                }

                var files = Request.Form.Files;

                if (files.Count == 0)
                {
                    throw new Exception("Necessário selectionar arquivo para upload.");
                }

                if (files.Count > 1)
                {
                    throw new Exception("Necessário realizar um upload de cada vez.");
                }

                var formFile = files.First();

                if (!ImageIs200x200(formFile))
                {
                    throw new Exception("A imagem deve ser do tamanho 200px por 200px.");
                }

                var result = await _imageUploader.UploadImage(formFile, imageName, imageDescription);                
            }
            catch (Exception e)
            {        
                ViewBag.returnedError = true;
                ViewBag.errorMessage = e.Message;
                return RedirectToAction("Index", new { errorMessage = e.Message, returnedError = true });
            }

            return RedirectToAction("Index", new { errorMessage = "" });
        }

        private bool ImageIs200x200(IFormFile formFile)
        {
            using (var image = System.Drawing.Image.FromStream(formFile.OpenReadStream()))
            {
                return image.Width == 200 && image.Height == 200;
            }
        }

        public async Task<IActionResult> DeleteImage(int id){
            var listOfImages = ListImages();
            var image = listOfImages.Find(i => i.Id == id);
            await _imageManager.DeleteImage(image);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DownloadImage(int id){
            try
            {
                var listOfImages = ListImages();
                var image = listOfImages.Find(i => i.Id == id);
                var imageByteArray = await _imageDownloader.Download(image);

                return File(imageByteArray, "application/octet-stream", string.Concat(image.Name, ".png"));       
            }
            catch (Exception e)
            {
                ViewBag.returnedError = true;
                ViewBag.errorMessage = e.Message;
                return RedirectToAction("Index", new { errorMessage = e.Message, returnedError = true });
            }
        }
    }
}
