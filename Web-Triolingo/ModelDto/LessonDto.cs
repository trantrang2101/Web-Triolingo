namespace Web_Triolingo.ModelDto
{
    public class LessonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int UnitId { get; set; }
        public int TypeId { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public int Status { get; set; }
    }
}
