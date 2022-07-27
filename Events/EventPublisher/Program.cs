// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using EventPublisher.Producer;
using EventPublisher.Publisher;
using Events;


var cancellationTokenSource = new CancellationTokenSource();
ConcurrentDictionary<long, Facility> facilitiesCollection = new ConcurrentDictionary<long, Facility>();
ConcurrentDictionary<long, Thing> thingsCollection = new ConcurrentDictionary<long, Thing>();

var random = new Random();
var tasks = new ConcurrentBag<Task>()
{
    Task.Run(
        async () =>
        {/* Generate facilities */
            
            for (int i = 0; i < 2; i++)
            {
                if (cancellationTokenSource.IsCancellationRequested) break;
                var facilityBogusProducer = new FacilityBogusProducer();
                var facilities = await facilityBogusProducer.Generate(2, cancellationTokenSource.Token);
                foreach (var facility in facilities)
                {
                    facilitiesCollection[facility.Id] = facility;
                }
                Console.WriteLine($"published {i} facilities");

                await Task.Delay(random.Next(3000, 10000 * 60), cancellationTokenSource.Token);
            }
        }/*, cancellationTokenSource.Token*/),
    Task.Run(
        async () =>
        { /* event facilities */
            for (int i = 0; i < 2; i++)
            {
                if (cancellationTokenSource.IsCancellationRequested) break;
                using var kafkaPublisher = new KafkaPublisher("facilities");
                foreach (var (id, facility) in facilitiesCollection)
                {
                    var serializedInput = JsonSerializer.Serialize<Facility>(facility);
                    await kafkaPublisher.Publish(serializedInput);
                }
                Console.WriteLine($"published {i} facility events");

                await Task.Delay(random.Next(5000, 10000), cancellationTokenSource.Token);
            }
        }/*, cancellationTokenSource.Token*/),
    Task.Run(
        async () =>
        {/* Generate things */
            for (int i = 0; i < 2; i++)
            {
                if (cancellationTokenSource.IsCancellationRequested) break;
                var bogusThingProducer = new BogusThingProducer();
                var things = await bogusThingProducer.Generate(2, cancellationTokenSource.Token);
                foreach (var thing in things)
                {
                    thingsCollection[thing.Id] = thing;
                }
                Console.WriteLine($"published {i} things");
                await Task.Delay(random.Next(3000, 5000 ), cancellationTokenSource.Token);
            }
        }/*, cancellationTokenSource.Token*/),
    Task.Run(
        async () =>
        { /* event things */
            for (int i = 0; i < 2; i++){

                if (cancellationTokenSource.IsCancellationRequested)
                    break;
                using var kafkaPublisher = new KafkaPublisher("things");
                foreach (var (id, thing) in thingsCollection)
                {
                    var serializedInput = JsonSerializer.Serialize<Thing>(thing);
                    await kafkaPublisher.Publish(serializedInput);
                }
                Console.WriteLine($"published {i} things change events");
                await Task.Delay(random.Next(3000, 5000), cancellationTokenSource.Token);
            }
        }/*, cancellationTokenSource.Token*/),
    Task.Run(
        async () =>
        { /* ThingEvent things */
            await Task.Delay(5000, cancellationTokenSource.Token); // wait because others have to produce data for this..
            for (int i = 0; i < 10; i++)
            {
                try
                {
                
                    if (cancellationTokenSource.IsCancellationRequested) break;
                    var bogusEventProducer = new BogusEventProducer();
                    var bogusEvents = await bogusEventProducer.Generate(
                        100,
                        facilitiesCollection.Values.ToArray(),
                        thingsCollection.Values.ToArray(),
                        cancellationTokenSource.Token);
                    using var kafkaPublisher = new KafkaPublisher("thingevents");
                    foreach (var e in bogusEvents)
                    {
                        var serializedInput = JsonSerializer.Serialize<ThingEvent>(e);
                        await kafkaPublisher.Publish(serializedInput);
                    }
                    Console.WriteLine($"published {i} telemetry thingevents");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                }
                await Task.Delay(random.Next(1000, 10000), cancellationTokenSource.Token);
            }
        }/*, cancellationTokenSource.Token*/),
    Task.Run(
        () =>
        {
            Console.ReadLine();
            Console.WriteLine("cancelling running tasks");

            cancellationTokenSource.Cancel();

        }/*, cancellationTokenSource.Token*/)
};


try
{
    Task.WaitAll(tasks.ToArray());

}
catch (System.AggregateException ae)
{
    if (ae.InnerException != null && ae.InnerException.GetType() == typeof(TaskCanceledException))
    {
        Console.WriteLine($"{nameof(TaskCanceledException)} thrown with message: {ae.Message}");

    }
    else throw;

}
catch (OperationCanceledException e)
{
    Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
}
finally
{
    cancellationTokenSource.Dispose();
}


