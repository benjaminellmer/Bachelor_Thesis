using System;
using System.Collections;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentNone {
    internal class Program {
        static async Task Main(string[] args) {
            int iterations = 1000;
            int testIteration = 5;

            ArrayList iterationTimes = new ArrayList();
            HttpClient httpClient = new HttpClient();

            DateTime startTotal = DateTime.Now;
            for (int i = 0; i < iterations; i++) {
                DateTime startIteration = DateTime.Now;
                using (var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5001/api/RandomNumber")) {
                    var response = await httpClient.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    Console.WriteLine((i + 1) + " of: " + iterations);
                }
                DateTime endIteration = DateTime.Now;
                TimeSpan durationIteration = (endIteration - startIteration);
                iterationTimes.Add(durationIteration.TotalMilliseconds);
            }
            DateTime endTotal = DateTime.Now;

            TimeSpan durationTotal = (endTotal - startTotal);
            Console.WriteLine("Elapsed Time is {0} s", durationTotal.TotalSeconds);
            Console.WriteLine("Average Request duration is {0} ms", (durationTotal.TotalMilliseconds / (double)iterations));

            //before your loop
            var csv = new StringBuilder();

            foreach (var iteration in iterationTimes) {
                var val = iteration;
                var newLine = string.Format("{0}", val);
                csv.AppendLine(newLine);
            }
            String filename = "results-" + iterations + "-NONE_0" + testIteration + ".csv";
            //after your loop
            File.WriteAllText(filename, csv.ToString());
            Task.Delay(60000).Wait();   // wait 50ms

            iterationTimes.Clear();
        }
    }
}
