using System;
using System.Collections.Generic;
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

                AsyncLoopDemo asyncLoopDemo = new AsyncLoopDemo();

                await asyncLoopDemo.LoopAsync(new List<String>() { "Kishor", "Eshaan", "Deepika" });

                var result = await asyncLoopDemo.LoopAsyncResult(new List<String>() { "Kishor", "Eshaan", "Deepika" });
                foreach(var value in result)
                {
                    Console.WriteLine($"Item: {value}");
                }

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

    public class AsyncLoopDemo
    {
        private Task DoAsync(string Item)
        {
            Task.Delay(6000);
            Console.WriteLine($"Item: {Item}");
            return Task.CompletedTask;
        }

        public async Task LoopAsync(IEnumerable<string> thingsToLoop)
        {
            List<Task> listOfTasks = new List<Task>();

            foreach (var thing in thingsToLoop)
            {
                listOfTasks.Add(DoAsync(thing));
            }

            await Task.WhenAll(listOfTasks);
        }

        private Task<string> DoAsyncResult(string item)
        {
            Task.Delay(1000);
            return Task.FromResult(item);
        }

        //Method to iterate over collection and await DoAsyncResult
        public  async Task<IEnumerable<string>> LoopAsyncResult(IEnumerable<string> thingsToLoop)
        {
            List<Task<string>> listOfTasks = new List<Task<string>>();

            foreach (var thing in thingsToLoop)
            {
                listOfTasks.Add(DoAsyncResult(thing));
            }

            return await Task.WhenAll<string>(listOfTasks);
        }
    }
}