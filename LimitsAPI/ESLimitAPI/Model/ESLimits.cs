using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ESLimitAPI.Model
{
    public partial class ESLimits : DbContext
    {
        public ESLimits(DbContextOptions<ESLimits> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Limit>();
            modelBuilder.Entity<Document>();
            modelBuilder.Entity<LimitType>();
            modelBuilder.Entity<Source>();
            modelBuilder.Entity<CheckLimit>();
            modelBuilder.Entity<Transaction>();
        }

        public IQueryable<Document> GetDocument(int? id) 
        {
            var documentID = new SqlParameter { ParameterName = "@p_id", DbType = DbType.Int32, Value = (object)id ?? DBNull.Value };
            return this.Set<Document>().FromSql(@"[dbo].[get_documents] @p_id", documentID);
        }

        public IQueryable<Limit> GetLimit(int? id) 
        {
            var parameter = new SqlParameter { ParameterName = "@p_id", DbType = DbType.Int32, IsNullable = true, Value = (object)id ?? DBNull.Value };
            return this.Set<Limit>().FromSql(@"[dbo].[get_limits] @p_id", parameter);
        }

        public async Task UpdateLimit(LimitView limit) 
        {
            var parameters = new SqlParameter[] {
                new SqlParameter { ParameterName = "@p_user_id_post", DbType = DbType.Int32, Value = limit.UserIdPost },
                new SqlParameter { ParameterName = "@p_name", DbType = DbType.String, Value = limit.Name },
                new SqlParameter { ParameterName = "@p_limit", DbType = DbType.Decimal, Value = limit.AmountLimit },
                new SqlParameter { ParameterName = "@p_limit_type", DbType = DbType.Int32, Value = limit.LimitType },
                new SqlParameter { ParameterName = "@p_id", DbType = DbType.Int32, Value = limit.Id },
                new SqlParameter { ParameterName = "@p_active", DbType = DbType.Int32, Value = limit.Active },
                new SqlParameter { ParameterName = "@p_column_check", DbType = DbType.String, Value = limit.ColumnCheck },
                new SqlParameter { ParameterName = "@p_date_expired", DbType = DbType.DateTime, Value = limit.DateExpired }
             };

             await this.Database.ExecuteSqlCommandAsync(@"[dbo].[add_update_limits] @p_id, @p_name, @p_limit, @p_limit_type, @p_active, @p_column_check, @p_date_expired, @p_user_id_post", parameters);
        }

        public IQueryable<Source> GetSource(string id)
        {
            var sourceId = new SqlParameter { ParameterName = "@p_id", DbType = DbType.String, Value = (object)id ?? DBNull.Value };
            return this.Set<Source>().FromSql("[dbo].[get_sources] @p_id", sourceId);
        }

        public async Task AddSource(Source source)
        {
            var sourceId = new SqlParameter { ParameterName = "@p_id", DbType = DbType.String, Value = source.Id };
            var sourceName = new SqlParameter { ParameterName = "@p_name", DbType = DbType.String, Value = source.Name };

            await this.Database.ExecuteSqlCommandAsync(@"[dbo].[add_sources] @p_id, @p_name", sourceId, sourceName);
        }     

        public async Task SetTransaction(LimitTransaction transaction) 
        {
            var parameters = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName = "@p_external_id", DbType = DbType.Int32, Value = transaction.ExternalId },
                new SqlParameter() { ParameterName = "@p_source", DbType = DbType.String, Value = transaction.Source },
                new SqlParameter() { ParameterName = "@p_amount", DbType = DbType.Decimal, Value = transaction.Amount },
                new SqlParameter() { ParameterName = "@p_okpoa", DbType = DbType.String, Value = (object)transaction.OkpoA ?? DBNull.Value },
                new SqlParameter() { ParameterName = "@p_documenta_type", DbType = DbType.Int32, Value = (object)transaction.DocumentTypeA ?? DBNull.Value },
                new SqlParameter() { ParameterName = "@p_documenta", DbType = DbType.String, Value = (object)transaction.DocumentA ?? DBNull.Value },
                new SqlParameter() { ParameterName = "@p_okpob", DbType = DbType.String, Value = (object)transaction.OkpoB ?? DBNull.Value },
                new SqlParameter() { ParameterName = "@p_documentb_type", DbType = DbType.Int32, Value = (object)transaction.DocumentTypeB ?? DBNull.Value },
                new SqlParameter() { ParameterName = "@p_documentb", DbType = DbType.String, Value = (object)transaction.DocumentB ?? DBNull.Value },
                new SqlParameter() { ParameterName = "@p_phone", DbType = DbType.String, Value = (object)transaction.Phone ?? DBNull.Value},
                new SqlParameter() { ParameterName = "@p_account", DbType = DbType.String, Value = (object)transaction.Account ?? DBNull.Value }
            };

            await this.Database.ExecuteSqlCommandAsync(@"[dbo].[set_transaction] @p_external_id, @p_source, @p_amount, @p_okpoa, @p_documenta_type,
                @p_documenta, @p_okpob, @p_documentb_type, @p_documentb, @p_phone, @p_account", parameters); 
        }

        public IQueryable<CheckLimit> CheckLimits(int limitType, decimal amount, DateTime? dateFrom, DateTime? dateTo, string whereColumn, string whereValue) 
        {
            var parametrs = new SqlParameter[] 
            {
                new SqlParameter { ParameterName = "@p_limit_type", DbType = DbType.Int32, Value = limitType },
                new SqlParameter { ParameterName = "@p_amount", DbType = DbType.Decimal, Value = amount },
                new SqlParameter { ParameterName = "@p_where_column", DbType = DbType.String, Value = whereColumn },
                new SqlParameter { ParameterName = "@p_where_value", DbType = DbType.String, Value = whereValue },
                new SqlParameter { ParameterName = "@date_from", DbType = DbType.DateTime, Value = (object)dateFrom ?? DBNull.Value },
                new SqlParameter { ParameterName = "@date_to", DbType = DbType.DateTime, Value = (object)dateTo ?? DBNull.Value },
            };

            return this.Set<CheckLimit>().FromSql(@"[dbo].[check_limits] @p_limit_type, @date_from, @date_to, 
                @p_amount, @p_where_column, @p_where_value", parametrs);
        }

        public IQueryable<LimitType> GetLimitTypes()
        {
            return this.Set<LimitType>().FromSql("[dbo].[get_limit_types]");
        }

        public async Task UpdateSource(Source source)
        {
            var parametrs = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName = "@p_name", DbType = DbType.String, Value = source.Name },
                new SqlParameter() { ParameterName = "@p_id", DbType = DbType.String, Value = source.Id },
            };

            await this.Database.ExecuteSqlCommandAsync("[dbo].[update_sources] @p_id, @p_name", parametrs);
        }

        public async Task CancelTransaction(int externalId, string source)
        {
            var parameters = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName = "@external_id", DbType = DbType.Int32, Value = externalId },
                new SqlParameter() { ParameterName = "@source", DbType = DbType.String, Value = source }
            };
            await this.Database.ExecuteSqlCommandAsync("[dbo].[cancel_transaction] @external_id, @source", parameters);
        }
    }
}
