namespace AdvanceAPI.DTO.DB
{
    public class SQLParameters
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public SQLParameters(string name, object value)
        {
            Name = name;
            Value = value;
        }

    }
}
