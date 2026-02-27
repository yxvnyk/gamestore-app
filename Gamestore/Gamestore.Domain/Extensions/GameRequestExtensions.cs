using Gamestore.Domain.Helpers;
using Gamestore.Domain.Models.DTO.Game;

namespace Gamestore.Domain.Extensions;

public static class GameRequestExtensions
{
    public static GetGamesRequest ToGetGameRequest(this GetGamesApiRequest apiRequest)
    {
        ArgumentNullException.ThrowIfNull(apiRequest);

        return new GetGamesRequest
        {
            Page = PaginationOptionsHelper.GetValidCurrentPage(apiRequest.Page),
            PageSize = PaginationOptionsHelper.GetValidPageSize(apiRequest.PageCount),

            Name = apiRequest.Name,
            MinPrice = apiRequest.MinPrice,
            MaxPrice = apiRequest.MaxPrice,
            Genres = apiRequest.Genres,
            DatePublishing = apiRequest.DatePublishing,
            Platforms = apiRequest.Platforms,
            Publishers = apiRequest.Publishers,
            Sort = apiRequest.Sort,
            Trigger = apiRequest.Trigger,
        };
    }
}