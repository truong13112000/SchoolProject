namespace SchoolProject.Dto.Class
{
    public class ClassViewDto
    {
        public long Id { get; set; }
        public string ClassName { get; set; }
        public int Capacity { get; set; }
        public string? Department { get; set; }
        public string? School { get; set; }
    }
}
