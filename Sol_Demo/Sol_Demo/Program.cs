using System;
using System.Threading.Tasks;

namespace Sol_Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Task.Run(async () =>
            {
                AsyncDemo asyncDemoObj = new AsyncDemo();
                //string fullName = await asyncDemoObj.DemoOldAsync("Kishor", "Naik");
                //Console.WriteLine(fullName);

                string fullName = await asyncDemoObj.DemoNewWay("Kishor", "Naik");
                Console.WriteLine(fullName);

                fullName = await asyncDemoObj.DemoValueTask("Kishor", "Naik");
                Console.WriteLine(fullName);

                await asyncDemoObj.LongRunningTask();
            }).Wait();
        }
    }

    public class AsyncDemo
    {
        // Old Way
        public Task<string> DemoOldAsync(string firstName, string lastName)
        {
            // Old Way
            return Task.Run(() =>
            {
                return $"{firstName} {lastName}";
            });
        }

        // New Way
        public Task<String> DemoNewWay(string firstName, string lastName)
        {
            // New Way
            string fullName = $"{firstName} {lastName}";
            return Task.FromResult<String>(fullName);
        }

        // Value Task instead of Task
        public ValueTask<String> DemoValueTask(string firstName, string lastName)
        {
            string fullName = $"{firstName} {lastName}";
            return new ValueTask<string>(fullName);
        }

        // Long Running Task
        public Task LongRunningTask()
        {
            return Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Long Running Task");
            }, TaskCreationOptions.LongRunning);
        }
    }
}