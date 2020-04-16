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

                await asyncDemoObj.DemoOldAsync();

                await asyncDemoObj.DemoNewAsync();

                //string fullName = await asyncDemoObj.DemoOldAsync("Kishor", "Naik");
                //Console.WriteLine(fullName);

                string fullName = await asyncDemoObj.DemoNewWay("Kishor", "Naik");
                Console.WriteLine(fullName);

                fullName = await asyncDemoObj.DemoValueTask("Kishor", "Naik");
                Console.WriteLine(fullName);

                await asyncDemoObj.LongRunningTask();

                await asyncDemoObj.LongRunningParallelism();
            }).Wait();
        }
    }

    public class AsyncDemo
    {
        // Old Way (Task does not return) (It will not create a Thread) (Concurrency)
        public Task DemoOldAsync()
        {
            return Task.Run(() => Console.WriteLine("Old Way"));
        }

        public Task DemoNewAsync()
        {
            Console.WriteLine("New way");
            return Task.CompletedTask;
        }


        // Old Way (Task return) (It will not create a Thread) (Concurrency)
        public Task<string> DemoOldAsync(string firstName, string lastName)
        {
            // Old Way
            return Task.Run(() =>
            {
                return $"{firstName} {lastName}";
            });
        }

        // New Way (It will not create a thread) (Concurrency)
        public Task<String> DemoNewWay(string firstName, string lastName)
        {
            // New Way
            string fullName = $"{firstName} {lastName}";
            return Task.FromResult<String>(fullName);
        }

        // Value Task instead of Task (It will not create a thread) (Concurrency)
        public ValueTask<String> DemoValueTask(string firstName, string lastName)
        {
            string fullName = $"{firstName} {lastName}";
            return new ValueTask<string>(fullName);
        }

        // Long Running Task (It will create a thread) (Parallelism)
        public Task LongRunningTask()
        {
            return Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Long Running Task");
            }, TaskCreationOptions.LongRunning);
        }

        public Task LongRunningParallelism()
        {
            Parallel.Invoke(
                () => { Console.WriteLine("Task 1"); },
                () => { Console.WriteLine("Task 2"); },
                () => { Console.WriteLine("Task 3"); }
                );

            return Task.CompletedTask;
        }


    }
}