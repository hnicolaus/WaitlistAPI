using Api.Models;
using Api.Requests;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using DomainTable = Domain.Models.Table;

namespace Api.Controllers
{
    [ApiController]
    [Route("tables")]
    public class TableController : ControllerBase
    {
        public readonly TableService _tableService;
        private readonly string[] _allowedPatchFields;

        public TableController(TableService TableService)
        {
            _tableService = TableService;
            _allowedPatchFields = DomainTable.PropNameToPatchPath.Values.ToArray();
        }

        [HttpGet]
        [MapException(new[] { typeof(InvalidRequestException) }, new[] { HttpStatusCode.BadRequest })]
        public IActionResult GetTables(int? partySize, bool? isAvailable)
        {
            var domainTables = _tableService.GetTables(partySize, isAvailable);

            var apiTables = domainTables.Select(domainTable => new Table(domainTable));
            return Ok(apiTables);
        }

        [HttpPost]
        [MapException(new[] { typeof(InvalidRequestException) }, new[] { HttpStatusCode.BadRequest })]
        public IActionResult CreateTable([FromBody] CreateTableRequest request)
        {
            var domainRequest = request.ToDomain();
            _tableService.CreateTable(domainRequest);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [MapException(new[] { typeof(TableNotFoundException) }, new[] { HttpStatusCode.NotFound })]
        public IActionResult DeleteTable(int id)
        {
            _tableService.DeleteTable(id);

            return Ok();
        }

        //NOTE: In the request header, clients have to specify Content-Type: application/json-patch+json.
        //Otherwise, JsonPatchDocument serialization will fail.
        [HttpPatch]
        [Route("{id}")]
        [MapException(new[] { typeof(TableNotFoundException), typeof(InvalidRequestException) },
            new[] { HttpStatusCode.NotFound, HttpStatusCode.BadRequest })]
        public IActionResult UpdateTable(int id, [FromBody] JsonPatchDocument<DomainTable> patchDoc)
        {
            var domainTable = _tableService.GetTable(id);

            if (patchDoc == null || !patchDoc.Operations.Any())
            {
                throw new InvalidRequestException("Request cannot be empty.");
            }

            var requestedFields = patchDoc.Operations.Select(o => o.path);
            var restrictedFields = requestedFields.Where(field => !_allowedPatchFields.Contains(field));
            if (restrictedFields.Any())
            {
                throw new InvalidRequestException("Cannot update the following restricted field(s): " + string.Join(", ", restrictedFields));
            }

            patchDoc.ApplyTo(domainTable, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _tableService.SaveChanges();
            return Ok();
        }
    }
}