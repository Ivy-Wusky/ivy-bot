using System;
using Discord;
// using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IvyBot.Services {
    public class LogService {
        private readonly SemaphoreSlim _semaphoreSlim;

        public LogService () {
            _semaphoreSlim = new SemaphoreSlim (1);
        }

        internal async Task LogAsync (LogMessage arg) {
            await _semaphoreSlim.WaitAsync ();

            var timeStamp = DateTime.UtcNow.ToString ("MM/dd/yyyy hh:mm tt");
            const string format = "{0,-10} {1,10}";

            Console.WriteLine ($"[{timeStamp}] {string.Format(format, arg.Source, $": {arg.Message}")}");

            // await File.AppendAllTextAsync("ivy-bot.log", $"{timeStamp}] {string.Format(format, arg.Source, $": {arg.Message}")}" + Environment.NewLine);

            _semaphoreSlim.Release ();
        }
    }
}