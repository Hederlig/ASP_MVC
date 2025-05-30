﻿namespace Presentation.Models;

public class ProjectViewModel
    {
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ProjectName { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string Description { get; set; } = null!;

    }