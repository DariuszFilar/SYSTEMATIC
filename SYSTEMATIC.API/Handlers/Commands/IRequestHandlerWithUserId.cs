namespace SYSTEMATIC.API.Handlers.Commands
{
    public interface IRequestWithUserIdHandler<in TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, long userId);
    }
}
