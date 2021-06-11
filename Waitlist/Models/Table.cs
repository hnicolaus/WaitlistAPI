using DomainTable = Domain.Models.Table;

namespace Api.Models
{
    public class Table
    {
        public Table(DomainTable domainTable)
        {
            Id = domainTable.Id;
            Number = domainTable.Number;
            PartySize = domainTable.PartySize;
            IsAvailable = domainTable.IsAvailable;
        }

        public int Id { get; set; }
        public int Number { get; set; }
        public int PartySize { get; set; }
        public bool IsAvailable { get; set; }
    }
}
