using Domain.Requests;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Table
    {
        public static readonly Dictionary<string, string> PropNameToPatchPath = new Dictionary<string, string>
        {
            [nameof(Table.Number)] = "/number",
            [nameof(Table.PartySize)] = "/partySize",
            [nameof(Table.IsAvailable)] = "/isAvailable",
        };

        //EF requires empty ctor for model binding
        public Table()
        {

        }

        public Table(CreateTableRequest request)
        {
            Number = request.Number;
            PartySize = request.PartySize;
            IsAvailable = true;
        }

        public int Id { get; set; }
        public int Number { get; set; }
        public int PartySize { get; set; }
        public bool IsAvailable { get; set; }
    }
}
