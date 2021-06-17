using Dapper;
using Microsoft.Extensions.Options;
using Notes.BackgroundTasks.Models;
using Notes.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.BackgroundTasks.Queries
{
    public class NoteQueries : INoteQueries
    {
        private readonly BackgroundTaskSettings _settings;
        public NoteQueries(IOptions<BackgroundTaskSettings> settings)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }
        public async Task<IEnumerable<Note>> GetNotesAsync()
        {
            string sql = "SELECT * FROM \"Note\" ";

            using (var conn = DBConnectionFactory.MakeDBConnection(_settings.ConnectionString))
            {
                conn.Open();
                return await conn.QueryAsync<Note>(sql);
            }
        }

        public async Task<IEnumerable<Note>> GetNoteAsync(int id)
        {
            string sql = "SELECT * FROM \"Note\" WHERE Id = @id";

            using (var conn = DBConnectionFactory.MakeDBConnection(_settings.ConnectionString))
            {
                conn.Open();
                return await conn.QueryAsync<Note>(sql, new { id });
            }
        }


    }
}
