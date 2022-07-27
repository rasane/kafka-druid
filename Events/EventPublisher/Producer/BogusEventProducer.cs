using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPublisher.Producer
{
    using System.Collections.Concurrent;
    using Bogus;
    using Events;

    public class BogusEventProducer
    {

        public Task<IEnumerable<ThingEvent>> Generate(int count, IEnumerable<Facility> facilities, IEnumerable<Thing>things,  CancellationToken stoppingToken)
        {
            string[] categories = new string[]
            {
                "AlarmChange", "ValueChange"
            };

            
            var priorities = Enumerable.Range(0, 26).Select(x => (Convert.ToChar('A' + x)).ToString()).ToArray();

            string[] acknowledgementStatus = new string[]
            {
                "NotAcknowledged", "Acknowledged"
            };

            string[] activeStatus = new string[]
            {
                "Active", "ReturnedToNormal"
            };
            long id = 1;
            var producer = new Faker<ThingEvent>().StrictMode(true).RuleFor(f => f.Id, () => id++)
                .RuleFor(f => f.FacilityId, (f) => f.PickRandom<Facility>(facilities).Id)
                .RuleFor(f => f.ThingId, (f) => f.PickRandom<Thing>(things).Id)
                .RuleFor(f => f.EventDescription, (f) => f.Hacker.Phrase())

                .RuleFor(f => f.Category, (f) => f.PickRandom<string>(categories))
                .RuleFor(f => f.Priority, (f) => f.PickRandom<string>(priorities))
                .RuleFor(f => f.AcknowledgementStatus, (f) => f.PickRandom<string>(acknowledgementStatus))
                .RuleFor(f => f.ActiveStatus, (f) => f.PickRandom<string>(activeStatus))
                .RuleFor(f => f.UpdatedDate, () => DateTime.UtcNow).RuleFor(f => f.UpdatedDate, () => DateTime.UtcNow);
            return Task.FromResult(producer.GenerateLazy(count));
        }
    }
}
