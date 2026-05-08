using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.UI;

public sealed partial class MultiplayerSaveRecoveryModal : Control, IScreenContext
{
    private readonly HostFlowController _controller;
    private readonly MultiplayerGameMode _gameMode;
    private readonly MultiplayerSavePickerRow _row;
    private readonly ActiveSaveRecoveryModel _model;
    private Control? _defaultFocusedControl;

    private MultiplayerSaveRecoveryModal(
        HostFlowController controller,
        MultiplayerGameMode gameMode,
        MultiplayerSavePickerRow row,
        ActiveSaveRecoveryModel model)
    {
        _controller = controller;
        _gameMode = gameMode;
        _row = row;
        _model = model;
        Name = "MultiplayerSaveRecoveryModal";
    }

    public Control? DefaultFocusedControl => _defaultFocusedControl;

    public static void Show(
        HostFlowController controller,
        MultiplayerGameMode gameMode,
        MultiplayerSavePickerRow row,
        ActiveSaveRecoveryModel model)
    {
        var container = NModalContainer.Instance;
        if (container is null)
            return;

        container.Clear();
        container.Add(new MultiplayerSaveRecoveryModal(controller, gameMode, row, model));
    }

    public override void _Ready()
    {
        AnchorLeft = 0;
        AnchorTop = 0;
        AnchorRight = 1;
        AnchorBottom = 1;

        var panel = new PanelContainer
        {
            CustomMinimumSize = new Vector2(720, 420)
        };
        panel.SetAnchorsPreset(LayoutPreset.Center);
        panel.OffsetLeft = -360;
        panel.OffsetTop = -210;
        panel.OffsetRight = 360;
        panel.OffsetBottom = 210;
        AddChild(panel);

        var root = new VBoxContainer();
        root.AddThemeConstantOverride("separation", 12);
        panel.AddChild(root);

        root.AddChild(new Label
        {
            Text = _model.Title,
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        });

        root.AddChild(new Label
        {
            Text = _model.Message,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        });

        foreach (var option in _model.Options)
        {
            var button = new Button
            {
                Text = $"{option.Label}\n{option.Description}",
                CustomMinimumSize = new Vector2(640, 64),
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            button.Pressed += () => SelectOption(option);
            root.AddChild(button);
            _defaultFocusedControl ??= button;
        }

        var cancel = new Button { Text = "Cancel", CustomMinimumSize = new Vector2(180, 44) };
        cancel.Pressed += Close;
        root.AddChild(cancel);
        _defaultFocusedControl ??= cancel;
    }

    private void SelectOption(ActiveSaveRecoveryOption option)
    {
        Close();

        OperationResult result = _row.Kind == PickerRowKind.StartNewRun
            ? _controller.RecoverAndSelectStartNewRun(option.Kind, _gameMode)
            : _controller.RecoverAndSelectExistingCampaign(option.Kind, _row.CampaignId!, _gameMode);

        if (result.Success)
            return;

        ShowError(result.ErrorMessage ?? "Unable to recover active multiplayer save.");
    }

    private static void ShowError(string message)
    {
        var popup = NErrorPopup.Create("Multiplayer Save Slots", message, showReportBugButton: false);
        var container = NModalContainer.Instance;
        if (popup is null || container is null)
            return;

        container.Clear();
        container.Add(popup);
    }

    private static void Close()
    {
        NModalContainer.Instance?.Clear();
    }
}
