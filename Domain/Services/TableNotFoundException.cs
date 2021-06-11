using System;

namespace Domain.Services
{
    public class TableNotFoundException : Exception
    {
        public TableNotFoundException() { }

        public TableNotFoundException(int tableId) : base($"Table with ID {tableId} does not exist.") { }
    }
}