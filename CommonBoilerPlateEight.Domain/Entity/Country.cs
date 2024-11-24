namespace CommonBoilerPlateEight.Domain.Entity
{
    public class Country :BaseEntity
    {
        protected Country()
        {
            
        }
        public Country(string name, string flagCode,string code, string dialCode)
        {
            Name = name;
            FlagCode = flagCode;
            Code = code;
            DialCode = dialCode;
        }

        public void Update(string name, string flagCode, string code, string dialCode)
        {
            Name = name;
            FlagCode = flagCode;
            Code = code;
            DialCode = dialCode;
        }
        public string Name { get; set; }
        public string FlagCode { get; set; }
        public string Code { get; set; }
        public string DialCode { get; set; }
    }
}
