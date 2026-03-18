using Employee_Hotline.Application.Commands.CreateEmployee;
using Employee_Hotline.Application.DTOs;
using Employee_Hotline.Tests.Integration.Fixtures;
using Employee_Hotline.Tests.Shared;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Employee_Hotline.Tests.Integration;

public class EmployeesControllerTests(WebAppFactory factory)
    : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    #region 생성

    [Fact]
    public async Task CreateEmployee_InvalidEmail()
    {
        var raw = EmployeeFixtures.CreateEmployeeRawData(email: "invalid-email");

        var response = await _client.PostAsJsonAsync("/api/employee", raw);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task CreateEmployee_ValidRequest()
    {
        var raw = EmployeeFixtures.CreateEmployeeRawData();
        var response = await _client.PostAsJsonAsync("/api/employee", raw);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<CreateEmployeeResult>();

        body!.SuccessCount.Should().Be(1);
    }

    #endregion

    #region 조회

    [Fact]
    public async Task GetByName_ExistingEmployee()
    {
        var raw = EmployeeFixtures.CreateEmployeeRawData(name: "조회테스트1");

        await _client.PostAsJsonAsync("/api/employee", raw);

        var response = await _client.GetAsync("/api/employee/조회테스트1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var employee = await response.Content.ReadFromJsonAsync<EmployeeDto>();
        employee!.Name.Should().Be("조회테스트1");
    }

    [Fact]
    public async Task GetByName_NotExisting()
    {
        var response = await _client.GetAsync("/api/employee/notfound");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetList_ExistingEmployee()
    {
        var raw = EmployeeFixtures.CreateEmployeeRawData(name: "조회테스트2");

        await _client.PostAsJsonAsync("/api/employee", raw);

        var response = await _client.GetAsync("/api/employee?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var employee = await response.Content.ReadFromJsonAsync<EmployeeListDto>();
        employee!.TotalCount.Should().BeGreaterThan(0);
    }

    #endregion
}