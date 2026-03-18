using Employee_Hotline.Application.Interfaces;
using Employee_Hotline.Domain.Entities;
using Employee_Hotline.Domain.Exceptions;
using MediatR;
using System.Globalization;

namespace Employee_Hotline.Application.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, CreateEmployeeResult>
{
    private readonly IEmployeeRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateEmployeeCommandHandler(
        IEmployeeRepository repo, IUnitOfWork uow)
        => (_repo, _uow) = (repo, uow);

    public async Task<CreateEmployeeResult> Handle(
    CreateEmployeeCommand req, CancellationToken ct)
    {
        var incomingNames = req.Items.Select(i => i.Name).ToHashSet();
        var existingNames = await _repo.GetExistingNamesAsync(incomingNames, ct);

        var successes = new List<Employee>();
        var failures = new List<CreateEmployeeFailure>();

        foreach (var (item, idx) in req.Items.Select((x, i) => (x, i)))
        {
            try
            {
                // 조회 조건이 이름이기 때문에, 이름 중복 예외 처리
                if (existingNames.Contains(item.Name))
                    throw new ConflictException($"이미 존재하는 이름입니다: {item.Name}");

                // 같은 요청 내 중복 확인
                if (successes.Any(e => e.Name == item.Name))
                    throw new ConflictException($"요청 내 중복된 이름입니다: {item.Name}");

                var joined = DateOnly
                    .ParseExact(item.Joined, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                    .ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

                var employee = Employee.Create(
                    name: item.Name,
                    email: item.Email,
                    tel: item.Tel,
                    joinedAt: joined
                );

                successes.Add(employee);
            }
            catch (Exception ex)
            {
                failures.Add(new CreateEmployeeFailure(idx + 1, item.Name, ex.Message));
            }
        }

        if (successes.Count > 0)
        {
            await _repo.AddRangeAsync(successes, ct);
            await _uow.SaveChangesAsync(ct);
        }

        return new CreateEmployeeResult(
            successes.Count,
            failures.Count,
            failures.AsReadOnly()
        );
    }
}