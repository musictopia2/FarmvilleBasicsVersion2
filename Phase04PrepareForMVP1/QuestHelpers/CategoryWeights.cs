using System;
using System.Collections.Generic;
using System.Text;

namespace Phase04PrepareForMVP1.QuestHelpers;

public sealed class CategoryWeights
{
    // Relative weights, not necessarily percentages.
    public int BasicWeight { get; init; } = 60;
    public int WorkshopWeight { get; init; } = 30;
    public int WorksiteWeight { get; init; } = 10;

    // Optional: “minimum desired” counts (soft goals). Helpful if you want guardrails.
    public int MinBasics { get; init; } = 0;
    public int MinWorkshops { get; init; } = 0;
    public int MinWorksites { get; init; } = 0;
}