namespace InstaHashtagUsage.ClassLibrary.Mediatr;
public record LoginRequest(string Login, string Password) : IRequest<bool>;
