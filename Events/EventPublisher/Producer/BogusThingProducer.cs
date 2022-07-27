using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPublisher.Producer
{
    using Bogus;
    using Events;

    public class BogusThingProducer
    {

        public Task<IEnumerable<Thing>> Generate(int count, CancellationToken stoppingToken)
        {
            long id = 1;
            var producer = new Faker<Thing>().StrictMode(true).RuleFor(f => f.Id, () => id++)
                .RuleFor(f => f.Name, (name, f) => name.Commerce.Product())
                .RuleFor(f => f.TypeOfThing, (name, f) => name.Commerce.ProductMaterial())
                .RuleFor(f => f.LocationOfThing, (name, f) => name.Commerce.Department(count))
                .RuleFor(f => f.CreatedDate, () => DateTime.UtcNow).RuleFor(f => f.UpdatedDate, () => DateTime.UtcNow);
            return Task.FromResult(producer.GenerateLazy(count));
        }
    }
}
