using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace Web.Forums.Infrastructure.EntityFrameworkCore.Convertors;

internal class UlidToGuidConvertor : ValueConverter<Ulid, TKey>
{
	public UlidToGuidConvertor() : base(UlidToGuid, GuidToUlid)
	{

	}
	public UlidToGuidConvertor(
		Expression<Func<Ulid, TKey>> convertToProviderExpression,
		Expression<Func<TKey, Ulid>> convertFromProviderExpression,
		ConverterMappingHints? mappingHints = null)
		: base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
	{

	}

	static Expression<Func<Ulid, TKey>> UlidToGuid = x => x.ToGuid();

	static Expression<Func<TKey, Ulid>> GuidToUlid = x => new Ulid(x);
}
