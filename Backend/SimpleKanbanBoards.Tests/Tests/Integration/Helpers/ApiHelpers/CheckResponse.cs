using FluentAssertions;
using SimpleKanbanBoards.Business.Models;

namespace SimpleKanbanBoards.Tests.Integration.Helpers.ApiHelpers;

public static class CheckResponse
{
    public static void Succeeded<T>(ApiResult<T> result)
    {
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Result.Should().NotBe(default(T));
    }
    public static void Failure<T>(ApiResult<T> result)
    {
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThan(0);
        result.Result.Should().Be(default(T));
    }
}
