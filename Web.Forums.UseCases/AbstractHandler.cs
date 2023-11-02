namespace Web.Forums.UseCases;


public abstract class AbstractHandler<TRequest, TResponse>
(
	IValidator<TRequest> validator
)
	: IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

	Task<TResponse> IRequestHandler<TRequest, TResponse>.Handle(TRequest request, CancellationToken cancellationToken)
	{
		validator.ValidateAndThrow(request);
		return Handle(request, cancellationToken);
	}


	public virtual Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

}