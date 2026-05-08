using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.UI;

namespace MultiplayerSaveSlots.Runtime;

public sealed class HostFlowController
{
    private readonly IHostFlowSaveBank _bank;
    private readonly IActiveSaveActivator _activator;
    private readonly IHostFlowContinuation _continuation;
    private readonly HostFlowSession _session;
    private readonly IClock _clock;

    public HostFlowController(
        IHostFlowSaveBank bank,
        IActiveSaveActivator activator,
        IHostFlowContinuation continuation,
        HostFlowSession session,
        IClock clock)
    {
        _bank = bank;
        _activator = activator;
        _continuation = continuation;
        _session = session;
        _clock = clock;
    }

    public MultiplayerSavePickerModel BuildPickerModel(MultiplayerGameMode gameMode)
    {
        var rows = new List<MultiplayerSavePickerRow> { MultiplayerSavePickerRow.StartNew() };
        rows.AddRange(_bank.ListCampaigns(gameMode).Select(MultiplayerSavePickerRow.Campaign));

        return new MultiplayerSavePickerModel(gameMode, rows);
    }

    public OperationResult SelectStartNewRun(MultiplayerGameMode gameMode)
    {
        _session.SelectNewRun(gameMode);
        return _continuation.StartNewRun(gameMode);
    }

    public OperationResult SelectExistingCampaign(string campaignId, MultiplayerGameMode gameMode)
    {
        var activation = _activator.Activate(campaignId, _clock.UtcNow);
        if (!activation.Success)
            return activation;

        _session.SelectExistingCampaign(campaignId, gameMode);
        return _continuation.LoadExistingRun();
    }
}
