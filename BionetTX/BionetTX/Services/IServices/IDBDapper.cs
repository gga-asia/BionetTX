namespace BionetTX.Services.IServices
{
    public interface IDBDapper
    {
        List<T> GetList<T>(string dbname, string sql, object? param = null);

        T Get<T>(string dbname, string sql, object? param = null);
        void Exec(string dbname, string sql, object? datas = null);

        //補充命名
        //List<T> GetEmpExt<T>(string dbname, string sql, object value);
        //object GetList<T>(string dbname, object sql, object value);
    }
}
