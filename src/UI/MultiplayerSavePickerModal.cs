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
    private readonly bool _archivesView;
    private readonly Dictionary<string, Button> _campaignButtons = new(StringComparer.Ordinal);
    private Control? _defaultFocusedControl;
    private Control? _detailsOverlay;
    private VBoxContainer? _previewRoot;
    private Button? _archiveButton;
    private Button? _restoreButton;
    private Button? _continueButton;
    private Button? _deleteButton;
    private Button? _renameButton;
    private MultiplayerSavePickerRow? _selectedCampaign;
    private Button? _selectedCampaignButton;
    private bool _built;

    private MultiplayerSavePickerModal(
        HostFlowController controller,
        MultiplayerSavePickerModel model,
        bool archivesView = false)
    {
        _controller = controller;
        _model = model;
        _archivesView = archivesView;
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

    private static void ShowArchives(HostFlowController controller, MultiplayerGameMode gameMode)
    {
        var model = controller.BuildArchivePickerModel(gameMode);
        var container = NModalContainer.Instance
            ?? throw new InvalidOperationException("Modal container is not available.");
        var modal = new MultiplayerSavePickerModal(controller, model, archivesView: true);
        modal.BuildUi();
        container.Clear();
        GD.Print($"[MultiplayerSaveSlots] Opening archive picker for {gameMode} with {model.Rows.Count} rows.");
        container.Add(modal);
    }

    public override void _Ready() => BuildUi();

    private void BuildUi()
    {
        if (_built)
            return;

        _built = true;
        ModalUiStyling.PrepareModalRoot(this);
        _selectedCampaign = _archivesView ? _model.DefaultSelectedArchive : _model.DefaultSelectedCampaign;

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
            Text = _archivesView ? $"Archived Saves - {_model.GameMode}" : $"Multiplayer Saves - {_model.GameMode}",
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

        var actions = _archivesView ? BuildArchiveFooterActions() : BuildActiveFooterActions();
        root.AddChild(actions);

        _defaultFocusedControl ??= actions.GetChildren().OfType<Control>().FirstOrDefault();
    }

    private HBoxContainer BuildActiveFooterActions()
    {
        var actions = CreateFooterActions();

        var cancel = new Button { Text = "Cancel", CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44) };
        ModalUiStyling.StyleButton(cancel);
        cancel.Pressed += Close;
        actions.AddChild(cancel);
        _defaultFocusedControl ??= cancel;

        actions.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

        if (_model.HasDeletedCampaigns)
        {
            var archives = new Button
            {
                Text = "Archives",
                CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44)
            };
            ModalUiStyling.StyleButton(archives);
            archives.Pressed += () => ShowArchives(_controller, _model.GameMode);
            actions.AddChild(archives);
        }

        actions.AddChild(CreateFooterContinueButton());
        return actions;
    }

    private HBoxContainer BuildArchiveFooterActions()
    {
        var actions = CreateFooterActions();

        var back = new Button { Text = "Back", CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44) };
        ModalUiStyling.StyleButton(back);
        back.Pressed += RefreshPicker;
        actions.AddChild(back);
        _defaultFocusedControl ??= back;

        actions.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

        if (_model.HasDeletedCampaigns)
        {
            var deleteAll = new Button
            {
                Text = "Delete All Archives",
                CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44)
            };
            ModalUiStyling.StyleDangerButton(deleteAll);
            deleteAll.Pressed += ShowDeleteAllArchivesConfirmation;
            actions.AddChild(deleteAll);
        }

        return actions;
    }

    private static HBoxContainer CreateFooterActions()
    {
        var actions = new HBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        actions.AddThemeConstantOverride("separation", 10);
        return actions;
    }

    private Button CreateFooterContinueButton()
    {
        _continueButton = new Button
        {
            Text = "Continue",
            CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44)
        };
        ModalUiStyling.StylePrimaryButton(_continueButton);
        _continueButton.Pressed += ContinueSelectedCampaign;
        SetSelectedCampaignActionsEnabled();
        return _continueButton;
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

        if (!_archivesView)
        {
            var startNewRow = _model.Rows.FirstOrDefault(row => row.Kind == PickerRowKind.StartNewRun)
                ?? MultiplayerSavePickerRow.StartNew();
            var startNew = CreateRowButton(startNewRow, new Vector2(GetCampaignListRowWidth(), 62));
            startNew.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            list.AddChild(startNew);
            _defaultFocusedControl ??= startNew;
        }

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

        var pickerRows = _archivesView ? _model.ArchivedRows : _model.CampaignRows;
        foreach (var row in pickerRows)
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

        if (RowSelectionKey(row) is { } key)
            _campaignButtons[key] = button;

        if (IsSameSelection(row, _selectedCampaign))
        {
            _defaultFocusedControl = button;
            SetSelectedCampaignButton(button);
        }
    }

    private static string? RowSelectionKey(MultiplayerSavePickerRow row) =>
        row.Kind == PickerRowKind.ArchivedCampaign ? row.ArchiveKey : row.CampaignId;

    private static bool IsSameSelection(MultiplayerSavePickerRow row, MultiplayerSavePickerRow? selected) =>
        selected is not null &&
        row.Kind == selected.Kind &&
        string.Equals(RowSelectionKey(row), RowSelectionKey(selected), StringComparison.Ordinal);

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

    private static float GetPreviewTitleMinimumWidth() => 390f;

    private static Vector2 GetRenameIconButtonSize() => new(40, 34);

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
        _selectedCampaign = row.Kind is PickerRowKind.Campaign or PickerRowKind.ArchivedCampaign ? row : null;

        RenderPreview(_selectedCampaign);
        SetSelectedCampaignActionsEnabled();
        if (_selectedCampaign is { } selected
            && RowSelectionKey(selected) is { } key
            && _campaignButtons.TryGetValue(key, out var button))
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
        if (_selectedCampaign?.Kind != PickerRowKind.Campaign)
            return;

        SelectRow(_selectedCampaign);
    }

    private Control CreateActiveCampaignActions()
    {
        var actions = new HBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        actions.AddThemeConstantOverride("separation", 10);

        _archiveButton = new Button
        {
            Text = "Archive",
            CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44)
        };
        ModalUiStyling.StyleButton(_archiveButton);
        _archiveButton.Pressed += ShowArchiveConfirmation;
        actions.AddChild(_archiveButton);

        actions.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

        _deleteButton = new Button
        {
            Text = "Delete",
            CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44)
        };
        ModalUiStyling.StyleDangerButton(_deleteButton);
        _deleteButton.Pressed += ShowActiveDeleteConfirmation;
        actions.AddChild(_deleteButton);

        SetSelectedCampaignActionsEnabled();
        return actions;
    }

    private Control CreateArchivedCampaignActions()
    {
        var actions = new HBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        actions.AddThemeConstantOverride("separation", 10);

        _restoreButton = new Button
        {
            Text = "Restore",
            CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44)
        };
        ModalUiStyling.StylePrimaryButton(_restoreButton);
        _restoreButton.Pressed += RestoreSelectedArchive;
        actions.AddChild(_restoreButton);

        actions.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

        _deleteButton = new Button
        {
            Text = "Delete",
            CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44)
        };
        ModalUiStyling.StyleDangerButton(_deleteButton);
        _deleteButton.Pressed += ShowArchivedDeleteConfirmation;
        actions.AddChild(_deleteButton);

        SetSelectedCampaignActionsEnabled();
        return actions;
    }

    private void SetSelectedCampaignActionsEnabled()
    {
        var activeDisabled = _selectedCampaign?.Kind != PickerRowKind.Campaign;
        var archivedDisabled = _selectedCampaign?.Kind != PickerRowKind.ArchivedCampaign;
        if (_archiveButton is not null)
            _archiveButton.Disabled = activeDisabled;
        if (_restoreButton is not null)
            _restoreButton.Disabled = archivedDisabled;
        if (_deleteButton is not null)
            _deleteButton.Disabled = _archivesView ? archivedDisabled : activeDisabled;
        if (_continueButton is not null)
            _continueButton.Disabled = activeDisabled;
    }

    private void ShowArchiveConfirmation()
    {
        if (_selectedCampaign?.Kind != PickerRowKind.Campaign)
            return;

        ShowConfirmation(
            "Archive Save Slot?",
            $"Move this multiplayer save to Archives?\nYou can restore it later.\n\n{_selectedCampaign.Title}\n{_selectedCampaign.Subtitle}",
            "Archive",
            destructive: false,
            ArchiveSelectedCampaign);
    }

    private void ShowActiveDeleteConfirmation()
    {
        if (_selectedCampaign?.Kind != PickerRowKind.Campaign)
            return;

        ShowConfirmation(
            "Delete Save Permanently?",
            $"Permanently delete this multiplayer save?\nThis cannot be undone.\n\n{_selectedCampaign.Title}\n{_selectedCampaign.Subtitle}",
            "Delete",
            destructive: true,
            DeleteSelectedCampaign);
    }

    private void ShowArchivedDeleteConfirmation()
    {
        if (_selectedCampaign?.Kind != PickerRowKind.ArchivedCampaign)
            return;

        ShowConfirmation(
            "Delete Archived Save?",
            $"Permanently delete this archived multiplayer save?\nThis cannot be undone.\n\n{_selectedCampaign.Title}\n{_selectedCampaign.Subtitle}",
            "Delete",
            destructive: true,
            DeleteSelectedArchive);
    }

    private void ShowDeleteAllArchivesConfirmation()
    {
        ShowConfirmation(
            "Delete All Archives?",
            "Permanently remove all archived multiplayer saves. This cannot be undone.",
            "Delete All Archives",
            destructive: true,
            ClearDeletedCampaigns);
    }

    private void ShowConfirmation(
        string titleText,
        string message,
        string confirmText,
        bool destructive,
        Action onConfirm)
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
        if (destructive)
            ModalUiStyling.StyleDangerButton(confirm);
        else
            ModalUiStyling.StylePrimaryButton(confirm);
        confirm.Pressed += () =>
        {
            CloseDetails();
            onConfirm();
        };
        actions.AddChild(confirm);
    }

    private void ShowRenameModal()
    {
        if (_selectedCampaign?.Kind != PickerRowKind.Campaign)
            return;

        CloseDetails();

        var overlay = new Control
        {
            Name = "MultiplayerSaveRename",
            MouseFilter = MouseFilterEnum.Stop
        };
        overlay.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(overlay);
        _detailsOverlay = overlay;

        var panel = ModalUiStyling.CreatePanel(new Vector2(620, 270), 310, 135);
        overlay.AddChild(panel);

        var root = new VBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddThemeConstantOverride("separation", 14);
        panel.AddChild(root);

        var title = new Label
        {
            Text = "Rename Save Slot",
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        };
        ModalUiStyling.StyleTitle(title);
        root.AddChild(title);

        var input = new LineEdit
        {
            Text = _selectedCampaign.Title,
            PlaceholderText = "Run name"
        };
        ModalUiStyling.StyleTextInput(input);
        root.AddChild(input);

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

        var save = new Button { Text = "Save", CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44) };
        ModalUiStyling.StylePrimaryButton(save);
        save.Pressed += () => RenameSelectedCampaign(input.Text);
        actions.AddChild(save);

        input.GrabFocus();
        input.SelectAll();
    }

    private void ArchiveSelectedCampaign()
    {
        var campaignId = _selectedCampaign?.CampaignId;
        if (string.IsNullOrWhiteSpace(campaignId))
            return;

        var result = _controller.ArchiveCampaign(campaignId);
        if (!result.Success)
        {
            ShowError(result.ErrorMessage ?? "Unable to archive multiplayer save.");
            return;
        }

        RefreshPicker();
    }

    private void RenameSelectedCampaign(string? customName)
    {
        var campaignId = _selectedCampaign?.CampaignId;
        if (string.IsNullOrWhiteSpace(campaignId))
            return;

        var result = _controller.RenameCampaign(campaignId, customName);
        if (!result.Success)
        {
            ShowError(result.ErrorMessage ?? "Unable to rename multiplayer save.");
            return;
        }

        RefreshPicker();
    }

    private void DeleteSelectedCampaign()
    {
        var campaignId = _selectedCampaign?.CampaignId;
        if (string.IsNullOrWhiteSpace(campaignId))
            return;

        var result = _controller.DeleteCampaign(campaignId);
        if (!result.Success)
        {
            ShowError(result.ErrorMessage ?? "Unable to delete multiplayer save.");
            return;
        }

        RefreshPicker();
    }

    private void RestoreSelectedArchive()
    {
        var archiveKey = _selectedCampaign?.ArchiveKey;
        if (string.IsNullOrWhiteSpace(archiveKey))
            return;

        var result = _controller.RestoreArchivedCampaign(archiveKey);
        if (!result.Success)
        {
            ShowError(result.ErrorMessage ?? "Unable to restore archived multiplayer save.");
            return;
        }

        RefreshPicker();
    }

    private void DeleteSelectedArchive()
    {
        var archiveKey = _selectedCampaign?.ArchiveKey;
        if (string.IsNullOrWhiteSpace(archiveKey))
            return;

        var result = _controller.DeleteArchivedCampaign(archiveKey);
        if (!result.Success)
        {
            ShowError(result.ErrorMessage ?? "Unable to delete archived multiplayer save.");
            return;
        }

        RefreshArchivePicker();
    }

    private void ClearDeletedCampaigns()
    {
        var result = _controller.ClearDeletedCampaigns();
        if (!result.Success)
        {
            ShowError(result.ErrorMessage ?? "Unable to delete archived multiplayer saves.");
            return;
        }

        if (_archivesView)
            RefreshArchivePicker();
        else
            RefreshPicker();
    }

    private void RefreshPicker()
    {
        CloseDetails();
        Show(_controller, _model.GameMode);
    }

    private void RefreshArchivePicker()
    {
        CloseDetails();
        ShowArchives(_controller, _model.GameMode);
    }

    private void RenderPreview(MultiplayerSavePickerRow? row)
    {
        if (_previewRoot is null)
            return;

        ClearChildren(_previewRoot);
        _deleteButton = null;
        _archiveButton = null;
        _restoreButton = null;
        _renameButton = null;

        if (row?.Details is null)
        {
            var emptyTitle = _archivesView
                ? MultiplayerSavePickerModel.EmptyArchiveTitle
                : MultiplayerSavePickerModel.EmptyPreviewTitle;
            var emptyBody = _archivesView
                ? MultiplayerSavePickerModel.EmptyArchiveBody
                : MultiplayerSavePickerModel.EmptyPreviewBody;
            _previewRoot.AddChild(CreatePreviewLabel(emptyTitle, 27, HorizontalAlignment.Center));
            _previewRoot.AddChild(CreatePreviewLabel(emptyBody, 19, HorizontalAlignment.Center));
            return;
        }

        var details = row.Details;
        _previewRoot.AddChild(CreatePreviewTitleRow(row, details));
        if (!string.IsNullOrWhiteSpace(details.AutoLabel))
            _previewRoot.AddChild(CreatePreviewLabel(details.AutoLabel, 17, HorizontalAlignment.Center));
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

        if (row.Kind == PickerRowKind.ArchivedCampaign)
            _previewRoot.AddChild(CreateArchivedCampaignActions());
        else
            _previewRoot.AddChild(CreateActiveCampaignActions());
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

    private Control CreatePreviewTitleRow(MultiplayerSavePickerRow row, MultiplayerSavePickerDetails details)
    {
        var canRename = row.Kind == PickerRowKind.Campaign;
        var titleRow = new HBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            Alignment = BoxContainer.AlignmentMode.Center
        };
        titleRow.AddThemeConstantOverride("separation", 8);

        if (canRename)
            titleRow.AddChild(CreateRenameIconSpacer());

        var title = CreatePreviewLabel(details.Title, 27, HorizontalAlignment.Center);
        title.CustomMinimumSize = new Vector2(GetPreviewTitleMinimumWidth(), 0);
        title.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        titleRow.AddChild(title);

        if (canRename)
        {
            _renameButton = CreateRenameIconButton();
            titleRow.AddChild(_renameButton);
        }

        return titleRow;
    }

    private static Control CreateRenameIconSpacer() =>
        new()
        {
            CustomMinimumSize = GetRenameIconButtonSize(),
            SizeFlagsHorizontal = SizeFlags.ShrinkBegin,
            SizeFlagsVertical = SizeFlags.ShrinkCenter
        };

    private Button CreateRenameIconButton()
    {
        var button = new Button
        {
            Text = "✎",
            TooltipText = "Rename",
            CustomMinimumSize = GetRenameIconButtonSize(),
            SizeFlagsHorizontal = SizeFlags.ShrinkBegin,
            SizeFlagsVertical = SizeFlags.ShrinkCenter
        };
        ModalUiStyling.StyleButton(button);
        button.AddThemeFontSizeOverride("font_size", 18);
        button.Pressed += ShowRenameModal;
        return button;
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

    private static string BuildDetailsBody(MultiplayerSavePickerDetails details)
    {
        var summary = string.Join('\n', details.SummaryLines);
        var roster = $"Roster\n{string.Join('\n', details.RosterLines)}";
        return string.IsNullOrWhiteSpace(details.AutoLabel)
            ? $"{summary}\n\n{roster}"
            : $"{details.AutoLabel}\n\n{summary}\n\n{roster}";
    }

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
