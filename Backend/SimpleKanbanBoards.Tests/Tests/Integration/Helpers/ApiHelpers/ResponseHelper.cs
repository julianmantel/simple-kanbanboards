using Newtonsoft.Json;
using SimpleKanbanBoards.Business.Models;

namespace SimpleKanbanBoards.Tests.Integration.Helpers.ApiHelpers;
public static class ResponseHelper
{
    public static async Task<ApiResult<T>> GetApiResultAsync<T>(HttpResponseMessage responseMessage)
    {
        var json = await responseMessage.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ApiResult<T>>(json)!;
    }
}
