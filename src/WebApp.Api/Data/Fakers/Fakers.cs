using Bogus;
using WebApp.Api.Models;
using Person = WebApp.Api.Models.Person;

namespace WebApp.Api.Data.Fakers;

public class Fakers
{
    Faker<Customer>? _customerFaker = null;
    Faker<Customer2>? _customerFaker2 = null;
    Faker<Address>? _addressFaker = null;
    public Faker<Person> getGeneratorPerson()
    {
        return new Faker<Person>("ru")
            .RuleFor(x => x.FullName, f => f.Name.FullName())
            .RuleFor(x => x.Age, f => f.Random.Byte(16, 100))
            .RuleFor(x => x.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(x => x.Work, f =>
            {
                return new Work
                {
                    Company = f.Company.CompanyName(),
                    Position = f.Random.ListItem(new List<string> {
                        "Программист",
                        "Менеджер",
                        "Бизнес-аналитик",
                        "Дизайнер",
                        "Тестировщик"
                    })
                };
            });
    }
    public Faker<Customer> GetCustomerGenerator(bool includeAddresses = true)
    {
        if (_customerFaker is null)
        {
            var addressFaker = GetAddressGenerator();
            var id = 0;
            _customerFaker = new Faker<Customer>(locale: "ru").UseSeed(100)
                .RuleFor(c => c.Id, f => ++id)
                .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                .RuleFor(c => c.ContactName, f => f.Name.FullName())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumberFormat().OrNull(f, .15f));
            //o.Age = f.Random.Number(30, 50);
            //o.FamilyName = f.Name.LastName();
            //o.Gender = f.PickRandom<Gender>();

            if (includeAddresses)
            {
                _customerFaker = _customerFaker
                    .RuleFor(c => c.Address, f => addressFaker.Generate(1).First()
                        .OrNull(f, .1f));
            }
        }
        return _customerFaker;
    }    
    
    public Faker<Customer2> GetCustomerGenerator2(bool includeAddresses = true)
    {
        if (_customerFaker2 is null)
        {
            var addressFaker = GetAddressGenerator();
            var id = 0;
            _customerFaker2 = new Faker<Customer2>(locale: "ru").UseSeed(100)
                .StrictMode(false)
                .CustomInstantiator(f => new Customer2())
                .Rules((f, o) =>
                {
                    o.Id = f.IndexFaker + 1;
                    o.Age = f.Random.Number(30, 50);
                    o.FamilyName = f.Name.LastName();
                    o.Gender = f.PickRandom<Gender>();
                });
        }

        return _customerFaker2;
    }

    public Faker<Address> GetAddressGenerator()
    {
        if (_addressFaker is null)
        {
            var id = 0;
            _addressFaker = new Faker<Address>(locale: "ru").UseSeed(100)
                .CustomInstantiator(f => new Address())
                .Rules((f, s) =>
                {
                    s.Id = ++id;
                    //o.Id = f.IndexFaker + 1;
                    s.Address1 = f.Address.StreetAddress();
                    s.Address2 = f.Address.SecondaryAddress().OrNull(f, .5f);
                    s.City = f.Address.City();
                    s.StateProvince = f.Address.State();
                    s.PostalCode = f.Address.ZipCode();
                });
        }

        return _addressFaker;
    }

    /*

    var dataTable = GenerateDataTable();

    const int peopleSize = 10;
    foreach (var row in FakePersons.Generate(peopleSize).Select(person => ToDataRow(person, dataTable)))
    {
        dataTable.Rows.Add(row);
    }

    private static DataRow ToDataRow(Person person, DataTable dataTable)
    {
        DataRow row = dataTable.NewRow();
        row[nameof(Person.FirstName)] = person.FirstName;
        row[nameof(Person.LastName)] = person.LastName;
        row[nameof(Person.Address)] = $"{person.Address?.Street}, {person.Address?.City}";
        return row;
    }

    private static DataSet GenerateDataSet()
    {
        DataSet ds = new();
        ds.DataSetName = "PersonDataSet";
        ds.Tables.Add(GenerateDataTable("People1"));
        ds.Tables.Add(GenerateDataTable("People2"));
        return ds;

        static DataTable GenerateDataTable(string tableName)
        {
            DataTable dt = new();
            dt.TableName = tableName;
            _ = dt.Columns.Add(nameof(Person.FirstName), typeof(string));
            _ = dt.Columns.Add(nameof(Person.LastName), typeof(string));
            _ = dt.Columns.Add(nameof(Person.Address), typeof(string));

            const int peopleSize = 10;
            foreach (var row in FakePersons.Generate(peopleSize).Select(person => ToDataRow(person, dt)))
            {
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
    */

    /*
    public static class FakeData
    {
        public static List<Address> Addresss = new List<Address>();
        public static List<Customer> Customers = new List<Customer>();

        public static void Init(int count)
        {
            var custId = 1;
            var CustomerFaker = new Faker<Customer>()
                .RuleFor(p => p.Id, _ => custId++)
                .RuleFor(p => p.FullName, f => f.Hacker.Phrase())
                .RuleFor(c => c.Email, f => f.Person.Email)
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumberFormat().OrNull(f, .15f));
            

            var addrId = 1;
            var AddressFaker = new Faker<Address>()
                .RuleFor(b => b.AdddressId, _ => addrId++)
                .RuleFor(c => c.Address1, f => f.Address.StreetAddress())
                .RuleFor(c => c.Address2, f => f.Address.SecondaryAddress().OrNull(f, .5f))
                .RuleFor(c => c.City, f => f.Address.City())
                .RuleFor(c => c.StateProvince, f => f.Address.State())
                .RuleFor(c => c.PostalCode, f => f.Address.ZipCode())
                .RuleFor(b => b.Customers, (f, b) =>
                {
                    CustomerFaker.RuleFor(p => p.AdddressId, _ => b.AdddressId);

                    var Customers = CustomerFaker.GenerateBetween(3, 5);

                    FakeData.Customers.AddRange(Customers);

                    return null; // Address.Posts is a getter only. The return value has no impact.
                });

            var Addresss = AddressFaker.Generate(count);

            FakeData.Addresss.AddRange(Addresss);
        }
    */
}