using Domain.Models;
using Domain.Repositories;
using Infrastructure.DbContexts;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class TableRepository : ITableRepository
    {
        private IWaitlistDbContext _context;

        public TableRepository(IWaitlistDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Table> GetTables(int? partySize, bool? isAvailable)
        {
            var tables = _context.Tables.AsQueryable();
            if (partySize.HasValue)
            {
                tables = tables.Where(table => table.PartySize == partySize.Value);
            }
            if (isAvailable.HasValue)
            {
                tables = tables.Where(table => table.IsAvailable == isAvailable.Value);
            }

            return tables.ToList();
        }

        public void Add(Table table)
        {
            _context.Tables.Add(table);
        }

        public void Save()
        {
            _context.Save();
        }

        public Table GetTable(int tableId)
        {
            return _context.Tables.SingleOrDefault(table => table.Id == tableId);
        }
        
        public void Remove(Table table)
        {
            _context.Tables.Remove(table);
        }
    }
}
