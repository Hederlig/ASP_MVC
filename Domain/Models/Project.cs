namespace Domain.Models;

public class Project
    {
    public string Id { get; set; } = null!;
    public string? Image { get; set; }
    public string ProjectName { get; set; } = null!;
    public Client Client { get; set; } = null!;
    public User? User { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ProjectOwner { get; set; } = null!;
    public decimal Budget { get; set; }     
    public Status Status { get; set; } = null!;

    }