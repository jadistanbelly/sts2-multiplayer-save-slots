using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public sealed class LoadLobbyCompatibilityGuard
{
    private readonly HostFlowSession _session;
    private readonly Func<string, CampaignMetadata> _campaignLookup;
    private readonly Func<IReadOnlyList<PlayerIdentity>> _currentRosterProvider;
    private readonly Action<CampaignCompatibilityWarning> _showWarning;

    public LoadLobbyCompatibilityGuard(
        HostFlowSession session,
        Func<string, CampaignMetadata> campaignLookup,
        Func<IReadOnlyList<PlayerIdentity>> currentRosterProvider,
        Action<CampaignCompatibilityWarning> showWarning)
    {
        _session = session;
        _campaignLookup = campaignLookup;
        _currentRosterProvider = currentRosterProvider;
        _showWarning = showWarning;
    }

    public bool ShouldAllowRunToBegin()
    {
        if (_session.SelectedCampaignId is null)
            return true;

        try
        {
            var campaign = _campaignLookup(_session.SelectedCampaignId);
            var currentRoster = _currentRosterProvider();
            var warning = CampaignCompatibilityChecker.BuildWarning(campaign, currentRoster);
            if (warning is null)
                return true;

            if (!_session.ShouldShowCompatibilityWarning(warning.WarningKey))
                return true;

            _showWarning(warning);
            return false;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to check load-lobby compatibility: {ex.Message}");
            return true;
        }
    }
}
