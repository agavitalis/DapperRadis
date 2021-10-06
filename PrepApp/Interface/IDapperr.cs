using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PrepApp.Interface
{
    public interface IDapperr
    {
        IEnumerable<T> Query<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
        int Execute(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
    }
}
