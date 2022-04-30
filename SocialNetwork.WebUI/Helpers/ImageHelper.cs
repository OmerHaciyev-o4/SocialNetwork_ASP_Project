using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SocialNetwork.WebUI.Helpers
{
    public static class ImageHelper
    {
        public static string GetURL(IWebHostEnvironment webHost, IFormFile file, int id, string Imglocation)
        {
            string imgExtension = Path.GetExtension(file.FileName);
            string imgFileName = $"{id}-{Imglocation}{imgExtension}";
            var location = Path.Combine(webHost.WebRootPath, "images", imgFileName);
            using (var imgWork = new FileStream(location, FileMode.Create))
            {
                file.CopyTo(imgWork);
            }

            return location;
        }
    }
}