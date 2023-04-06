using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_TaskMaster.Models
{
    public class MyTask
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string TaskPriority { get; set; }
        public DateTime TaskDueDate { get; set; }
        public bool TaskCompletionStatus { get; set; }
    }
}