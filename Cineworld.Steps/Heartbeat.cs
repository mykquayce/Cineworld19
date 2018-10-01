using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Cineworld.Steps
{
    public class Heartbeat : StepBody
    {
        private static long _lapNumber = 0;

        private readonly Action<string>[] _messageWriters =
        {
            Console.WriteLine,
            s => System.Diagnostics.Debug.WriteLine(s),
        };

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            if (!(context.Workflow.Data is Models.PersistanceData data))
            {
                return ExecutionResult.Next();
            }

            var lapNumber = Interlocked.Increment(ref _lapNumber);

            var message = $"{lapNumber:D5} {DateTime.Now:O}";


            return ExecutionResult.Next();
        }
    }
}
