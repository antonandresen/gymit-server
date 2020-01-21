using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gymit.Contracts.V1.Requests
{
    public class UpdateTestRequest
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }
}
