using System.ComponentModel.DataAnnotations;
using DomainCreateTableRequest = Domain.Requests.CreateTableRequest;

namespace Api.Requests
{
    public class CreateTableRequest
    {
        [Required]
        public int Number { get; set; }
        [Required]
        public int PartySize { get; set; }

        public DomainCreateTableRequest ToDomain()
        {
            return new DomainCreateTableRequest(Number, PartySize);
        }
    }
}