using Api.Helpers;
using Api.Models;
using Api.Requests;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using DomainTable = Domain.Models.Table;

namespace Api.Controllers
{
    //[Authorize(Roles = "Admin")]
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
        [MapException(typeof(InvalidRequestException), HttpStatusCode.BadRequest)]
        public IActionResult GetTables(int? partySize, bool? isAvailable)
        {
            var domainTables = _tableService.GetTables(partySize, isAvailable);

            var apiTables = domainTables.Select(domainTable => new Table(domainTable));
            return Ok(apiTables);
        }

        [HttpPost]
        [MapException(typeof(InvalidRequestException), HttpStatusCode.BadRequest)]
        public IActionResult CreateTable([FromBody] CreateTableRequest request)
        {
            var domainRequest = request.ToDomain();
            _tableService.CreateTable(domainRequest);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [MapException(typeof(TableNotFoundException), HttpStatusCode.NotFound)]
        public IActionResult DeleteTable(int id)
        {
            _tableService.DeleteTable(id);

            return Ok();
        }

        //NOTE: In the request header, clients have to specify Content-Type: application/json-patch+json.
        //Otherwise, JsonPatchDocument serialization will fail.
        [HttpPatch]
        [Route("{id}")]
        [MapException(typeof(TableNotFoundException), HttpStatusCode.NotFound)]
        [MapException(typeof(InvalidRequestException), HttpStatusCode.BadRequest)]
        public IActionResult UpdateTable(int id, [FromBody] JsonPatchDocument<DomainTable> patchDoc)
        {
            var domainTable = _tableService.GetTable(id);

            Validate<DomainTable>.EmptyPatchRequest(patchDoc);
            Validate<DomainTable>.PatchRestrictedFields(patchDoc, _allowedPatchFields);

            var isValidTableNumberRequest = ValidateUpdateTableNumberRequest(domainTable, patchDoc);
            var isValidTableSizeRequest = ValidateUpdateTableSizeRequest(domainTable, patchDoc);

            //isSaveRequired = ValidateA() || ValidateB() - patchDoc.Operations.Remove() in ValidateB() will not get executed if ValidateA() returns true.
            var isSaveRequired = isValidTableNumberRequest || isValidTableSizeRequest;

            patchDoc.ApplyTo(domainTable, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (isSaveRequired)
            {
                _tableService.SaveChanges();
            }

            return Ok();
        }

        private bool ValidateUpdateTableNumberRequest(DomainTable table, JsonPatchDocument<DomainTable> patchDoc)
        {
            var tableNumberPath = DomainTable.PropNameToPatchPath[nameof(DomainTable.Number)];
            var tableNumberRequest = patchDoc.Operations.SingleOrDefault(o => o.path.Equals(tableNumberPath));
            var isTableNumberRequest = tableNumberRequest != null;
            if (!isTableNumberRequest)
            {
                return false;
            }

            var requestedTableNumber = Convert.ToInt32(tableNumberRequest.value);
            if (requestedTableNumber <= 0)
            {
                throw new InvalidRequestException("Table number cannot be 0 or a negative number.");
            }
            if (requestedTableNumber == table.Number)
            {
                patchDoc.Operations.Remove(tableNumberRequest);
                return false;
            }

            _tableService.ValidateTableNumberUnique(requestedTableNumber);
            return true;
        }

        private static bool ValidateUpdateTableSizeRequest(DomainTable table, JsonPatchDocument<DomainTable> patchDoc)
        {
            var tableSizePath = DomainTable.PropNameToPatchPath[nameof(DomainTable.PartySize)];
            var tableSizeRequest = patchDoc.Operations.SingleOrDefault(o => o.path.Equals(tableSizePath));
            var isTableSizeRequest = tableSizeRequest != null;
            if (!isTableSizeRequest)
            {
                return false;
            }

            var requestedTableSize = Convert.ToInt32(tableSizeRequest.value);
            if (requestedTableSize <= 0)
            {
                throw new InvalidRequestException("Table's supported PartySize must be bigger than 0.");
            }
            if (requestedTableSize == table.PartySize)
            {
                patchDoc.Operations.Remove(tableSizeRequest);
                return false;
            }

            return true;
        }
    }
}