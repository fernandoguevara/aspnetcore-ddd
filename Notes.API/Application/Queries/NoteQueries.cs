using Dapper;
using Notes.Domain.AggregatesModel;
using Notes.Infrastructure;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.API.Application.Queries
{
    public class NoteQueries : INoteQueries
    {
        private string _connectionString = String.Empty;

        public NoteQueries(string connectionString)
        {
            _connectionString = !String.IsNullOrWhiteSpace(connectionString) ? connectionString :
                throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<NoteDTO> GetNoteAsync(Guid id)
        {
            string sql = "SELECT * FROM \"Note\" WHERE \"Id\" = @id";

            using (var conn = DBConnectionFactory.MakeDBConnection(_connectionString))
            {
                conn.Open();

                var result = await conn.QuerySingleAsync<NoteDTO>(sql, new { id = id } );

                if (result == null)
                {
                    throw new KeyNotFoundException();
                }

                return result;
            }
        }

        public async Task<IEnumerable<NoteDTO>> GetNotesAsync()
        {
            string sql = "SELECT * FROM \"Note\"";

            using (var conn = DBConnectionFactory.MakeDBConnection(_connectionString))
            {
                conn.Open();
                return await conn.QueryAsync<NoteDTO>(sql);
            }
        }
    }

}
