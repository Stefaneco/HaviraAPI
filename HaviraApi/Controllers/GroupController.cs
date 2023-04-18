using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaviraApi.Models;
using HaviraApi.Models.Request;
using HaviraApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HaviraApi.Controllers;

[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpPost]
    public ActionResult CreateGroup([FromBody] CreateGroupRequest dto) {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        if (clientId is null) return Unauthorized();
        var group = _groupService.CreateGroup(dto, clientId);
        return Created(group.Id.ToString(), group);
    }

    [HttpGet]
    public ActionResult GetAllGroupsOfUser() {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        if (clientId is null) return Unauthorized();
        var groups = _groupService.GetGroupsByUserId(clientId);
        return Ok(groups);
    }

    [HttpGet("{id}")]
    public ActionResult GetGroupById([FromRoute] int id) {
        var group = _groupService.GetGroupById(id);
        return Ok(group);
    }

    [HttpGet("join/{joinCode}")]
    public ActionResult JoinGroup([FromRoute] string joinCode) {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        if (clientId is null) return Unauthorized();
        var group = _groupService.JoinGroup(joinCode, clientId);
        return Ok(group);
    }

    [HttpPut("{groupId}/leave")]
    public ActionResult LeaveGroup([FromRoute] long groupId) {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        if (clientId is null) return Unauthorized();
        _groupService.LeaveGroup(groupId, clientId);
        return Ok();
    }

    [HttpDelete("{groupId}")]
    public ActionResult DeleteGroup([FromRoute] long groupId) {
        var clientId = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].ToString();
        if (clientId is null) return Unauthorized();
        _groupService.DeleteGroup(groupId, clientId);
        return Ok();
    }
}

