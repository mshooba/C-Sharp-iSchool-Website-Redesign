using System.Net.Http.Headers;

namespace iSchoolWebApp.Utils
{
    public class DataRetrieval
    {
        /*
         * Task - has async/await and a return value
         * 
         * Thread - no direct way to get a callback from a thread...
         * 
         * THis method will take in a string (like "about/") and return a string of a JSON object
         */

        public async Task<string> GetData(string d)
        {
            //using statement - at the end of it it is automatically disposed of
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://ist.rit.edu/api/");
                //clear out the request headers...
                client.DefaultRequestHeaders.Accept.Clear();
                //we are declaring that the return has to be of type app/json
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //setup complete!

                try
                {
                    //http://ist.rit.edu/api/about/
                    HttpResponseMessage response = await client.GetAsync(d,HttpCompletionOption.ResponseHeadersRead);
                    //make sure it worked!
                    response.EnsureSuccessStatusCode();
                    //go get the data!  Data is just a string...
                    var data = await response.Content.ReadAsStringAsync();
                    return data;
                }
                catch (HttpRequestException hre)
                {
                    var msg = hre.Message;
                    return "HttpRequest: " + msg;
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    return "Ex: " + msg;
                }

            }
        }
    }
}
