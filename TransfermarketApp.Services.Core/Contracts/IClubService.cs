using TransfermarketApp.ViewModels.Clubs;

namespace TransfermarketApp.Services.Core.Contracts
{
	public interface IClubService
	{
		Task<IEnumerable<ClubListViewModel>> GetAllClubsAsync();
		Task<ClubDetailsViewModel?> GetClubByIdAsync(int id);
		Task CreateClubAsync(CreateClubViewModel model);
		Task<EditClubViewModel?> GetClubForEditAsync(int id);
		Task UpdateClubAsync(int id, EditClubViewModel model);
		Task DeleteClubAsync(int id);
		Task<List<LeagueDropdownViewModel>> GetLeaguesAsync();

	}
}
