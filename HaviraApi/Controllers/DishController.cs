using System;
using HaviraApi.Models.Request;
using HaviraApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HaviraApi.Controllers;

[Route("api/[controller]")]
public class DishController : ControllerBase
{
	private readonly IDishService _dishService;

	public DishController(IDishService dishService)
	{
		_dishService = dishService;
	}

	[HttpPost]
	public ActionResult CreateDish([FromBody] CreateDishRequest request) {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        if (clientId is null) return Unauthorized();
        var createdDish = _dishService.CreateDish(request, clientId);
		return Ok(createdDish);
	}

	[HttpGet("{id}")]
	public ActionResult GetDish([FromRoute] long id) {
		var dish = _dishService.GetDish(id);
		return Ok(dish);
	}

	[HttpPost("{dishId}/DishPrep")]
	public ActionResult CreateDishPrep([FromBody] CreateDishPrepRequest request, [FromRoute] long dishId)
    {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        if (clientId is null) return Unauthorized();
		var createdDishPrep = _dishService.CreateDishPrep(request, clientId, dishId);
        return Created(createdDishPrep.Id.ToString(), createdDishPrep);
    }

	[HttpPut("{dishId}")]
	public ActionResult UpdateDish([FromBody] UpdateDishRequest request,[FromRoute] long dishId) {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        if (clientId is null) return Unauthorized();
		var updatedDish = _dishService.UpdateDish(request, dishId, clientId);
		return Ok(updatedDish);
    }

	[HttpDelete("{dishId}")]
    public ActionResult DeleteDish([FromRoute] long dishId) {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        if (clientId is null) return Unauthorized();
		_dishService.DeleteDish(dishId, clientId);
		return Ok();
    }
}

