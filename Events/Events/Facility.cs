
namespace Events;

using System.Collections;

public class Facility
{
    public Facility()
    {
        Name = Timezone = AddressLine1 = AddressLine2 = State = Country = Region = ZipCode = string.Empty;
    }
    public long Id { get; set; }

    public string Name { get; set; }

    public string Timezone { get; set; }

    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public string State { get; set; }

    public string Country { get; set; }

    public string ZipCode { get; set; }

    public string  Region { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

}