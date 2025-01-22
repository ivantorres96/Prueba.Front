namespace Prueba.Front.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Priority { get; set; }

        public Stream? File { get; set; }

        public int StateId { get; set; }
    }

    public class TaskFilterDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Priority { get; set; }

        public int StateId { get; set; }
    }

    public class TaskSum
    {
        public int uno { get; set; }
        public int dos { get; set; }
        public int tres { get; set; }
    }
}
