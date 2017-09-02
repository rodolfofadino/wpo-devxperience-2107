using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevXperience.Models;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace DevXperience.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<Photo> photosList;
            photosList = Photo.GetPhotos();

            return View(photosList);
        }

    }

    public class Photo
    {
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string thumbnailUrl { get; set; }

        public static List<Photo> GetPhotos()
        {
            var client = new RestClient("https://jsonplaceholder.typicode.com/");
            var request = new RestRequest("photos", Method.GET);
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var photoList = JsonConvert.DeserializeObject<List<Photo>>(response.Content);
            int count = 0;
            foreach (var item in photoList)
            {
                count++;
                item.url = $"/images/cat{count}.jpg";
            }
            return photoList.Take(20).ToList();
        }

        public async Task<List<Photo>> GetPhotosAsync()
        {
            var client = new RestClient("https://jsonplaceholder.typicode.com/");
            var request = new RestRequest("photos", Method.GET);
            var response = new RestResponse();

            response = await GetResponseContentAsync(client, request) as RestResponse;
            var photoList = JsonConvert.DeserializeObject<List<Photo>>(response.Content);
            int count = 0;
            foreach (var item in photoList)
            {
                count++;
                item.url = $"/images/cat{count}.jpg";
            }
            return photoList.Take(20).ToList();
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
