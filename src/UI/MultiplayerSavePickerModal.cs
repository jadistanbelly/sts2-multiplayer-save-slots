using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.UI;

public sealed partial class MultiplayerSavePickerModal : Control, IScreenContext
{
    private const float DetailsBodyMinimumWidth = 580f;

    private readonly HostFlowController _controller;
    private readonly MultiplayerSavePickerModel _model;
    private readonly Dictionary<string, Button> _campaignButtons = new(StringComparer.Ordinal);
    private Control? _defaultFocusedControl;
    private Control? _detailsOverlay;
    private VBoxContainer? _previewRoot;
    private Button? _continueButton;
    private MultiplayerSavePickerRow? _selectedCampaign;
    private bool _built;

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
        var modal = new MultiplayerSavePickerModal(controller, model);
        modal.BuildUi();
        container.Clear();
        GD.Print($"[MultiplayerSaveSlots] Opening save picker for {gameMode} with {model.Rows.Count} rows.");
        container.Add(modal);
    }

    public override void _Ready() => BuildUi();

    private void BuildUi()
    {
        if (_built)
            return;

        _built = true;
        ModalUiStyling.PrepareModalRoot(this);
        _selectedCampaign = _model.DefaultSelectedCampaign;

        var panel = ModalUiStyling.CreatePanel(new Vector2(980, 620), 490, 310);
        AddChild(panel);

        var root = new VBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddThemeConstantOverride("separation", 12);
        panel.AddChild(root);

        var title = new Label
        {
            Text = $"Multiplayer Saves - {_model.GameMode}",
            HorizontalAlignment = HorizontalAlignment.Center
        };
        ModalUiStyling.StyleTitle(title);
        root.AddChild(title);

        var body = new HBoxContainer
        {
            CustomMinimumSize = new Vector2(900, 430),
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        body.AddThemeConstantOverride("separation", 14);
        root.AddChild(body);

        body.AddChild(BuildCampaignList());
        body.AddChild(new VSeparator { SizeFlagsVertical = SizeFlags.ExpandFill });
        body.AddChild(BuildPreviewPanel());

        var actions = new HBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        actions.AddThemeConstantOverride("separation", 10);
        root.AddChild(actions);

        var cancel = new Button { Text = "Cancel", CustomMinimumSize = new Vector2(180, 44) };
        ModalUiStyling.StyleButton(cancel);
        cancel.Pressed += Close;
        actions.AddChild(cancel);

        actions.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

        _continueButton = new Button
        {
            Text = "Continue",
            Disabled = _selectedCampaign is null,
            CustomMinimumSize = new Vector2(180, 44)
        };
        ModalUiStyling.StyleButton(_continueButton);
        _continueButton.Pressed += ContinueSelectedCampaign;
        actions.AddChild(_continueButton);

        _defaultFocusedControl ??= cancel;
    }

    private Control BuildCampaignList()
    {
        var list = new VBoxContainer
        {
            CustomMinimumSize = new Vector2(420, 430),
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        list.AddThemeConstantOverride("separation", 8);

        var startNewRow = _model.Rows.FirstOrDefault(row => row.Kind == PickerRowKind.StartNewRun)
            ?? MultiplayerSavePickerRow.StartNew();
        var startNew = CreateRowButton(startNewRow, new Vector2(400, 60));
        startNew.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        list.AddChild(startNew);
        _defaultFocusedControl ??= startNew;

        var scroll = new ScrollContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        list.AddChild(scroll);

        var rows = new VBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        rows.AddThemeConstantOverride("separation", 8);
        scroll.AddChild(rows);

        foreach (var row in _model.CampaignRows)
            AddCampaignRow(rows, row);

        return list;
    }

    private void AddCampaignRow(VBoxContainer rows, MultiplayerSavePickerRow row)
    {
        var button = new Button
        {
            Text = string.IsNullOrWhiteSpace(row.Subtitle) ? row.Title : $"{row.Title}\n{row.Subtitle}",
            CustomMinimumSize = new Vector2(400, 66),
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        ModalUiStyling.StyleButton(button);
        button.Pressed += () => SelectCampaignPreview(row);
        rows.AddChild(button);

        if (!string.IsNullOrWhiteSpace(row.CampaignId))
            _campaignButtons[row.CampaignId] = button;

        if (row.CampaignId == _selectedCampaign?.CampaignId)
            _defaultFocusedControl = button;
    }

    private Control BuildPreviewPanel()
    {
        _previewRoot = new VBoxContainer
        {
            CustomMinimumSize = new Vector2(440, 430),
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        _previewRoot.AddThemeConstantOverride("separation", 10);
        RenderPreview(_selectedCampaign);
        return _previewRoot;
    }

    private Button CreateRowButton(MultiplayerSavePickerRow row, Vector2 minimumSize)
    {
        var button = new Button
        {
            Text = string.IsNullOrWhiteSpace(row.Subtitle) ? row.Title : $"{row.Title}\n{row.Subtitle}",
            CustomMinimumSize = minimumSize,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        };
        ModalUiStyling.StyleButton(button);
        button.Pressed += () => SelectRow(row);
        return button;
    }

    private void SelectCampaignPreview(MultiplayerSavePickerRow row)
    {
        _selectedCampaign = row.Kind == PickerRowKind.Campaign ? row : null;
        if (_continueButton is not null)
            _continueButton.Disabled = _selectedCampaign is null;

        RenderPreview(_selectedCampaign);
        if (_selectedCampaign?.CampaignId is { } campaignId
            && _campaignButtons.TryGetValue(campaignId, out var button))
        {
            button.GrabFocus();
        }
    }

    private void ContinueSelectedCampaign()
    {
        if (_selectedCampaign is null)
            return;

        SelectRow(_selectedCampaign);
    }

    private void RenderPreview(MultiplayerSavePickerRow? row)
    {
        if (_previewRoot is null)
            return;

        ClearChildren(_previewRoot);

        if (row?.Details is null)
        {
            _previewRoot.AddChild(CreatePreviewLabel(MultiplayerSavePickerModel.EmptyPreviewTitle, 28, HorizontalAlignment.Center));
            _previewRoot.AddChild(CreatePreviewLabel(MultiplayerSavePickerModel.EmptyPreviewBody, 20, HorizontalAlignment.Center));
            return;
        }

        var details = row.Details;
        _previewRoot.AddChild(CreatePreviewLabel(details.Title, 28, HorizontalAlignment.Center));
        _previewRoot.AddChild(CreatePreviewLabel(details.Subtitle, 21, HorizontalAlignment.Center));

        var scroll = new ScrollContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        _previewRoot.AddChild(scroll);

        var content = new VBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        content.AddThemeConstantOverride("separation", 8);
        scroll.AddChild(content);

        content.AddChild(CreatePreviewLabel("Party", 21, HorizontalAlignment.Left));
        foreach (var entry in details.RosterEntries.Count == 0
            ? details.RosterLines.Select(line => new MultiplayerSavePickerRosterEntry(line, null, false))
            : details.RosterEntries)
        {
            content.AddChild(CreateRosterPreviewRow(entry));
        }

        content.AddChild(CreatePreviewLabel("Run Details", 21, HorizontalAlignment.Left));
        foreach (var line in details.SummaryLines)
            content.AddChild(CreatePreviewLabel(line, 18, HorizontalAlignment.Left));
    }

    private static Control CreateRosterPreviewRow(MultiplayerSavePickerRosterEntry entry)
    {
        var row = new HBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        row.AddThemeConstantOverride("separation", 8);

        if (entry.HasKnownPlayer)
            row.AddChild(CreateCharacterIndicator(entry.SelectedCharacterId));

        var label = CreatePreviewLabel(entry.Text, 18, HorizontalAlignment.Left);
        label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        row.AddChild(label);
        return row;
    }

    private static Label CreatePreviewLabel(string text, int fontSize, HorizontalAlignment alignment)
    {
        var label = new Label
        {
            Text = text,
            HorizontalAlignment = alignment,
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        ModalUiStyling.StyleBody(label, fontSize);
        return label;
    }

    private static void ClearChildren(Node parent)
    {
        foreach (var child in parent.GetChildren())
        {
            parent.RemoveChild(child);
            child.QueueFree();
        }
    }

    private static string? CharacterIconPath(string? selectedCharacterId)
    {
        if (string.IsNullOrWhiteSpace(selectedCharacterId))
            return null;

        return selectedCharacterId.Trim().ToUpperInvariant() switch
        {
            "CHARACTER.IRONCLAD" => "res://images/ui/top_panel/character_icon_ironclad.png",
            "CHARACTER.SILENT" => "res://images/ui/top_panel/character_icon_silent.png",
            "CHARACTER.DEFECT" => "res://images/ui/top_panel/character_icon_defect.png",
            "CHARACTER.NECROBINDER" => "res://images/ui/top_panel/character_icon_necrobinder.png",
            _ => null
        };
    }

    private static Texture2D? TryLoadCharacterIcon(string? selectedCharacterId)
    {
        var path = CharacterIconPath(selectedCharacterId);
        if (path is null || !ResourceLoader.Exists(path))
            return null;

        return ResourceLoader.Load<Texture2D>(path);
    }

    private static Control CreateCharacterBadge(string? selectedCharacterId)
    {
        var panel = new PanelContainer
        {
            CustomMinimumSize = new Vector2(38, 34)
        };
        ModalUiStyling.StyleBadgePanel(panel);

        var label = new Label
        {
            Text = MultiplayerSavePickerModel.CharacterBadgeText(selectedCharacterId),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            CustomMinimumSize = new Vector2(38, 34)
        };
        ModalUiStyling.StyleBody(label, 16);
        panel.AddChild(label);
        return panel;
    }

    private static Control CreateCharacterIndicator(string? selectedCharacterId)
    {
        var texture = TryLoadCharacterIcon(selectedCharacterId);
        if (texture is null)
            return CreateCharacterBadge(selectedCharacterId);

        return new TextureRect
        {
            Texture = texture,
            ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional,
            StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
            CustomMinimumSize = new Vector2(34, 34)
        };
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

        var panel = ModalUiStyling.CreatePanel(new Vector2(660, 480), 330, 240);
        overlay.AddChild(panel);

        var root = new VBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddThemeConstantOverride("separation", 12);
        panel.AddChild(root);

        var title = new Label
        {
            Text = details.Title,
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        };
        ModalUiStyling.StyleTitle(title);
        root.AddChild(title);

        var subtitle = new Label
        {
            Text = details.Subtitle,
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        };
        ModalUiStyling.StyleBody(subtitle);
        root.AddChild(subtitle);

        var scroll = new ScrollContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddChild(scroll);

        scroll.AddChild(CreateDetailsBodyLabel(BuildDetailsBody(details)));

        var close = new Button { Text = "Close", CustomMinimumSize = new Vector2(180, 44) };
        ModalUiStyling.StyleButton(close);
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

    private static Label CreateDetailsBodyLabel(string text)
    {
        var body = new Label
        {
            Text = text,
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            CustomMinimumSize = new Vector2(GetDetailsBodyMinimumWidth(), 0)
        };
        ModalUiStyling.StyleBody(body, 20);
        return body;
    }

    private static float GetDetailsBodyMinimumWidth() => DetailsBodyMinimumWidth;

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
