using TravelAgency.Models.UserRelated;

namespace TravelAgency.Models.Admin;

public class AdminPanelViewModel
{
    public List<AccountModel>? AccModels { get; init; }
    public List<FlightModel>? FlightModels { get; init; }
}