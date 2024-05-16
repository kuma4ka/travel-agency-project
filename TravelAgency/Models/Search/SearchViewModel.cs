using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models.Search;

public class SearchViewModel
{
    [Required]
    public string? DepartureCity { get; init; }
    [Required]
    public string? ArrivalCity { get; init; }
    [Required]
    public DateTime DepartureDate { get; init; }
    [Required]
    public int Quantity { get; init; }
}