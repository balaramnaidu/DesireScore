namespace MailingAddress
{
    public class AddressInfo
    {
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        //These two are temporary properties that need to be removed later
        public uint ListedPrice { get; set; }
        public uint Hoa { get; set; }

        public bool Validate()
        {
            return !(string.IsNullOrEmpty(Address1) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(State));
        }
    }

    public class PropertyInfo
    {
        public uint ZId { get; set; }
        public string ZAmount { get; set; }
        public string ZLow { get; set; }
        public string ZHigh { get; set; }
        public string ZRent { get; set; }
        public string ZRentLow { get; set; }
        public string ZRentHigh { get; set; }
        public string ZAmountLastUpdated { get; set; }
        public string ZRentLastUpdated { get; set; }
        public string HomeType { get; set; }
        public string YearBuilt { get; set; }
        public string LotSize { get; set; }
        public string FinishedSize { get; set; }
        public string Bathrooms { get; set; }
        public string Bedrooms { get; set; }
        public string LastSoldPrice { get; set; }
        public string LastSoldDate { get; set; }
        public string FipsCountyCode { get; set; }
        public uint ListedPrice { get; set; }
        public uint Hoa { get; set; }
        public AddressInfo Address { get; set; }
        public int DesirabilityScore { get; set; }

        public bool ValidateForScore()
        {
            return !((ListedPrice <= 0) || string.IsNullOrEmpty(ZAmount) || string.IsNullOrEmpty(ZRent));
        }
    }
}
