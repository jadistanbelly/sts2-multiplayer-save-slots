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
    private Button? _deleteButton;
    private MultiplayerSavePickerRow? _selectedCampaign;
    private Button? _selectedCampaignButton;
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

        var panel = ModalUiStyling.CreatePanel(new Vector2(1020, 640), 510, 320);
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
            CustomMinimumSize = new Vector2(940, 450),
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        body.AddThemeConstantOverride("separation", 16);
        root.AddChild(body);

        body.AddChild(BuildCampaignList());
        body.AddChild(BuildPreviewPanel());

        var actions = new HBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        actions.AddThemeConstantOverride("separation", 10);
        root.AddChild(actions);

        var cancel = new Button { Text = "Cancel", CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44) };
        ModalUiStyling.StyleButton(cancel);
        cancel.Pressed += Close;
        actions.AddChild(cancel);

        actions.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

        if (_model.HasDeletedCampaigns)
        {
            var clearDeleted = new Button
            {
                Text = "Clear Deleted Saves",
                CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44)
            };
            ModalUiStyling.StyleDangerButton(clearDeleted);
            clearDeleted.Pressed += ShowClearDeletedConfirmation;
            actions.AddChild(clearDeleted);
        }

        _defaultFocusedControl ??= cancel;
    }

    private Control BuildCampaignList()
    {
        var frame = CreateCampaignListFrame();
        var list = new VBoxContainer
        {
            CustomMinimumSize = new Vector2(GetCampaignListRowWidth(), 430),
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        list.AddThemeConstantOverride("separation", 8);
        frame.AddChild(list);

        var startNewRow = _model.Rows.FirstOrDefault(row => row.Kind == PickerRowKind.StartNewRun)
            ?? MultiplayerSavePickerRow.StartNew();
        var startNew = CreateRowButton(startNewRow, new Vector2(GetCampaignListRowWidth(), 62));
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

        return frame;
    }

    private static PanelContainer CreateCampaignListFrame()
    {
        var frame = new PanelContainer
        {
            CustomMinimumSize = new Vector2(GetCampaignListFrameWidth(), 450),
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill,
            ClipContents = true
        };
        ModalUiStyling.StylePreviewFrame(frame);
        return frame;
    }

    private void AddCampaignRow(VBoxContainer rows, MultiplayerSavePickerRow row)
    {
        var button = new Button
        {
            Text = string.IsNullOrWhiteSpace(row.Subtitle) ? row.Title : $"{row.Title}\n{row.Subtitle}",
            CustomMinimumSize = new Vector2(GetCampaignListRowWidth(), 74),
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        ModalUiStyling.StyleButton(button);
        button.Pressed += () => SelectCampaignPreview(row);
        rows.AddChild(button);

        if (!string.IsNullOrWhiteSpace(row.CampaignId))
            _campaignButtons[row.CampaignId] = button;

        if (row.CampaignId == _selectedCampaign?.CampaignId)
        {
            _defaultFocusedControl = button;
            SetSelectedCampaignButton(button);
        }
    }

    private Control BuildPreviewPanel()
    {
        var frame = CreatePreviewFrame();
        _previewRoot = new VBoxContainer
        {
            CustomMinimumSize = new Vector2(GetPreviewContentWidth(), 430),
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill,
            ClipContents = true
        };
        _previewRoot.AddThemeConstantOverride("separation", 9);
        frame.AddChild(_previewRoot);
        RenderPreview(_selectedCampaign);
        return frame;
    }

    private static PanelContainer CreatePreviewFrame()
    {
        var frame = new PanelContainer
        {
            CustomMinimumSize = new Vector2(GetPreviewFrameWidth(), 450),
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill,
            ClipContents = true
        };
        ModalUiStyling.StylePreviewFrame(frame);
        return frame;
    }

    private static float GetCampaignListFrameWidth() => 400f;

    private static float GetCampaignListRowWidth() => 370f;

    private static float GetPreviewFrameWidth() => 530f;

    private static float GetPreviewContentWidth() => 510f;

    private static float GetActionButtonWidth() => 230f;

    private Button CreateRowButton(MultiplayerSavePickerRow row, Vector2 minimumSize)
    {
        var button = new Button
        {
            Text = string.IsNullOrWhiteSpace(row.Subtitle) ? row.Title : $"{row.Title}\n{row.Subtitle}",
            CustomMinimumSize = minimumSize,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        };
        if (row.Kind == PickerRowKind.StartNewRun)
            ModalUiStyling.StylePrimaryButton(button);
        else
            ModalUiStyling.StyleButton(button);
        button.Pressed += () => SelectRow(row);
        return button;
    }

    private void SelectCampaignPreview(MultiplayerSavePickerRow row)
    {
        _selectedCampaign = row.Kind == PickerRowKind.Campaign ? row : null;

        RenderPreview(_selectedCampaign);
        SetSelectedCampaignActionsEnabled();
        if (_selectedCampaign?.CampaignId is { } campaignId
            && _campaignButtons.TryGetValue(campaignId, out var button))
        {
            SetSelectedCampaignButton(button);
        }
    }

    private void SetSelectedCampaignButton(Button? button)
    {
        if (ReferenceEquals(_selectedCampaignButton, button))
            return;

        if (_selectedCampaignButton is not null)
            ModalUiStyling.StyleButton(_selectedCampaignButton);

        _selectedCampaignButton = button;
        if (_selectedCampaignButton is null)
            return;

        ModalUiStyling.StyleSelectedButton(_selectedCampaignButton);
        _selectedCampaignButton.GrabFocus();
    }

    private void ContinueSelectedCampaign()
    {
        if (_selectedCampaign is null)
            return;

        SelectRow(_selectedCampaign);
    }

    private Control CreateSelectedCampaignActions()
    {
        var actions = new HBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        actions.AddThemeConstantOverride("separation", 10);

        _deleteButton = new Button
        {
            Text = "Delete",
            CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44)
        };
        ModalUiStyling.StyleDangerButton(_deleteButton);
        _deleteButton.Pressed += ShowDeleteConfirmation;
        actions.AddChild(_deleteButton);

        actions.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

        _continueButton = new Button
        {
            Text = "Continue",
            CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44)
        };
        ModalUiStyling.StylePrimaryButton(_continueButton);
        _continueButton.Pressed += ContinueSelectedCampaign;
        actions.AddChild(_continueButton);

        SetSelectedCampaignActionsEnabled();
        return actions;
    }

    private void SetSelectedCampaignActionsEnabled()
    {
        var disabled = _selectedCampaign is null;
        if (_deleteButton is not null)
            _deleteButton.Disabled = disabled;
        if (_continueButton is not null)
            _continueButton.Disabled = disabled;
    }

    private void ShowDeleteConfirmation()
    {
        if (_selectedCampaign is null)
            return;

        ShowConfirmation(
            "Delete Save Slot?",
            $"Move this multiplayer save to deleted archives?\n\n{_selectedCampaign.Title}\n{_selectedCampaign.Subtitle}",
            "Delete",
            DeleteSelectedCampaign);
    }

    private void ShowClearDeletedConfirmation()
    {
        ShowConfirmation(
            "Clear Deleted Saves?",
            "Permanently remove all deleted multiplayer save archives. This cannot be undone.",
            "Clear Deleted Saves",
            ClearDeletedCampaigns);
    }

    private void ShowConfirmation(string titleText, string message, string confirmText, Action onConfirm)
    {
        CloseDetails();

        var overlay = new Control
        {
            Name = "MultiplayerSaveConfirmation",
            MouseFilter = MouseFilterEnum.Stop
        };
        overlay.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(overlay);
        _detailsOverlay = overlay;

        var panel = ModalUiStyling.CreatePanel(new Vector2(620, 330), 310, 165);
        overlay.AddChild(panel);

        var root = new VBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddThemeConstantOverride("separation", 16);
        panel.AddChild(root);

        var title = new Label
        {
            Text = titleText,
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        };
        ModalUiStyling.StyleTitle(title);
        root.AddChild(title);

        var body = CreatePreviewLabel(message, 20, HorizontalAlignment.Center);
        body.SizeFlagsVertical = SizeFlags.ExpandFill;
        root.AddChild(body);

        var actions = new HBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        actions.AddThemeConstantOverride("separation", 12);
        root.AddChild(actions);

        var cancel = new Button { Text = "Cancel", CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44) };
        ModalUiStyling.StyleButton(cancel);
        cancel.Pressed += CloseDetails;
        actions.AddChild(cancel);

        actions.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

        var confirm = new Button { Text = confirmText, CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44) };
        ModalUiStyling.StyleDangerButton(confirm);
        confirm.Pressed += () =>
        {
            CloseDetails();
            onConfirm();
        };
        actions.AddChild(confirm);
    }

    private void DeleteSelectedCampaign()
    {
        var campaignId = _selectedCampaign?.CampaignId;
        if (string.IsNullOrWhiteSpace(campaignId))
            return;

        var result = _controller.ArchiveCampaign(campaignId);
        if (!result.Success)
        {
            ShowError(result.ErrorMessage ?? "Unable to delete multiplayer save.");
            return;
        }

        RefreshPicker();
    }

    private void ClearDeletedCampaigns()
    {
        var result = _controller.ClearDeletedCampaigns();
        if (!result.Success)
        {
            ShowError(result.ErrorMessage ?? "Unable to clear deleted multiplayer saves.");
            return;
        }

        RefreshPicker();
    }

    private void RefreshPicker()
    {
        CloseDetails();
        Show(_controller, _model.GameMode);
    }

    private void RenderPreview(MultiplayerSavePickerRow? row)
    {
        if (_previewRoot is null)
            return;

        ClearChildren(_previewRoot);
        _continueButton = null;
        _deleteButton = null;

        if (row?.Details is null)
        {
            _previewRoot.AddChild(CreatePreviewLabel(MultiplayerSavePickerModel.EmptyPreviewTitle, 27, HorizontalAlignment.Center));
            _previewRoot.AddChild(CreatePreviewLabel(MultiplayerSavePickerModel.EmptyPreviewBody, 19, HorizontalAlignment.Center));
            return;
        }

        var details = row.Details;
        _previewRoot.AddChild(CreatePreviewLabel(details.Title, 27, HorizontalAlignment.Center));
        _previewRoot.AddChild(CreatePreviewLabel(details.Subtitle, 19, HorizontalAlignment.Center));

        var scroll = new ScrollContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        _previewRoot.AddChild(scroll);

        var content = CreatePreviewContent();
        scroll.AddChild(content);

        content.AddChild(CreatePreviewSectionTitle("Party"));
        foreach (var entry in details.RosterEntries.Count == 0
            ? details.RosterLines.Select(line => new MultiplayerSavePickerRosterEntry(line, null, "?", false))
            : details.RosterEntries)
        {
            content.AddChild(CreateRosterPreviewRow(entry));
        }

        content.AddChild(CreatePreviewSectionTitle("Run Details"));
        foreach (var line in details.SummaryLines)
            content.AddChild(CreatePreviewLabel(line, 16, HorizontalAlignment.Left));

        _previewRoot.AddChild(CreateSelectedCampaignActions());
    }

    private static VBoxContainer CreatePreviewContent()
    {
        var content = new VBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        content.AddThemeConstantOverride("separation", 7);
        return content;
    }

    private static Control CreateRosterPreviewRow(MultiplayerSavePickerRosterEntry entry)
    {
        var row = new HBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        row.AddThemeConstantOverride("separation", 8);

        if (entry.HasKnownPlayer)
            row.AddChild(CreateCharacterIndicator(entry.SelectedCharacterId, entry.BadgeText));

        var label = CreatePreviewLabel(entry.Text, 17, HorizontalAlignment.Left);
        label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        row.AddChild(label);
        return row;
    }

    private static Label CreatePreviewSectionTitle(string text)
    {
        var label = new Label
        {
            Text = text,
            HorizontalAlignment = HorizontalAlignment.Left,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        ModalUiStyling.StyleSectionTitle(label);
        return label;
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
            "CHARACTER.REGENT" => "res://images/ui/top_panel/character_icon_regent.png",
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

    private static Vector2 GetCharacterIconSize() => new(42, 42);

    private static TextureRect.ExpandModeEnum GetCharacterIconExpandMode() => TextureRect.ExpandModeEnum.IgnoreSize;

    private static Control CreateCharacterBadge(string badgeText)
    {
        var panel = new PanelContainer
        {
            CustomMinimumSize = new Vector2(38, 38),
            SizeFlagsHorizontal = SizeFlags.ShrinkCenter,
            SizeFlagsVertical = SizeFlags.ShrinkCenter
        };
        ModalUiStyling.StyleBadgePanel(panel);

        var label = new Label
        {
            Text = string.IsNullOrWhiteSpace(badgeText) ? "?" : badgeText,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            CustomMinimumSize = new Vector2(38, 38)
        };
        ModalUiStyling.StyleBody(label, 16);
        panel.AddChild(label);
        return panel;
    }

    private static TextureRect CreateCharacterIconTextureRect(Texture2D texture) =>
        new()
        {
            Texture = texture,
            ExpandMode = GetCharacterIconExpandMode(),
            StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
            CustomMinimumSize = GetCharacterIconSize(),
            Size = GetCharacterIconSize(),
            SizeFlagsHorizontal = SizeFlags.ShrinkBegin,
            SizeFlagsVertical = SizeFlags.ShrinkBegin
        };

    private static PanelContainer CreateCharacterIconSlot(Control child)
    {
        var slot = new PanelContainer
        {
            CustomMinimumSize = GetCharacterIconSize(),
            Size = GetCharacterIconSize(),
            SizeFlagsHorizontal = SizeFlags.ShrinkBegin,
            SizeFlagsVertical = SizeFlags.ShrinkBegin,
            ClipContents = true,
            MouseFilter = MouseFilterEnum.Ignore
        };
        ModalUiStyling.StyleIconSlotPanel(slot);
        slot.AddChild(child);
        return slot;
    }

    private static Control CreateCharacterIndicator(string? selectedCharacterId, string badgeText)
    {
        var texture = TryLoadCharacterIcon(selectedCharacterId);
        if (texture is null)
            return CreateCharacterIconSlot(CreateCharacterBadge(badgeText));

        return CreateCharacterIconSlot(CreateCharacterIconTextureRect(texture));
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
