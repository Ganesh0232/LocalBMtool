using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMtool.Core.Entities
{
    public class ConnectionStringOptions
    {
        public const string Position = "ConnectionStrings";
        public string? SqlConnection { get; set; }
    }
}
