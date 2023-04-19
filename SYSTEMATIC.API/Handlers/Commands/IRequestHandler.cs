namespace SYSTEMATIC.API.Handlers.Commands
{
    public interface IRequestHandler<in TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}
