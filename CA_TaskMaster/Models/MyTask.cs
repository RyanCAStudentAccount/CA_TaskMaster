using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_TaskMaster.Models
{
    // This class represents a Task in the application.
    public class MyTask
    {
        // Unique Identifier for the Task.
        public int TaskId { get; set; }

        // The name of the Task.
        public string TaskName { get; set; }

        // A brief description of the Task.
        public string TaskDescription { get; set; }

        // The priority level of the Task.
        public string TaskPriority { get; set; }

        // The due date for the Task.
        public DateTime TaskDueDate { get; set; }

        // The completion status of the Task, true indicates the task is complete, false indicates it is not.
        public bool TaskCompletionStatus { get; set; }
    }
}