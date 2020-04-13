using System;
using System.IO;
using System.Net;

namespace HiredScore
{
    class Program
    {
        static void Main(string[] args)
        {
            string target = args[0];

            // string pagerdutyservicekey = Environment.GetEnvironmentVariable("PDKEY");

            if (GetStatusCode(target) != HttpStatusCode.OK)
            {
                //TriggerPdIncident(pagerdutyservicekey, target + " is not OK");
            }
        }

        private static void TriggerPdIncident(string pagerdutyservicekey, string description)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://events.pagerduty.com/generic/2010-04-15/create_event.json");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"service_key\":\"" + pagerdutyservicekey + "\"," +
                              "\"event_type\":\"trigger\"," +
                              "\"description\":\"" + description + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
        }

        private static HttpStatusCode GetStatusCode(string target)
        {
            WebRequest myWebRequest = WebRequest.Create(target);
            // TODO safe cast
            HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();

            myWebResponse.Close();

            Console.WriteLine(myWebResponse.StatusCode);
            return myWebResponse.StatusCode;
        }
    }
}
