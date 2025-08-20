using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Common;
using CreateReq = Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using GetReq    = Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using CancelReq = Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using AddItem   = Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.AddItem;
using ChgItem   = Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.ChangeItem;
using CxlItem   = Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.CancelItem;
using AppCreate = Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AppGet    = Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using AppCancel = Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using AppAdd    = Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;
using AppChg    = Ambev.DeveloperEvaluation.Application.Sales.Items.ChangeItem;
using AppCxl    = Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/sales")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<CreateReq.CreateSaleResponse>> Create([FromBody] CreateReq.CreateSaleRequest request, CancellationToken ct)
    {
        var cmd = _mapper.Map<AppCreate.CreateSaleCommand>(request);
        var result = await _mediator.Send(cmd, ct);
        var response = _mapper.Map<CreateReq.CreateSaleResponse>(result);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetReq.GetSaleResponse>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new AppGet.GetSaleCommand(id), ct);
        return Ok(_mapper.Map<GetReq.GetSaleResponse>(result));
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<ActionResult<CancelReq.CancelSaleResponse>> Cancel([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new AppCancel.CancelSaleCommand(id), ct);
        return Ok(_mapper.Map<CancelReq.CancelSaleResponse>(result));
    }

    [HttpPost("{id:guid}/items")]
    public async Task<ActionResult<AddItem.AddItemResponse>> AddItem([FromRoute] Guid id, [FromBody] AddItem.AddItemRequest body, CancellationToken ct)
    {
        var cmd = _mapper.Map<AppAdd.AddItemCommand>(body with { SaleId = id });
        var result = await _mediator.Send(cmd, ct);
        return Ok(_mapper.Map<AddItem.AddItemResponse>(result));
    }

    [HttpPut("{id:guid}/items/{itemId:guid}")]
    public async Task<ActionResult<ChgItem.ChangeItemResponse>> ChangeItem([FromRoute] Guid id, [FromRoute] Guid itemId, [FromBody] ChgItem.ChangeItemRequest body, CancellationToken ct)
    {
        var cmd = _mapper.Map<AppChg.ChangeItemCommand>(body with { SaleId = id, ItemId = itemId });
        var result = await _mediator.Send(cmd, ct);
        return Ok(_mapper.Map<ChgItem.ChangeItemResponse>(result));
    }

    [HttpPost("{id:guid}/items/{itemId:guid}/cancel")]
    public async Task<ActionResult<CxlItem.CancelItemResponse>> CancelItem([FromRoute] Guid id, [FromRoute] Guid itemId, CancellationToken ct)
    {
        var result = await _mediator.Send(new AppCxl.CancelItemCommand(id, itemId), ct);
        return Ok(_mapper.Map<CxlItem.CancelItemResponse>(result));
    }
}
