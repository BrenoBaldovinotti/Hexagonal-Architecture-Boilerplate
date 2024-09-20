namespace Domain.ValueObjects;

public class Address
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string ZipCode { get; private set; }

    public Address(string street, string city, string zipCode)
    {
        if (string.IsNullOrEmpty(street)) throw new ArgumentNullException(nameof(street));
        if (string.IsNullOrEmpty(city)) throw new ArgumentNullException(nameof(city));
        if (string.IsNullOrEmpty(zipCode)) throw new ArgumentNullException(nameof(zipCode));

        Street = street;
        City = city;
        ZipCode = zipCode;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType()) return false;

        var address = (Address)obj;
        return Street == address.Street && City == address.City && ZipCode == address.ZipCode;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, ZipCode);
    }
}
