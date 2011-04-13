using System.Collections.Generic;

using AutoScout24.SMP.Common.Domain;
using AutoScout24.SMP.Common.Domain.TestHelper;
using AutoScout24.SMP.Garages.Domain;

namespace AutoScout24.SMP.Garages.TestHelper
{
    public class GarageBuilder : BuilderBase<Garage>
    {
        public GarageBuilder()
        {
            PropertyBag.CalculationSets = new List<CalculationSet>();
            PropertyBag.Id = UniqueIdBuilder.GenerateUniqueId();
            PropertyBag.Name = "Müller";
            PropertyBag.GeoLocation = new GeoLocation(5, 90);
            PropertyBag.Address = new Address("this street", "53127", "the city");
            PropertyBag.Phone = new PhoneNumber("089", "127679812");
            PropertyBag.Fax = new PhoneNumber("089", "123454");
            PropertyBag.Email = "john@foo.com";
            PropertyBag.Website = "www.foo.com";
            PropertyBag.OfficeHoursWeekdaysFrom = 8;
            PropertyBag.OfficeHoursWeekdaysTill = 20;
            PropertyBag.OfficeHoursSaturdaysFrom = 10;
            PropertyBag.OfficeHoursSaturdaysTill = 16;
            PropertyBag.AuthorizedMakes = new List<Make>();
            PropertyBag.GarageType = GarageType.Independent;
            PropertyBag.Makes = new List<Make>();
            PropertyBag.EncryptedPassword = "0123456789";
        }

        public static Garage GarageExampleWithTwoCalculationSets
        {
            get
            {
                return new GarageBuilder()
                    .WithCalculationSet(new CalculationSetBuilder()
                        .WithName("calcSet1")
                        .WithHourlyRate(new Money(10000))
                        .WithPartSurcharge(Discount.CreateReduction(12.25))
                        .WithMotorOilPrices(new List<MotorOilPrice>
                        {
                            new MotorOilPrice("oil1", new Money(1000)) 
                        })
                        .WithMakes(new MakeBuilder()
                            .WithId(16)
                            .WithName("BMW")
                            .Build())
                        .Build())
                    .WithCalculationSet(new CalculationSetBuilder()
                        .WithName("calcSet2").
                        WithHourlyRate(new Money(20000)).
                        WithPartSurcharge(Discount.CreateReduction(5.95)).
                        WithMotorOilPrices(new List<MotorOilPrice>
                        {
                            new MotorOilPrice("oil2", new Money(1500)) 
                        })
                        .WithMakes(new MakeBuilder()
                            .WithId(120)
                            .WithName("Volvo")
                            .Build())
                        .Build())
                    .Build();
            }
        }

        public GarageBuilder WithAddress(Address address)
        {
            PropertyBag.Address = address;
            return this;
        }

        public GarageBuilder WithId(UniqueId id)
        {
            PropertyBag.Id = id;
            return this;
        }

        public GarageBuilder WithIdFromString(string idString)
        {
            return WithId(new UniqueId(idString));
        }

        public GarageBuilder WithName(string name)
        {
            PropertyBag.Name = name;
            return this;
        }

        public GarageBuilder WithCalculationSet(CalculationSet calculationSet)
        {
            PropertyBag.CalculationSets.Add(calculationSet);
            return this;
        }

        public GarageBuilder WithoutCalculationSet()
        {
            while (PropertyBag.CalculationSets.Count > 0)
            {
                PropertyBag.CalculationSets.Remove(PropertyBag.CalculationSets.First());
            }

            return this;
        }

        public GarageBuilder WithGeoLocation(GeoLocation geoLocation)
        {
            PropertyBag.GeoLocation = geoLocation;
            return this;
        }

        public GarageBuilder WithPhone(PhoneNumber phone)
        {
            PropertyBag.Phone = phone;
            return this;
        }

        public GarageBuilder WithFax(PhoneNumber fax)
        {
            PropertyBag.Fax = fax;
            return this;
        }

        public GarageBuilder WithEmail(string email)
        {
            PropertyBag.Email = email;
            return this;
        }

        public GarageBuilder WithWebsite(string website)
        {
            PropertyBag.Website = website;
            return this;
        }

        public GarageBuilder WithOfficeHoursWeekdaysFrom(int officeHoursWeekdaysFrom)
        {
            PropertyBag.OfficeHoursWeekdaysFrom = officeHoursWeekdaysFrom;
            return this;
        }

        public GarageBuilder WithOfficeHoursWeekdaysTill(int officeHoursWeekdaysTill)
        {
            PropertyBag.OfficeHoursWeekdaysTill = officeHoursWeekdaysTill;
            return this;
        }

        public GarageBuilder WithOfficeHoursSaturdaysFrom(int? officeHoursSaturdaysFrom)
        {
            PropertyBag.OfficeHoursSaturdaysFrom = officeHoursSaturdaysFrom;
            return this;
        }

        public GarageBuilder WithOfficeHoursSaturdaysTill(int? officeHoursSaturdaysTill)
        {
            PropertyBag.OfficeHoursSaturdaysTill = officeHoursSaturdaysTill;
            return this;
        }

        public GarageBuilder WithContactPerson(ContactPerson contactPerson)
        {
            PropertyBag.ContactPerson = contactPerson;
            return this;
        }
        
        public GarageBuilder WithAuthorizedMakes(IEnumerable<Make> authorizedMakes)
        {
            PropertyBag.AuthorizedMakes = authorizedMakes;
            return this;
        }

        public GarageBuilder WithGarageType(GarageType garageType)
        {
            PropertyBag.GarageType = garageType;
            return this;
        }

        public GarageBuilder WithLegalNotice(string legalNotice)
        {
            PropertyBag.LegalNotice = legalNotice;
            return this;
        }

        public GarageBuilder WithEncryptedPassword(string encryptedPassword)
        {
            PropertyBag.EncryptedPassword = encryptedPassword;
            return this;
        }
    }
}