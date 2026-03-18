namespace Employee_Hotline.Domain.Exceptions;

public sealed class ConflictException(string message) : Exception(message);