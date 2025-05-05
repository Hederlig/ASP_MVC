using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Models;

public class EditProjectViewModel
    {
    public string Id { get; set; } = null!;

    [Display(Name = "Project Name")]
    public string? ProjectName { get; set; }

    [Display(Name = "Client Name")]
    public string? ClientName { get; set; }

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    public decimal? Budget { get; set; }

    public string Status { get; set; } = null!;

    public List<SelectListItem> Statuses { get; set; } = new();
    }