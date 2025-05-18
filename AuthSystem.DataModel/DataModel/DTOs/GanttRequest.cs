using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPMethod.DataModel.DTOs
{
    public class GanttRequest
    {
        public Dictionary<string, CPMTask>? tasks { get; set; }
    }
}
