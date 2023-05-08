using iSchoolWebApp.Models;
using iSchoolWebApp.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Dynamic;

namespace iSchoolWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

  
        private void SetActiveLink(string activeLink)
        {
            //Set ViewBag property for active links
            ViewBag.ActiveLink = activeLink;
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {

            //Set ViewBag property for active links
            SetActiveLink("Home");

            DataRetrieval dataR = new DataRetrieval();
            //Load the data from "about/"
            var loadedAbout = await dataR.GetData("about/");
            var jsonResult = JsonConvert.DeserializeObject<AboutRootModel>(loadedAbout);

            return View(jsonResult);

        }

        public async Task<IActionResult> People()
        {
            SetActiveLink("People");
            DataRetrieval dataR = new DataRetrieval();
            var loadedPeople = await dataR.GetData("people/");
            var jsonResult = JsonConvert.DeserializeObject<PeopleRootModel>(loadedPeople);
            return View(jsonResult);

        }

        public async Task<IActionResult> Degrees()
        {
            SetActiveLink("Degrees");
            DataRetrieval dataR = new DataRetrieval();
            var loadedDegrees = await dataR.GetData("degrees/");
            var jsonResult = JsonConvert.DeserializeObject<DegreeRootModel>(loadedDegrees);
            return View(jsonResult);

        }


        public async Task<IActionResult> MinorsWithCourses()
        {

            SetActiveLink("MinorsWithCourses");

            //object to make API requests
            DataRetrieval dataR = new DataRetrieval();

            // Load data from the "minors/" and "course/"
            var loadedMinors = await dataR.GetData("minors/");
            var loadedCourses = await dataR.GetData("course/");

            // Deserialize the JSON responses into C# objects
            var minorJson = JsonConvert.DeserializeObject<MinorRootModel>(loadedMinors);
            var courseJson = JsonConvert.DeserializeObject<List<CourseRootModel>>(loadedCourses);

            // Create a new expando object to hold the data that will go to the view
            dynamic model = new ExpandoObject();

            // Add the list of undergraduate minors to the expando
            model.UgMinors = minorJson.UgMinors;

            // Create a dictionary of courses matched by the courseID
            model.CourseDictionary = courseJson.ToDictionary(course => course.courseID);

            // Update the courses list for each minor to be a list of courseIDs 
            foreach (var minor in model.UgMinors)
            {
                var courseIDs = new List<string>();
                foreach (var courseID in minor.courses)
                {
                    //add the courseID to the list
                    courseIDs.Add(courseID);
                }
                //set the courses to the courseID
                minor.courses = courseIDs;
            }

            // Return the view with the expando
            return View(model);
        }





        public async Task<IActionResult> Employment()
        {
            SetActiveLink("Employment");
            DataRetrieval dataR = new DataRetrieval();
            var loadedEmployment = dataR.GetData("employment/");
            var jsonResult = JsonConvert.DeserializeObject<EmploymentRootModel>(await loadedEmployment);
            return View(jsonResult);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //set ViwBag propery for active links

    }
}