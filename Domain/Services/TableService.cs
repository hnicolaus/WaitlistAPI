using Domain.Models;
using Domain.Repositories;
using Domain.Requests;
using System.Collections.Generic;

namespace Domain.Services
{
    public class TableService
    {
        public ITableRepository _tableRepository;

        public TableService(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        public Table CreateTable(CreateTableRequest request)
        {
            if (request.Number < 0)
            {
                throw new InvalidRequestException("Table number cannot be a negative number.");
            }
            if (request.PartySize <= 0)
            {
                throw new InvalidRequestException("Table's supported PartySize must be bigger than 0.");
            }

            var table = new Table(request);
            _tableRepository.Add(table);
            SaveChanges();

            return table;
        }

        public IEnumerable<Table> GetTables(int? partySize, bool? isAvailable)
        {
            return _tableRepository.GetTables(partySize, isAvailable);
        }

        public Table GetTable(int tableId)
        {
            var table = _tableRepository.GetTable(tableId);
            if (table == null)
            {
                throw new TableNotFoundException(tableId);
            }

            return table;
        }

        public void DeleteTable(int tableId)
        {
            var table = _tableRepository.GetTable(tableId);
            if (table == null)
            {
                throw new TableNotFoundException(tableId);
            }

            _tableRepository.Remove(table);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _tableRepository.Save();
        }
    }
}
