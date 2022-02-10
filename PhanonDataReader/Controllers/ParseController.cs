using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhanonDataReader.Models;

namespace PhanonDataReader.Controllers
{
    public class ParseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Matrix() {
            return View();
        }
        
        public IActionResult Exercises() {
          return View();
        }

        //https://dotnetcoretutorials.com/2019/09/11/how-to-parse-json-in-net-core/
        [HttpPost]
        public IActionResult FormattedExercises(String json) {
            LessonHolder lessh = JsonConvert.DeserializeObject<LessonHolder>(json);
            return View("Exercises",lessh);
        }

        /// <summary>
        /// This call should stitch together all of the files for a course before calling the matrix
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// https://stackoverflow.com/questions/42440949/net-core-calling-web-api-from-mvc-application-with-windows-authentication-on
        public async Task<IActionResult> Stitcher(int id) {
            // id => course ID folder from Phanon

            try {
                PhanonCredentialList pcl = new PhanonCredentialList();
                string pass = "";
                string uname = "";
                pcl.Add(new Uri("http://phanon.herokuapp.com"), "Login", new System.Net.NetworkCredential(uname,pass));
                using (var client = new HttpClient(new HttpClientHandler { Credentials = pcl})) {
                    var response = await client.GetStringAsync(String.Format("http://phanon.herokuapp.com/api/progress_maps/{0}?per_page=400&page=0",id));
                    return View("Matrix");
                }
            } catch (Exception ex) {
                return View("Matrix");
            }
        }

    }
}