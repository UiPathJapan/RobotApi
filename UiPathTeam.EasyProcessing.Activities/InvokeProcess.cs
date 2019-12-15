using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UiPath.Robot.Api;

namespace UiPathTeam.EasyProcessing.Activities
{
    [DisplayName("Invoke Process")]
    public class InvokeProcess : AsyncCodeActivity
    {
        [Category("Input")]
        [DisplayName("Process Name")]
        public InArgument<string> ProcessName { get; set; }

        [Category("Input")]
        [DisplayName("Input Values")]
        public InArgument<IDictionary<string, object>> InputValues { get; set; }

        [Category("Output")]
        [DisplayName("Output Values")]
        public OutArgument<IReadOnlyDictionary<string, object>> OutputValues { get; set; }

        private delegate IReadOnlyDictionary<string, object> RunProcessDelegate(string processName, IDictionary<string, object> inputValues);

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            var processName = ProcessName?.Get(context);
            if (string.IsNullOrEmpty(processName))
            {
                throw new ArgumentException("No process name was given.");
            }
            var inputValues = InputValues?.Get(context);
            var runProcess = new RunProcessDelegate(RunProcess);
            context.UserState = runProcess;
            return runProcess.BeginInvoke(processName, inputValues, callback, state);
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            var runProcess = (RunProcessDelegate)context.UserState;
            var outputValues = runProcess.EndInvoke(result);
            OutputValues.Set(context, outputValues);
        }

        private IReadOnlyDictionary<string, object> RunProcess(string processName, IDictionary<string, object> inputValues)
        {
            var client = new RobotClient();
            var task1 = client.GetProcesses();
            task1.Wait();
            var processes = task1.Result;
            var myProcess = processes.Single(process => process.Name == processName);
            var job = myProcess.ToJob();
            job.InputArguments = inputValues;
            var task2 = client.RunJob(job);
            task2.Wait();
            return task2.Result.Arguments;
        }
    }
}
