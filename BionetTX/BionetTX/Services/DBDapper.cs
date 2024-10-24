using BionetTX.Services.IServices;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BionetTX.Services
{
    public class DBDapper(IConfiguration configuration) : IDBDapper
    {

        public IConfiguration Configuration { get; } = configuration;  // 連到資料庫

        //新增、更新、刪除的方法
        public void Exec(string dbname, string sql, object? datas = null)
        {
            using var connection = new SqlConnection(Configuration.GetConnectionString(dbname));
            connection.Execute(sql, datas);
        }
        // 撈出單筆資料的方法
        public T Get<T>(string dbname, string sql, object? param = null)
        {
            using var connection = new SqlConnection(Configuration.GetConnectionString(dbname));
            return connection.QueryFirstOrDefault<T>(sql, param);
        }

        // 撈出很多筆資料的方法
        public List<T> GetList<T>(string dbname, string sql, object? param = null)
        {
            using var connection = new SqlConnection(Configuration.GetConnectionString(dbname));
            return connection.Query<T>(sql, param).ToList();
        }
    }
}
