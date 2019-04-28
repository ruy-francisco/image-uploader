using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public HomeController(IImageUploader imageUploader)
        {
            _imageUploader = imageUploader;
            //_imageDownloader = imageDownloader;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task UploadImage(string imageName, string imageDescription){
            try
            {
                var files = Request.Form.Files;

                if (files.Count > 1)
                {
                    throw new Exception("Necessário realizar um upload de cada vez.");
                }

                var formFile = files.First();
                var result = await _imageUploader.UploadImage(formFile, imageName, imageDescription);
                
            }
            catch (Exception e)
            {                
                throw;
            }
        }
    }
}
