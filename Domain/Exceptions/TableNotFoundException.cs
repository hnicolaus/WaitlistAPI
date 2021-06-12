using System;

namespace Domain.Exceptions
{
    public class TableNotFoundException : Exception
    {
        public TableNotFoundException() { }

        public TableNotFoundException(int tableId) : base($"Table with ID {tableId} does not exist.") { }
    }
}