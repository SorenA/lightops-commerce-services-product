using LightOps.Commerce.Proto.Services.Product.V1;
using LightOps.Mapping.Api.Mappers;
using NodaMoney;

namespace LightOps.Commerce.Services.Product.Domain.Mappers.V1
{
    public class ProtoMoneyMapper : IMapper<Money, ProtoMoney>
    {
        public ProtoMoney Map(Money source)
        {
            var dest = new ProtoMoney();

            var units = decimal.ToInt64(source.Amount);
            var nanoUnits = decimal.ToInt32((source.Amount - units) * 1_000_000_000);

            dest.CurrencyCode = source.Currency.Code;
            dest.Units = units;
            dest.Nanos = nanoUnits;

            return dest;
        }
    }
}