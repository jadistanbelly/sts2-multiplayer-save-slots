using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.UI;

public sealed partial class MultiplayerSavePickerModal : Control, IScreenContext
{
    private readonly HostFlowController _controller;
    private readonly MultiplayerSavePickerModel _model;
    private Control? _defaultFocusedControl;
    private Control? _detailsOverlay;

    private MultiplayerSavePickerModal(HostFlowController controller, MultiplayerSavePickerModel model)
    {
        _controller = controller;
        _model = model;
        Name = "MultiplayerSavePickerModal";
    }

    public Control? DefaultFocusedControl => _defaultFocusedControl;

    public static void Show(HostFlowController controller, MultiplayerGameMode gameMode)
    {
        var model = controller.BuildPickerModel(gameMode);
        var container = NModalContainer.Instance
            ?? throw new InvalidOperationException("Modal container is not available.");
        container.Add(new MultiplayerSavePickerModal(controller, model));
    }

    public override void _Ready()
    {
        AnchorLeft = 0;
        AnchorTop = 0;
        AnchorRight = 1;
        AnchorBottom = 1;

        var panel = new PanelContainer
        {
            CustomMinimumSize = new Vector2(720, 520)
        };
        panel.SetAnchorsPreset(LayoutPreset.Center);
        panel.OffsetLeft = -360;
        panel.OffsetTop = -260;
        panel.OffsetRight = 360;
        panel.OffsetBottom = 260;
        AddChild(panel);

        var root = new VBoxContainer();
        root.AddThemeConstantOverride("separation", 12);
        panel.AddChild(root);

        root.AddChild(new Label
        {
            Text = $"Multiplayer Saves - {_model.GameMode}",
            HorizontalAlignment = HorizontalAlignment.Center
        });

        var scroll = new ScrollContainer
        {
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddChild(scroll);

        var rows = new VBoxContainer();
        rows.AddThemeConstantOverride("separation", 8);
        scroll.AddChild(rows);

        foreach (var row in _model.Rows)
        {
            AddPickerRow(rows, row);
        }

        var cancel = new Button { Text = "Cancel", CustomMinimumSize = new Vector2(180, 44) };
        cancel.Pressed += Close;
        root.AddChild(cancel);
        _defaultFocusedControl ??= cancel;
    }

    private void AddPickerRow(VBoxContainer rows, MultiplayerSavePickerRow row)
    {
        if (row.Details is null)
        {
            var button = CreateRowButton(row, new Vector2(640, 56));
            rows.AddChild(button);
            _defaultFocusedControl ??= button;
            return;
        }

        var rowContainer = new HBoxContainer();
        rowContainer.AddThemeConstantOverride("separation", 8);
        rows.AddChild(rowContainer);

        var action = CreateRowButton(row, new Vector2(520, 56));
        action.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        rowContainer.AddChild(action);
        _defaultFocusedControl ??= action;

        var details = new Button
        {
            Text = "Details",
            CustomMinimumSize = new Vector2(104, 56),
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        };
        details.Pressed += () => ShowDetails(row.Details);
        rowContainer.AddChild(details);
    }

    private Button CreateRowButton(MultiplayerSavePickerRow row, Vector2 minimumSize)
    {
        var button = new Button
        {
            Text = string.IsNullOrWhiteSpace(row.Subtitle) ? row.Title : $"{row.Title}\n{row.Subtitle}",
            CustomMinimumSize = minimumSize,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        };
        button.Pressed += () => SelectRow(row);
        return button;
    }

    private void ShowDetails(MultiplayerSavePickerDetails details)
    {
        CloseDetails();

        var overlay = new Control
        {
            Name = "MultiplayerSaveCampaignDetails",
            MouseFilter = MouseFilterEnum.Stop
        };
        overlay.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(overlay);
        _detailsOverlay = overlay;

        var panel = new PanelContainer
        {
            CustomMinimumSize = new Vector2(640, 460)
        };
        panel.SetAnchorsPreset(LayoutPreset.Center);
        panel.OffsetLeft = -320;
        panel.OffsetTop = -230;
        panel.OffsetRight = 320;
        panel.OffsetBottom = 230;
        overlay.AddChild(panel);

        var root = new VBoxContainer();
        root.AddThemeConstantOverride("separation", 12);
        panel.AddChild(root);

        root.AddChild(new Label
        {
            Text = details.Title,
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        });

        root.AddChild(new Label
        {
            Text = details.Subtitle,
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        });

        var scroll = new ScrollContainer
        {
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddChild(scroll);

        scroll.AddChild(new Label
        {
            Text = BuildDetailsBody(details),
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        });

        var close = new Button { Text = "Close", CustomMinimumSize = new Vector2(180, 44) };
        close.Pressed += CloseDetails;
        root.AddChild(close);
    }

    private void CloseDetails()
    {
        _detailsOverlay?.QueueFree();
        _detailsOverlay = null;
    }

    private static string BuildDetailsBody(MultiplayerSavePickerDetails details) =>
        $"{string.Join('\n', details.SummaryLines)}\n\nRoster\n{string.Join('\n', details.RosterLines)}";

    private void SelectRow(MultiplayerSavePickerRow row)
    {
        Close();

        OperationResult result = row.Kind == PickerRowKind.StartNewRun
            ? _controller.SelectStartNewRun(_model.GameMode)
            : _controller.SelectExistingCampaign(row.CampaignId!, _model.GameMode);

        if (result.Success)
            return;

        var recovery = _controller.BuildRecoveryModel(_model.GameMode);
        if (recovery.HasOptions)
        {
            MultiplayerSaveRecoveryModal.Show(_controller, _model.GameMode, row, recovery);
            return;
        }

        ShowError(result.ErrorMessage ?? "Unable to continue multiplayer host flow.");
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
