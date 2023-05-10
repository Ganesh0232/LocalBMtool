using BMtool.Core.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMtool.Infrastructure.Data.Context
{
    public class BMtoolContext
    {
        private readonly ConnectionStringOptions _connectionStringOptions;
        public BMtoolContext(IOptionsMonitor<ConnectionStringOptions> optionsMonitor)
        {
            this._connectionStringOptions = optionsMonitor.CurrentValue;
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionStringOptions.SqlConnection);
    }
}
