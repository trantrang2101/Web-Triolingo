namespace Web_Triolingo.ModelDto
{
    public class SettingDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }
        public string? Value { get; set; }
        public int? ParentId { get; set; }
    }
}
