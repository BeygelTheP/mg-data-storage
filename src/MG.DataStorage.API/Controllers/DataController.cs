using System.Text.Json;
using MG.DataStorage.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("data")]
public class DataController : ControllerBase
{
    private readonly IDataRetrievalService _dataService;
    public DataController(IDataRetrievalService dataService) => _dataService = dataService;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {        
        var data = await _dataService.GetDataAsync(id);
        if (data == null) return NotFound();
        return Ok(data);
    }    
}