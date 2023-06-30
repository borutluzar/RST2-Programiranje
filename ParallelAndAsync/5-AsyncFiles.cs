using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelAndAsync
{
    /// <summary>
    /// Arh, Q70
    /// V tem razdelku bomo naredili primere najboljših praks pri 
    /// asinhronih operacijah z datotekami.
    /// 
    /// Vemo že, da so operacije z datotekami (ali bolj splošno I/O operacije) 
    /// lahko do stokrat ali tisočkrat počasnejše od računskih operacij
    /// pa čeprav imamo zdaj opravka z datotekami na lokalnih SSD diskih.
    /// </summary>
    class AsyncFiles
    {
        public static void AsyncFilesTest()
        {
            List<string> filePaths = new List<string>()
            {
                @"C:\Graph-Files\max3con2\max3con2num18_g6.g6",
                @"C:\Graph-Files\max3con2\max3con2num18_g5.g6",
                @"C:\Graph-Files\max3con2\max3con2num18.g6",
                @"C:\Graph-Files\max3con2\max3con2num19_g5.g6",
                @"C:\Graph-Files\max3con2\max3con2num19.g6",
                //@"C:\Graph-Files\max3con2\max3con2num20.g6"
            };

            // PRIMER 1: Branje datotek zaporedno
            /*
            // Pri branju velikih datotek, se ob uporabi zgolj sinhrone kode, naša aplikacija ustavi in čaka.
            foreach (var filePath in filePaths)
            {
                Console.WriteLine($"\nBeremo datoteko {Path.GetFileName(filePath)}.");
                double time = MeasureTime(ReadTextSync, filePath, out string text);
                //double time2 = MeasureTime(ReadTextSimpleSync, filePath, out string text2);
                Console.WriteLine($"Branje datoteke {Path.GetFileName(filePath)} nam je vzelo {time} sekund.");
                //Console.WriteLine($"Branje datoteke {Path.GetFileName(filePath)} s Streamreaderjem nam je vzelo {time2} sekund.");
                Console.WriteLine($"Prvih nekaj znakov v datoteki: {text.Substring(0, Math.Min(40, text.Length))}.");
                //Console.WriteLine($"Prvih nekaj znakov v datoteki: {text2.Substring(0, Math.Min(40, text2.Length))}.");
            }
            Console.WriteLine($"\nIn sedaj se je aplikacija 'odmrznila'.");
            */

            // PRIMER 2: Branje datotek asinhrono

            // Preverimo sedaj asinhrone klice
            
            foreach (var filePath in filePaths)
            {
                Console.WriteLine($"\nBeremo datoteko {Path.GetFileName(filePath)}.");
                double time = MeasureTime(ReadTextAsync, filePath, out Task<string> text);
                Console.WriteLine($"Branje datoteke {Path.GetFileName(filePath)} nam je vzelo {time} sekund.");
                Console.WriteLine($"Prvih nekaj znakov v datoteki: {text.Result.Substring(0, Math.Min(40, text.Result.Length))}.");
            }
             Console.WriteLine($"\nIn sedaj se je aplikacija 'odmrznila'."); // Lahko zaključimo s programom, čeprav se vzporedno še izvajajo operacije (glejte RAM :))
            
        }

        /// <summary>
        /// Calls the given function and measures execution time.
        /// </summary>
        private static double MeasureTime<T, TResult>(Func<T, TResult> f, T par, out TResult result)
        {
            Stopwatch sw = Stopwatch.StartNew();
            result = f(par);
            return sw.Elapsed.TotalSeconds;
        }

        /// <summary>
        /// Primer metode, ki prebere dano datoteko sinhrono
        /// </summary>
        private static string ReadTextSync(string fileName)
        {
            int bufferLength = 300000; // Zakaj prihaja do razlik v času branja, ko spreminjamo dolžino bufferja? Kaj je optimalna dolžina?
            var buffer = new byte[bufferLength];
            var strBuilder = new StringBuilder();

            using var stream = new FileStream(fileName, FileMode.Open);
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, bufferLength)) > 0)
            {
                strBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            try
            {
                return strBuilder.ToString();
            }
            catch (OutOfMemoryException)
            {
                return "We cannot handle such a big string";
            }
        }

        /// <summary>
        /// Uporabimo razred StreamReader, ki ga že poznamo.
        /// Opazimo, da je bistveno počasnejše kot zgornja metoda.
        /// Raziščite zakaj.
        /// </summary>
        private static string ReadTextSimpleSync(string fileName)
        {
            int bufferLength = 100000;
            var buffer = new char[bufferLength];
            using var streamReader = new StreamReader(fileName);
            StringBuilder strBuilder = new StringBuilder();
            while (!streamReader.EndOfStream)
            {
                streamReader.Read(buffer, 0, bufferLength);
                strBuilder.Append(buffer);
                //strBuilder.Append(streamReader.ReadLine());
            }
            try
            {
                return strBuilder.ToString();
            }
            catch (OutOfMemoryException)
            {
                return "We cannot handle such a big string";
            }
        }

        /// <summary>
        /// Asinhrono branje datoteke.
        /// - metodi damo določilo async!
        /// - vračamo Task z izbranim tipom rezultata!
        /// </summary>
        private static async Task<string> ReadTextAsync(string fileName)
        {
            int bufferLength = 100000; 
            var buffer = new byte[bufferLength];
            var strBuilder = new StringBuilder();

            using var stream = new FileStream(fileName, FileMode.Open);
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, bufferLength)) > 0) // Kličemo metodo ReadAsync in ob klicu damo pred njo določilo await
            {
                strBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            try
            {
                return strBuilder.ToString();
            }
            catch (OutOfMemoryException)
            {
                return "We cannot handle such a big string";
            }

            // Vse ostalo je enako kot v sinhroni metodi
        }


        /// <summary>
        /// Metodi dodajmo še možnost prekinitve!
        /// </summary>
        private static async Task<string> ReadTextAsync(string fileName, CancellationToken cancelToken)
        {
            int bufferLength = 1000;
            var buffer = new byte[bufferLength];
            var strBuilder = new StringBuilder();

            using var stream = new FileStream(fileName, FileMode.Open);
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, bufferLength, cancelToken)) > 0) // Tukaj dodamo sklic na token
            {
                strBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            try
            {
                return strBuilder.ToString();
            }
            catch (OutOfMemoryException)
            {
                return "We cannot handle such a big string";
            }
        }

        public static void AsyncFilesTestWithCancel()
        {
            List<string> filePaths = new List<string>()
            {
                @"C:\Graph-Files\max3con2\max3con2num18_g6.g6",
                @"C:\Graph-Files\max3con2\max3con2num18_g5.g6",
                @"C:\Graph-Files\max3con2\max3con2num18.g6",
                @"C:\Graph-Files\max3con2\max3con2num19_g5.g6",
                @"C:\Graph-Files\max3con2\max3con2num19.g6",
                @"C:\Graph-Files\max3con2\max3con2num20.g6"
            };

            // Berimo datoteke, vendar z možnostjo prekinitve
            CancellationTokenSource cancelSource = new CancellationTokenSource();

            foreach (var filePath in filePaths)
            {
                Console.WriteLine($"\nBeremo datoteko {Path.GetFileName(filePath)}.");
                Task<string> text = ReadTextAsync(filePath, cancelSource.Token);
                Console.WriteLine($"Branje datoteke {Path.GetFileName(filePath)}...");
            }

            Console.WriteLine($"\nAplikacija teče dalje... pritisni 0 za prekinitev.");
            while (true)
            {                
                string command = Console.ReadLine();
                if (command == "0")
                {
                    cancelSource.Cancel(); // Prekinimo branje
                    Console.WriteLine($"\nBranje datoteke se je zaključilo!");
                    GC.Collect(); // Pospravimo za sabo.
                    break;
                }
                else
                    Console.WriteLine($"\nPritisni 0 za prekinitev!");
            }
        }


        /// <summary>
        /// Poleg prekinitev, lahko spremljamo tudi delež pregledanega besedila.
        /// </summary>
        private static async Task<string> ReadTextAsync(string fileName, IProgress<int> progress, CancellationToken cancelToken)
        {
            var fi = new FileInfo(fileName);
            int fileSize = (int)fi.Length;

            int bufferLength = 1000;
            var buffer = new byte[bufferLength];
            var strBuilder = new StringBuilder();

            using var stream = new FileStream(fileName, FileMode.Open);
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, bufferLength, cancelToken)) > 0) // Tukaj dodamo sklic na token
            {
                strBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                progress.Report(strBuilder.Length * 100 / fileSize);
            }
            try
            {
                return strBuilder.ToString();
            }
            catch (OutOfMemoryException)
            {
                return "We cannot handle such a big string";
            }
        }

        public static void AsyncFilesTestWithCancelAndProgress()
        {
            List<string> filePaths = new List<string>()
            {
                /*@"C:\Graph-Files\max3con2\max3con2num18_g6.g6",
                @"C:\Graph-Files\max3con2\max3con2num18_g5.g6",
                @"C:\Graph-Files\max3con2\max3con2num18.g6",
                @"C:\Graph-Files\max3con2\max3con2num19_g5.g6",*/
                //@"C:\Graph-Files\max3con2\max3con2num19.g6",
                @"C:\Graph-Files\max3con2\max3con2num20.g6"
            };

            // Berimo datoteke, vendar z možnostjo prekinitve
            CancellationTokenSource cancelSource = new CancellationTokenSource();
            int progressValue = 0;
            var progress = new Progress<int>(percent => progressValue = percent);

            foreach (var filePath in filePaths)
            {
                Console.WriteLine($"\nBeremo datoteko {Path.GetFileName(filePath)}.");
                Task<string> text = ReadTextAsync(filePath, progress, cancelSource.Token);
                Console.WriteLine($"Branje datoteke {Path.GetFileName(filePath)}...");
            }

            Console.WriteLine($"\nAplikacija teče dalje... pritisni 0 za prekinitev.");
            while (true)
            {                
                string command = Console.ReadLine();
                if (command == "0")
                {
                    cancelSource.Cancel(); // Prekinimo branje
                    Console.WriteLine($"\nBranje datoteke se je zaključilo!");
                    GC.Collect(); // Pospravimo za sabo.
                    Console.WriteLine($"\nPrebrali smo {progressValue}% datoteke.!");
                    break;
                }
                else
                {
                    Console.WriteLine($"\nPritisni 0 za prekinitev!");
                    Console.WriteLine($"\nPrebrali smo {progressValue}% datoteke.!");
                }
            }
        }
    }
}
