using InstaHashtagUsage.ClassLibrary.Services;
using InstaHashtagUsage.ClassLibrary.Models;

namespace InstaHashtagUsage.ClassLibrary.Mediatr;

public class NewHashtagCountTableHandler : IRequestHandler<NewHashtagCountTableRequest, HashtagCountTable>
{
	private readonly IHastagCountTableFactory _factory;

	public NewHashtagCountTableHandler(IHastagCountTableFactory factory)
    {
		_factory = factory;
	}
    public Task<HashtagCountTable> Handle(NewHashtagCountTableRequest request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_factory.GetNewTable());
	}
}
