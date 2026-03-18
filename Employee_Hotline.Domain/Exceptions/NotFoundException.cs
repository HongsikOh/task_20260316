namespace Employee_Hotline.Domain.Exceptions;

public sealed class NotFoundException(string name, object key)
    : Exception($"{name} (name: {key}) 를 찾을 수 없습니다.");