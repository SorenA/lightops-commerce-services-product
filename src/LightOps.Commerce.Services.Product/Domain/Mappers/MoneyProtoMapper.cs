using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Mappers;
using NodaMoney;

namespace LightOps.Commerce.Services.Product.Domain.Mappers
{
    public class MoneyProtoMapper : IMapper<Money, MoneyProto>
    {
        public MoneyProto Map(Money src)
        {
            var units = decimal.ToInt64(src.Amount);
            var nanoUnits = decimal.ToInt32((src.Amount - units) * 1_000_000_000);

            return new MoneyProto
            {
                CurrencyCode = src.Currency.Code,
                Units = units,
                Nanos = nanoUnits,
            };
        }
    }
}