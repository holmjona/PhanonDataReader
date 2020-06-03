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

    }
}