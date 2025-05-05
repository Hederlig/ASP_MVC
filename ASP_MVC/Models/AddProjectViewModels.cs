using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class AddProjectViewModel
    {
    [Required(ErrorMessage = "! Required.")]
    [Display(Name = "Project Name", Prompt = "Enter project name")]
    public string ProjectName { get; set; } = null!;

    [Required(ErrorMessage = "! Required.")]
    [Display(Name = "Client Name", Prompt = "Enter client name")]
    public string ClientName { get; set; } = null!;

    [Display(Name = "Project Description", Prompt = "Enter project description")]
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }

    [Range(0, 100000000, ErrorMessage = "Budget must be a positive number.")]
    [DataType(DataType.Currency)]
    public decimal? Budget { get; set; }
    }