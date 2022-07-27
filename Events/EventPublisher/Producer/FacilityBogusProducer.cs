using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPublisher.Producer
{
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Events;
    using Bogus;
    using Bogus.DataSets;

    public class FacilityBogusProducer
    {
        public FacilityBogusProducer()
        {

        }
        public Task<IEnumerable<Facility>> Generate(int count, CancellationToken stoppingToken)
        {
            ReadOnlyCollection<TimeZoneInfo> timeZones;
            timeZones = TimeZoneInfo.GetSystemTimeZones();
            long id = 1;
            var producer = new Faker<Facility>().StrictMode(true)
                    .RuleFor(f => f.Id, () => id++)
                .RuleFor(f => f.Name, (name, f) => name.Person.FullName)
                .RuleFor(
                    f => f.Timezone,
                    (f) => f.PickRandom(timeZones).ToString())
                .RuleFor(
                    f => f.AddressLine1,
                    f => $" {f.Address.BuildingNumber()} {f.Address.StreetAddress(false)}")
                .RuleFor(f => f.AddressLine2, f => f.Address.SecondaryAddress())
                .RuleFor(f => f.State, f => f.Address.State()).RuleFor(f => f.Country, f => f.Address.Country())
                .RuleFor(f => f.ZipCode, f => f.Address.ZipCode())
                .RuleFor(f => f.Region, f => f.Address.CardinalDirection(false))
                .RuleFor(f => f.CreatedDate, () => DateTime.UtcNow).RuleFor(
                    f => f.UpdatedDate,
                    () => DateTime.UtcNow)
                ;

            return Task.FromResult(producer.GenerateLazy(count));

        }
    }
}
