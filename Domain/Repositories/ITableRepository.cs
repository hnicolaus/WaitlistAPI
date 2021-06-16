using Domain.Models;
using System.Collections.Generic;

namespace Domain.Repositories
{
    public interface ITableRepository
    {
        IEnumerable<Table> GetTables(int? partySize, bool? isAvailable);
        void Add(Table table);
        void Save();
        Table GetTableById(int tableId);
        Table GetTableByNumber(int tableNumber);
        void Remove(Table table);
    }
}
