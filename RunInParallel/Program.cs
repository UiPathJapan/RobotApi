using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UiPath.Robot.Api;

namespace RunInParallel
{
    class Program
    {
        /// <summary>
        /// Runs the designated process asynchronously.
        /// </summary>
        /// <param name="processName">Name of the process to run</param>
        /// <returns>Task with JobOutput</returns>
        private static async Task<JobOutput> RunAsync(string processName)
        {
            try
            {
                var client = new RobotClient();
                var processes = await client.GetProcesses();
                var myProcess = processes.Single(process => process.Name == processName);
                var job = myProcess.ToJob();
                return await client.RunJob(job);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("{0}", ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Writes the output parameters in a JSON-like format to the standard output.
        /// </summary>
        /// <param name="output">JobOutput returned by the completed task</param>
        private static void PrintOutput(JobOutput output)
        {
            if (output == null || output.Arguments == null || output.Arguments.Count <= 0)
            {
                return;
            }
            int index = 0;
            Console.Write("{");
            foreach (KeyValuePair<string, object> kv in output.Arguments)
            {
                Console.Write("{0}\"{1}\": \"{2}\"", index++ == 0 ? " " : ", ", kv.Key, kv.Value);
            }
            Console.WriteLine(" }");
        }

        /// <summary>
        /// Runs processes asynchronously and writes the output parameters to the standard output.
        /// Processes should be given at the command line.
        /// </summary>
        /// <param name="args">Names of the process to run asynchronously</param>
        static void Main(string[] args)
        {
            var tasks = new List<Task<JobOutput>>();
            foreach (var processName in args)
            {
                tasks.Add(RunAsync(processName));
            }
            while (tasks.Count > 0)
            {
                var index = Task.WaitAny(tasks.ToArray());
                var task = tasks[index];
                PrintOutput(task.Result);
                tasks.RemoveAt(index);
            }
        }
    }
}
