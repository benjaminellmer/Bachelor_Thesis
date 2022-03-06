using System;
using System.Collections;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentMTLS {
    internal class Program {
        static async Task Main(string[] args) {
            int testIterations = 1;
            int iterations = 1000;
            ArrayList iterationTimes = new ArrayList();

            // Load pfx, which includes the certificate and the privaet key of the service
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            X509Certificate2 certificate = new X509Certificate2(projectDirectory + "/service.pfx", "B3njam1n");


            for (int j = 1; j <= 1; j++) {
                DateTime startTotal = DateTime.Now;
                for (int i = 0; i < iterations; i++) {
                    DateTime startIteration = DateTime.Now;
                    var handler = new HttpClientHandler();
                    handler.ClientCertificates.Add(certificate);
                    HttpClient httpClient = new HttpClient(handler);

                    using (var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5001/api/User?page=1&pagesize=5")) {
                        var response = await httpClient.SendAsync(request);
                        var content = await response.Content.ReadAsStringAsync();
                        //Console.WriteLine(content);
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

                String filename = "results-" + iterations + "-MTLS_0" + testIterations + ".csv";
                //after your loop
                File.WriteAllText(filename, csv.ToString());
                Task.Delay(60000).Wait();   // wait 50ms

                iterationTimes.Clear();
            }

            //int count = 1;
            //foreach (var iteration in iterationTimes) {
            //    Console.WriteLine(count++ + ": " + iteration);
            //}
        }
    }
}

