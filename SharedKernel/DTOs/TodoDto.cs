namespace SharedKernel.DTOs;
public class TodoDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
