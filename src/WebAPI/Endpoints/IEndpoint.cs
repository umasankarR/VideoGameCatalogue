namespace WebAPI.Endpoints;

/// <summary>
/// Interface for defining API endpoints
/// Each endpoint class implements this to register its route
/// </summary>
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}

