using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPMethod.DataModel.DTOs
{
    public class CPMTask
    {
        public int duration { get; set; }
        public IEnumerable<string> previous { get; set; } = Enumerable.Empty<string>();
    }
}
