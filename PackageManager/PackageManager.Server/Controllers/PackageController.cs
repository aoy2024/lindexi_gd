using Microsoft.AspNetCore.Mvc;

namespace PackageManager.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class PackageController : ControllerBase
{
    /// <summary>
    /// 获取包下载或更新
    /// </summary>
    /// <param name="request"></param>
    /// <returns>
    /// 返回给定的包的最新版本，最新版本指的是传入的参数所能支持的最新版本
    /// </returns>
    [HttpGet]
    public IActionResult Get([FromQuery] GetPackageRequest? request)
    {


        return Ok(new GetPackageResponse($"Hello Version={request}"));
    }

    // 列举首页的所有包

    /// <summary>
    /// 推送包
    /// </summary>
    [HttpPut]
    public IActionResult Put([FromBody] PutPackageRequest request)
    {
        if (HttpContext.Request.Headers.TryGetValue("Token",out var value) && string.Equals(value.ToString(),TokenConfiguration.Token,StringComparison.Ordinal))
        {
            // 证明有权限可以推送
        }

        return NotFound();
    }
}

public static class TokenConfiguration
{
    public const string Token = "B44A0A9E-D6C8-434F-9215-894A30BC5674";
}

public record PutPackageRequest();

