using iSchoolWebApp.Models;
using iSchoolWebApp.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace iSchoolWebApp.Controllers
{
    public class DegreeController : Controller
    {
        private readonly ILogger<DegreeController> _logger;

        public DegreeController(ILogger<DegreeController> logger)
        {
            _logger = logger;
        }

        //why do I need this?
        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> Degrees()
        {

            //first, I need to go get the data
            DataRetrieval dataR = new DataRetrieval();
            //go and load the data
            var loadedDegrees = await dataR.GetData("degrees/");
            //issue is now that loadedAbout is a string, need it to be JSON!
            //another issue is that Visual Studio does not have a good JSON converter
            //Newtonsoft.Json to the rescue! (library for converting to JSON)
            //install newtonsoft in the nuget packages

            //convert loadedabout string to JSON, put that into the model and store in jsonResult
            var jsonResult = JsonConvert.DeserializeObject<DegreeRootModel>(loadedDegrees);
            
            
            //why do I need this link for it to work?
            return View("~/Views/Home/Degrees.cshtml", jsonResult);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}