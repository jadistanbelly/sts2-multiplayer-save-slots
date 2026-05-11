using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;
using MultiplayerSaveSlots.UI;

namespace MultiplayerSaveSlots.Tests;

public static class HostFlowControllerTests
{
    public static IEnumerable<TestCase> All()
    {
        yield return new TestCase("runtime paths place bank beside active multiplayer save", RuntimePathsPlaceBankBesideActiveSave);
        yield return new TestCase("runtime paths globalize STS2 user save paths", RuntimePathsGlobalizeSts2UserSavePaths);
        yield return new TestCase("host flow session tracks existing campaign selection", SessionTracksExistingCampaignSelection);
        yield return new TestCase("host flow session tracks pending new run", SessionTracksPendingNewRun);
        yield return new TestCase("host flow session clears selected and pending state", SessionClearsSelectedAndPendingState);
        yield return new TestCase("controller builds picker model with start new and campaign rows", ControllerBuildsPickerModel);
        yield return new TestCase("STS2 save bank adapter repairs listed campaign character ids from payload", SaveBankAdapterRepairsListedCampaignCharacterIdsFromPayload);
        yield return new TestCase("STS2 save bank adapter repairs listed campaign progress from payload", SaveBankAdapterRepairsListedCampaignProgressFromPayload);
        yield return new TestCase("controller picker model exposes deleted save availability", ControllerPickerModelExposesDeletedSaveAvailability);
        yield return new TestCase("controller builds archive picker model with archived rows", ControllerBuildsArchivePickerModel);
        yield return new TestCase("picker model exposes default selected campaign", PickerModelExposesDefaultSelectedCampaign);
        yield return new TestCase("picker model exposes default selected archive", PickerModelExposesDefaultSelectedArchive);
        yield return new TestCase("picker model describes empty campaign list", PickerModelDescribesEmptyCampaignList);
        yield return new TestCase("picker character badge labels are stable", PickerCharacterBadgeLabelsAreStable);
        yield return new TestCase("picker roster entries use player badge fallback", PickerRosterEntriesUsePlayerBadgeFallback);
        yield return new TestCase("controller disambiguates duplicate picker rows", ControllerDisambiguatesDuplicatePickerRows);
        yield return new TestCase("picker subtitle omits unknown progress", PickerSubtitleOmitsUnknownProgress);
        yield return new TestCase("picker campaign row includes full details", PickerCampaignRowIncludesFullDetails);
        yield return new TestCase("picker campaign row uses custom run name", PickerCampaignRowUsesCustomRunName);
        yield return new TestCase("picker campaign row falls back when custom run name is cleared", PickerCampaignRowFallsBackWhenCustomRunNameCleared);
        yield return new TestCase("picker details show selected characters", PickerDetailsShowSelectedCharacters);
        yield return new TestCase("picker details handle missing progress and roster", PickerDetailsHandleMissingProgressAndRoster);
        yield return new TestCase("picker start new row has no details", PickerStartNewRowHasNoDetails);
        yield return new TestCase("compatibility checker allows matching stable ids", CompatibilityCheckerAllowsMatchingStableIds);
        yield return new TestCase("compatibility checker warns for missing expected players", CompatibilityCheckerWarnsForMissingExpectedPlayers);
        yield return new TestCase("compatibility checker warns for extra current players", CompatibilityCheckerWarnsForExtraCurrentPlayers);
        yield return new TestCase("compatibility checker skips empty expected roster", CompatibilityCheckerSkipsEmptyExpectedRoster);
        yield return new TestCase("compatibility checker skips roster without stable ids", CompatibilityCheckerSkipsRosterWithoutStableIds);
        yield return new TestCase("compatibility checker warning key is stable", CompatibilityCheckerWarningKeyIsStable);
        yield return new TestCase("compatibility guard warns once for identical mismatch", CompatibilityGuardWarnsOnceForIdenticalMismatch);
        yield return new TestCase("compatibility guard acknowledges after warning display", CompatibilityGuardAcknowledgesAfterWarningDisplay);
        yield return new TestCase("compatibility guard allows without selected campaign", CompatibilityGuardAllowsWithoutSelectedCampaign);
        yield return new TestCase("controller starts new run through continuation", ControllerStartsNewRunThroughContinuation);
        yield return new TestCase("controller does not start new run when active preflight fails", ControllerStopsStartNewWhenPreflightFails);
        yield return new TestCase("controller does not select session when start new continuation fails", ControllerStopsSessionSelectionWhenStartNewFails);
        yield return new TestCase("controller activates existing campaign before load continuation", ControllerActivatesExistingCampaign);
        yield return new TestCase("controller repairs metadata after existing campaign activation", ControllerRepairsMetadataAfterExistingCampaignActivation);
        yield return new TestCase("controller continues existing campaign when metadata repair fails", ControllerContinuesExistingCampaignWhenMetadataRepairFails);
        yield return new TestCase("controller does not activate existing campaign when active preflight fails", ControllerStopsExistingCampaignWhenActivePreflightFails);
        yield return new TestCase("controller does not activate existing campaign when load preflight fails", ControllerStopsExistingCampaignWhenLoadPreflightFails);
        yield return new TestCase("controller does not continue when activation fails", ControllerStopsWhenActivationFails);
        yield return new TestCase("controller restores previous active save when existing load continuation fails", ControllerRestoresPreviousActiveWhenLoadFails);
        yield return new TestCase("controller reports rollback failure after existing load continuation fails", ControllerReportsRollbackFailureWhenLoadFails);
        yield return new TestCase("controller does not select session when existing load continuation fails", ControllerStopsSessionSelectionWhenLoadFails);
        yield return new TestCase("active save activator maps exceptions to failed result", ActiveSaveActivatorMapsExceptions);
        yield return new TestCase("save sync no-ops without selected campaign", SaveSyncNoOpsWithoutSelection);
        yield return new TestCase("save sync syncs existing selected campaign", SaveSyncSyncsExistingSelection);
        yield return new TestCase("save sync finalizes pending new run", SaveSyncFinalizesPendingNewRun);
        yield return new TestCase("save sync finalizes pending new run with metadata", SaveSyncFinalizesPendingNewRunWithMetadata);
        yield return new TestCase("save sync keeps pending new run selected when finalization fails", SaveSyncKeepsPendingNewRunWhenFinalizationFails);
        yield return new TestCase("save sync maps sync exceptions to failed result", SaveSyncMapsExceptions);
        yield return new TestCase("controller exposes recovery model", ControllerExposesRecoveryModel);
        yield return new TestCase("controller duplicates unmanaged active save without starting new run", ControllerDuplicatesUnmanagedActiveSaveWithoutStartingNewRun);
        yield return new TestCase("controller syncs managed active save then starts new run", ControllerSyncsManagedActiveSaveThenStartsNewRun);
        yield return new TestCase("controller recovers then selects existing campaign", ControllerRecoversThenSelectsExistingCampaign);
        yield return new TestCase("controller stops when recovery fails", ControllerStopsWhenRecoveryFails);
        yield return new TestCase("recovery model reports unavailable when no options exist", RecoveryModelReportsUnavailableWhenNoOptionsExist);
        yield return new TestCase("controller archives selected campaign", ControllerArchivesSelectedCampaign);
        yield return new TestCase("controller reports archive campaign failure", ControllerReportsArchiveCampaignFailure);
        yield return new TestCase("controller renames selected campaign", ControllerRenamesSelectedCampaign);
        yield return new TestCase("controller reports rename campaign failure", ControllerReportsRenameCampaignFailure);
        yield return new TestCase("controller restores archived campaign", ControllerRestoresArchivedCampaign);
        yield return new TestCase("controller reports restore archived campaign failure", ControllerReportsRestoreArchivedCampaignFailure);
        yield return new TestCase("controller permanently deletes active campaign", ControllerPermanentlyDeletesActiveCampaign);
        yield return new TestCase("controller reports permanent active delete failure", ControllerReportsPermanentActiveDeleteFailure);
        yield return new TestCase("controller permanently deletes archived campaign", ControllerPermanentlyDeletesArchivedCampaign);
        yield return new TestCase("controller reports permanent archived delete failure", ControllerReportsPermanentArchivedDeleteFailure);
        yield return new TestCase("controller clears deleted campaigns", ControllerClearsDeletedCampaigns);
        yield return new TestCase("controller reports clear deleted failure", ControllerReportsClearDeletedFailure);
    }

    private static void RuntimePathsPlaceBankBesideActiveSave()
    {
        var saveDirectory = Path.Combine(Path.GetTempPath(), "sts2", "profile1", "saves");
        var activeSavePath = Path.Combine(saveDirectory, "current_run_mp.save");

        var paths = MultiplayerSaveRuntimePaths.FromActiveSavePath(activeSavePath);

        AssertEx.Equal(activeSavePath, paths.ActiveSavePath);
        AssertEx.Equal(Path.Combine(saveDirectory, "MultiplayerSaveSlots"), paths.BankRootDirectory);
        AssertEx.Equal(Path.Combine(saveDirectory, "MultiplayerSaveSlots", "active-state.json"), paths.ActiveStatePath);
    }

    private static void RuntimePathsGlobalizeSts2UserSavePaths()
    {
        const string activeSavePath = "profile1/saves/current_run_mp.save";
        const string storeFullPath = "user://steam/76561198851319384/profile1/saves/current_run_mp.save";
        var globalizedPath = Path.Combine(
            Path.GetTempPath(),
            "SlayTheSpire2",
            "steam",
            "76561198851319384",
            "profile1",
            "saves",
            "current_run_mp.save");

        var paths = MultiplayerSaveRuntimePaths.FromSts2ActiveSavePath(
            activeSavePath,
            path => path == activeSavePath ? storeFullPath : path,
            path => path == storeFullPath ? globalizedPath : path);

        AssertEx.Equal(globalizedPath, paths.ActiveSavePath);
        AssertEx.Equal(Path.Combine(Path.GetDirectoryName(globalizedPath)!, "MultiplayerSaveSlots"), paths.BankRootDirectory);
    }

    private static void SessionTracksExistingCampaignSelection()
    {
        var session = new HostFlowSession();

        session.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", session.SelectedCampaignId);
        AssertEx.Equal(MultiplayerGameMode.Standard, session.SelectedGameMode);
        AssertEx.False(session.IsPendingNewRun);
    }

    private static void SessionTracksPendingNewRun()
    {
        var session = new HostFlowSession();

        session.SelectNewRun(MultiplayerGameMode.Custom);

        AssertEx.Equal(null, session.SelectedCampaignId);
        AssertEx.Equal(MultiplayerGameMode.Custom, session.SelectedGameMode);
        AssertEx.True(session.IsPendingNewRun);
    }

    private static void SessionClearsSelectedAndPendingState()
    {
        var session = new HostFlowSession();

        session.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);
        session.Clear();

        AssertEx.Equal(null, session.SelectedCampaignId);
        AssertEx.Equal(null, session.SelectedGameMode);
        AssertEx.False(session.IsPendingNewRun);

        session.SelectNewRun(MultiplayerGameMode.Custom);
        session.Clear();

        AssertEx.Equal(null, session.SelectedCampaignId);
        AssertEx.Equal(null, session.SelectedGameMode);
        AssertEx.False(session.IsPendingNewRun);
    }

    private static void ControllerBuildsPickerModel()
    {
        var bank = new FakeHostFlowSaveBank
        {
            Campaigns =
            [
                new CampaignMetadata(
                    "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                    MultiplayerGameMode.Standard,
                    "buddy1 + buddy2",
                    [new PlayerIdentity("1", "buddy1"), new PlayerIdentity("2", "buddy2")],
                    DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
                    DateTimeOffset.Parse("2026-05-08T01:00:00Z"),
                    null,
                    "checksum",
                    "Floor 7")
            ]
        };
        var controller = CreateController(bank);

        var model = controller.BuildPickerModel(MultiplayerGameMode.Standard);

        AssertEx.Equal(MultiplayerGameMode.Standard, model.GameMode);
        AssertEx.Equal(2, model.Rows.Count);
        AssertEx.Equal(PickerRowKind.StartNewRun, model.Rows[0].Kind);
        AssertEx.Equal("Start New Run", model.Rows[0].Title);
        AssertEx.Equal(PickerRowKind.Campaign, model.Rows[1].Kind);
        AssertEx.Equal("buddy1 + buddy2", model.Rows[1].Title);
        AssertEx.Equal("Floor 7 - 2 players", model.Rows[1].Subtitle);
    }

    private static void SaveBankAdapterRepairsListedCampaignCharacterIdsFromPayload()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        File.WriteAllText(
            source,
            "{\"platform_type\":\"steam\",\"players\":[{\"net_id\":111,\"character_id\":\"CHARACTER.SILENT\"},{\"net_id\":222,\"character_id\":\"CHARACTER.IRONCLAD\"}]}");
        var bank = new Storage.MultiplayerSaveBank(new Storage.SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [new PlayerIdentity("Steam:111", "Alice"), new PlayerIdentity("Steam:222", "Bob")],
            source,
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            "Floor 5"));
        var adapter = new Sts2SaveBankAdapter(bank);

        var campaigns = adapter.ListCampaigns(MultiplayerGameMode.Standard);

        AssertEx.Equal(1, campaigns.Count);
        AssertEx.Equal(metadata.CampaignId, campaigns[0].CampaignId);
        AssertEx.Equal("CHARACTER.SILENT", campaigns[0].Roster[0].SelectedCharacterId);
        AssertEx.Equal("CHARACTER.IRONCLAD", campaigns[0].Roster[1].SelectedCharacterId);
        AssertEx.Equal("CHARACTER.SILENT", bank.GetCampaign(metadata.CampaignId).Roster[0].SelectedCharacterId);
    }

    private static void SaveBankAdapterRepairsListedCampaignProgressFromPayload()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        File.WriteAllText(
            source,
            """
            {
              "platform_type": "steam",
              "current_act_index": 0,
              "map_point_history": [["node-1"]],
              "players": []
            }
            """);
        var bank = new Storage.MultiplayerSaveBank(new Storage.SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            source,
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            "Floor 2"));
        var adapter = new Sts2SaveBankAdapter(bank);

        var campaigns = adapter.ListCampaigns(MultiplayerGameMode.Standard);

        AssertEx.Equal(1, campaigns.Count);
        AssertEx.Equal("Act 1 - Floor 2", campaigns[0].ActOrFloor);
        AssertEx.Equal("Act 1 - Floor 2", bank.GetCampaign(metadata.CampaignId).ActOrFloor);
    }

    private static void ControllerPickerModelExposesDeletedSaveAvailability()
    {
        var bank = new FakeHostFlowSaveBank { HasDeleted = true };

        var model = CreateController(bank).BuildPickerModel(MultiplayerGameMode.Standard);

        AssertEx.True(model.HasDeletedCampaigns);
    }

    private static void ControllerBuildsArchivePickerModel()
    {
        var bank = new FakeHostFlowSaveBank
        {
            ArchivedCampaigns =
            [
                new ArchivedCampaign(
                    "20260510123456-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                    new CampaignMetadata(
                        "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                        MultiplayerGameMode.Standard,
                        "Alice, Bob",
                        [new PlayerIdentity("steam:1", "Alice"), new PlayerIdentity("steam:2", "Bob")],
                        DateTimeOffset.Parse("2026-05-09T20:00:00Z"),
                        DateTimeOffset.Parse("2026-05-09T20:30:00Z"),
                        null,
                        "checksum",
                        "Floor 3"))
            ]
        };

        var model = CreateController(bank).BuildArchivePickerModel(MultiplayerGameMode.Standard);

        AssertEx.Equal(MultiplayerGameMode.Standard, model.GameMode);
        AssertEx.Equal(1, model.Rows.Count);
        AssertEx.Equal(PickerRowKind.ArchivedCampaign, model.Rows[0].Kind);
        AssertEx.Equal("Alice, Bob", model.Rows[0].Title);
        AssertEx.Equal("Floor 3 - 2 players", model.Rows[0].Subtitle);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", model.Rows[0].CampaignId);
        AssertEx.Equal("20260510123456-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", model.Rows[0].ArchiveKey);
    }

    private static void PickerModelExposesDefaultSelectedCampaign()
    {
        var model = new MultiplayerSavePickerModel(
            MultiplayerGameMode.Standard,
            [
                MultiplayerSavePickerRow.StartNew(),
                MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
                    "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                    MultiplayerGameMode.Standard,
                    "Alice, Bob",
                    [new PlayerIdentity("steam:1", "Alice"), new PlayerIdentity("steam:2", "Bob")],
                    DateTimeOffset.Parse("2026-05-09T20:00:00Z"),
                    DateTimeOffset.Parse("2026-05-09T20:30:00Z"),
                    null,
                    "checksum",
                    "Floor 3"))
            ]);

        var campaignRowsProperty = typeof(MultiplayerSavePickerModel).GetProperty("CampaignRows")
            ?? throw new InvalidOperationException("CampaignRows helper was not found");
        var defaultSelectedProperty = typeof(MultiplayerSavePickerModel).GetProperty("DefaultSelectedCampaign")
            ?? throw new InvalidOperationException("DefaultSelectedCampaign helper was not found");

        var campaignRows = (IReadOnlyList<MultiplayerSavePickerRow>)campaignRowsProperty.GetValue(model)!;
        var defaultSelected = (MultiplayerSavePickerRow?)defaultSelectedProperty.GetValue(model);

        AssertEx.Equal(1, campaignRows.Count);
        AssertEx.Equal("Alice, Bob", campaignRows[0].Title);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", defaultSelected?.CampaignId);
    }

    private static void PickerModelExposesDefaultSelectedArchive()
    {
        var model = new MultiplayerSavePickerModel(
            MultiplayerGameMode.Standard,
            [
                MultiplayerSavePickerRow.ArchivedCampaign(new ArchivedCampaign(
                    "20260510123456-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                    new CampaignMetadata(
                        "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                        MultiplayerGameMode.Standard,
                        "Alice, Bob",
                        [new PlayerIdentity("steam:1", "Alice"), new PlayerIdentity("steam:2", "Bob")],
                        DateTimeOffset.Parse("2026-05-09T20:00:00Z"),
                        DateTimeOffset.Parse("2026-05-09T20:30:00Z"),
                        null,
                        "checksum",
                        "Floor 3")))
            ]);

        var archivedRowsProperty = typeof(MultiplayerSavePickerModel).GetProperty("ArchivedRows")
            ?? throw new InvalidOperationException("ArchivedRows helper was not found");
        var defaultSelectedProperty = typeof(MultiplayerSavePickerModel).GetProperty("DefaultSelectedArchive")
            ?? throw new InvalidOperationException("DefaultSelectedArchive helper was not found");

        var archivedRows = (IReadOnlyList<MultiplayerSavePickerRow>)archivedRowsProperty.GetValue(model)!;
        var defaultSelected = (MultiplayerSavePickerRow?)defaultSelectedProperty.GetValue(model);

        AssertEx.Equal(1, archivedRows.Count);
        AssertEx.Equal("20260510123456-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", archivedRows[0].ArchiveKey);
        AssertEx.Equal("20260510123456-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", defaultSelected?.ArchiveKey);
    }


    private static void PickerModelDescribesEmptyCampaignList()
    {
        var model = new MultiplayerSavePickerModel(MultiplayerGameMode.Standard, [MultiplayerSavePickerRow.StartNew()]);
        var campaignRowsProperty = typeof(MultiplayerSavePickerModel).GetProperty("CampaignRows")
            ?? throw new InvalidOperationException("CampaignRows helper was not found");
        var defaultSelectedProperty = typeof(MultiplayerSavePickerModel).GetProperty("DefaultSelectedCampaign")
            ?? throw new InvalidOperationException("DefaultSelectedCampaign helper was not found");
        var emptyTitleProperty = typeof(MultiplayerSavePickerModel).GetProperty("EmptyPreviewTitle")
            ?? throw new InvalidOperationException("EmptyPreviewTitle helper was not found");
        var emptyBodyProperty = typeof(MultiplayerSavePickerModel).GetProperty("EmptyPreviewBody")
            ?? throw new InvalidOperationException("EmptyPreviewBody helper was not found");

        var campaignRows = (IReadOnlyList<MultiplayerSavePickerRow>)campaignRowsProperty.GetValue(model)!;
        var defaultSelected = (MultiplayerSavePickerRow?)defaultSelectedProperty.GetValue(model);

        AssertEx.Equal(0, campaignRows.Count);
        AssertEx.Equal(null, defaultSelected);
        AssertEx.Equal("No saved runs", emptyTitleProperty.GetValue(null));
        AssertEx.Equal("Start a new multiplayer run to create the first save slot.", emptyBodyProperty.GetValue(null));
    }

    private static void PickerCharacterBadgeLabelsAreStable()
    {
        var badgeText = typeof(MultiplayerSavePickerModel).GetMethod("CharacterBadgeText")
            ?? throw new InvalidOperationException("CharacterBadgeText helper was not found");

        AssertEx.Equal("IC", badgeText.Invoke(null, ["CHARACTER.IRONCLAD"]));
        AssertEx.Equal("SI", badgeText.Invoke(null, ["CHARACTER.SILENT"]));
        AssertEx.Equal("DE", badgeText.Invoke(null, ["CHARACTER.DEFECT"]));
        AssertEx.Equal("NE", badgeText.Invoke(null, ["CHARACTER.NECROBINDER"]));
        AssertEx.Equal("RG", badgeText.Invoke(null, ["CHARACTER.REGENT"]));
        AssertEx.Equal("??", badgeText.Invoke(null, ["CHARACTER.UNKNOWN"]));
        AssertEx.Equal("??", badgeText.Invoke(null, [null]));
    }

    private static void PickerRosterEntriesUsePlayerBadgeFallback()
    {
        var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            MultiplayerGameMode.Standard,
            "Alice",
            [new PlayerIdentity("Steam:1", "Alice")],
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            null,
            "checksum",
            "Floor 1"));

        var details = row.Details ?? throw new InvalidOperationException("Expected campaign details");
        AssertEx.Equal("A", details.RosterEntries[0].BadgeText);
    }

    private static void ControllerDisambiguatesDuplicatePickerRows()
    {
        var roster = new[]
        {
            new PlayerIdentity("steam:1", "phatstatss"),
            new PlayerIdentity("steam:2", "Magical Crocs")
        };
        var bank = new FakeHostFlowSaveBank
        {
            Campaigns =
            [
                new CampaignMetadata(
                    "11111111111111111111111111111111",
                    MultiplayerGameMode.Standard,
                    "phatstatss, Magical Crocs",
                    roster,
                    DateTimeOffset.Parse("2026-05-09T20:58:00Z"),
                    DateTimeOffset.Parse("2026-05-09T21:10:00Z"),
                    null,
                    "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                    "Floor 3"),
                new CampaignMetadata(
                    "22222222222222222222222222222222",
                    MultiplayerGameMode.Standard,
                    "phatstatss, Magical Crocs",
                    roster,
                    DateTimeOffset.Parse("2026-05-09T21:24:00Z"),
                    DateTimeOffset.Parse("2026-05-09T21:30:00Z"),
                    null,
                    "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb",
                    "Floor 3")
            ]
        };

        var model = CreateController(bank).BuildPickerModel(MultiplayerGameMode.Standard);

        AssertEx.Equal("Floor 3 - 2 players - ID 11111111", model.Rows[1].Subtitle);
        AssertEx.Equal("Floor 3 - 2 players - ID 22222222", model.Rows[2].Subtitle);
        AssertEx.Equal("Floor 3 - 2 players - ID 11111111", model.Rows[1].Details?.Subtitle);
        AssertEx.Equal("Floor 3 - 2 players - ID 22222222", model.Rows[2].Details?.Subtitle);
    }

    private static void PickerSubtitleOmitsUnknownProgress()
    {
        var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            MultiplayerGameMode.Standard,
            "buddy1, buddy2 +2",
            [
                new PlayerIdentity("1", "buddy1"),
                new PlayerIdentity("2", "buddy2"),
                new PlayerIdentity("3", "buddy3"),
                new PlayerIdentity("4", "buddy4")
            ],
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            DateTimeOffset.Parse("2026-05-08T01:00:00Z"),
            null,
            "checksum",
            null));

        AssertEx.Equal("buddy1, buddy2 +2", row.Title);
        AssertEx.Equal("4 players", row.Subtitle);
    }

    private static void PickerCampaignRowIncludesFullDetails()
    {
        var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            MultiplayerGameMode.Standard,
            "buddy1, buddy2 +2",
            [
                new PlayerIdentity("1", "buddy1"),
                new PlayerIdentity("2", "buddy2"),
                new PlayerIdentity("3", "buddy3"),
                new PlayerIdentity("4", "buddy4")
            ],
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            DateTimeOffset.Parse("2026-05-08T01:30:00Z"),
            null,
            "checksum",
            "Floor 18"));

        var details = row.Details ?? throw new InvalidOperationException("Expected campaign details");

        AssertEx.Equal("buddy1, buddy2 +2", details.Title);
        AssertEx.Equal("Floor 18 - 4 players", details.Subtitle);
        AssertEx.Equal(2, details.SummaryLines.Count);
        AssertEx.Equal("Last played: 2026-05-08 01:30 UTC", details.SummaryLines[0]);
        AssertEx.Equal("Save id: aaaaaaaa", details.SummaryLines[1]);
        AssertEx.Equal(4, details.RosterLines.Count);
        AssertEx.Equal("1. buddy1", details.RosterLines[0]);
        AssertEx.Equal("2. buddy2", details.RosterLines[1]);
        AssertEx.Equal("3. buddy3", details.RosterLines[2]);
        AssertEx.Equal("4. buddy4", details.RosterLines[3]);
    }

    private static void PickerCampaignRowUsesCustomRunName()
    {
        var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            MultiplayerGameMode.Standard,
            "phatstatss, Magical Crocs",
            [new PlayerIdentity("steam:1", "phatstatss"), new PlayerIdentity("steam:2", "Magical Crocs")],
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            DateTimeOffset.Parse("2026-05-08T01:00:00Z"),
            null,
            "payload",
            "Floor 5",
            "Friday Poison Run"));

        AssertEx.Equal("Friday Poison Run", row.Title);
        AssertEx.Equal("Floor 5 - 2 players", row.Subtitle);
        AssertEx.Equal("Friday Poison Run", row.Details!.Title);
        AssertEx.Equal(null, row.Details.AutoLabel);
    }

    private static void PickerCampaignRowFallsBackWhenCustomRunNameCleared()
    {
        var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            MultiplayerGameMode.Standard,
            "phatstatss, Magical Crocs",
            [new PlayerIdentity("steam:1", "phatstatss"), new PlayerIdentity("steam:2", "Magical Crocs")],
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            DateTimeOffset.Parse("2026-05-08T01:00:00Z"),
            null,
            "payload",
            "Floor 5",
            null));

        AssertEx.Equal("phatstatss, Magical Crocs", row.Title);
        AssertEx.Equal(null, row.Details!.AutoLabel);
    }

    private static void PickerDetailsShowSelectedCharacters()
    {
        var constructor = typeof(PlayerIdentity).GetConstructor([typeof(string), typeof(string), typeof(string)]);
        AssertEx.True(constructor is not null, "PlayerIdentity should capture a selected character id");
        var player = (PlayerIdentity)constructor!.Invoke(["steam:1", "phatstatss", "CHARACTER.IRONCLAD"]);

        var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
            "cccccccccccccccccccccccccccccccc",
            MultiplayerGameMode.Standard,
            "phatstatss",
            [player],
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            DateTimeOffset.Parse("2026-05-08T01:30:00Z"),
            null,
            "1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef",
            "Floor 18"));

        var details = row.Details ?? throw new InvalidOperationException("Expected campaign details");

        AssertEx.Equal("1. phatstatss - The Ironclad", details.RosterLines[0]);
    }

    private static void PickerDetailsHandleMissingProgressAndRoster()
    {
        var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
            "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb",
            MultiplayerGameMode.Standard,
            "Unknown party",
            [],
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            null,
            "checksum",
            null));

        var details = row.Details ?? throw new InvalidOperationException("Expected campaign details");

        AssertEx.Equal("0 players", row.Subtitle);
        AssertEx.Equal(2, details.SummaryLines.Count);
        AssertEx.Equal("Last played: 2026-05-08 00:00 UTC", details.SummaryLines[0]);
        AssertEx.Equal("Save id: bbbbbbbb", details.SummaryLines[1]);
        AssertEx.Equal(1, details.RosterLines.Count);
        AssertEx.Equal("Unknown party", details.RosterLines[0]);
    }

    private static void PickerStartNewRowHasNoDetails()
    {
        var row = MultiplayerSavePickerRow.StartNew();

        AssertEx.Equal(null, row.Details);
    }

    private static void CompatibilityCheckerAllowsMatchingStableIds()
    {
        var metadata = CampaignWithRoster([
            new PlayerIdentity("Steam:1", "Alice"),
            new PlayerIdentity("Steam:2", "Bob")
        ]);

        var warning = CampaignCompatibilityChecker.BuildWarning(metadata, [
            new PlayerIdentity("Steam:2", "Bob"),
            new PlayerIdentity("Steam:1", "Alice")
        ]);

        AssertEx.Equal(null, warning);
    }

    private static void CompatibilityCheckerWarnsForMissingExpectedPlayers()
    {
        var metadata = CampaignWithRoster([
            new PlayerIdentity("Steam:1", "Alice"),
            new PlayerIdentity("Steam:2", "Bob")
        ]);

        var warning = CampaignCompatibilityChecker.BuildWarning(metadata, [
            new PlayerIdentity("Steam:1", "Alice")
        ]) ?? throw new InvalidOperationException("Expected compatibility warning");

        AssertEx.True(warning.Message.Contains("Missing original players: Bob", StringComparison.Ordinal));
        AssertEx.False(warning.Message.Contains("Extra current players:", StringComparison.Ordinal));
    }

    private static void CompatibilityCheckerWarnsForExtraCurrentPlayers()
    {
        var metadata = CampaignWithRoster([
            new PlayerIdentity("Steam:1", "Alice")
        ]);

        var warning = CampaignCompatibilityChecker.BuildWarning(metadata, [
            new PlayerIdentity("Steam:1", "Alice"),
            new PlayerIdentity("Steam:3", "Casey")
        ]) ?? throw new InvalidOperationException("Expected compatibility warning");

        AssertEx.True(warning.Message.Contains("Extra current players: Casey", StringComparison.Ordinal));
        AssertEx.False(warning.Message.Contains("Missing original players:", StringComparison.Ordinal));
    }

    private static void CompatibilityCheckerSkipsEmptyExpectedRoster()
    {
        var metadata = CampaignWithRoster([]);

        var warning = CampaignCompatibilityChecker.BuildWarning(metadata, [
            new PlayerIdentity("Steam:1", "Alice")
        ]);

        AssertEx.Equal(null, warning);
    }

    private static void CompatibilityCheckerSkipsRosterWithoutStableIds()
    {
        var metadata = CampaignWithRoster([
            new PlayerIdentity(null, "Alice")
        ]);

        var warning = CampaignCompatibilityChecker.BuildWarning(metadata, [
            new PlayerIdentity("Steam:1", "Alice")
        ]);

        AssertEx.Equal(null, warning);
    }

    private static void CompatibilityCheckerWarningKeyIsStable()
    {
        var metadata = CampaignWithRoster([
            new PlayerIdentity("Steam:1", "Alice"),
            new PlayerIdentity("Steam:2", "Bob")
        ]);

        var first = CampaignCompatibilityChecker.BuildWarning(metadata, [
            new PlayerIdentity("Steam:1", "Alice"),
            new PlayerIdentity("Steam:3", "Casey")
        ]) ?? throw new InvalidOperationException("Expected first compatibility warning");
        var second = CampaignCompatibilityChecker.BuildWarning(metadata, [
            new PlayerIdentity("Steam:3", "Casey"),
            new PlayerIdentity("Steam:1", "Alice")
        ]) ?? throw new InvalidOperationException("Expected second compatibility warning");

        AssertEx.Equal(first.WarningKey, second.WarningKey);
    }

    private static void CompatibilityGuardWarnsOnceForIdenticalMismatch()
    {
        var session = new HostFlowSession();
        session.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);
        var warnings = new List<CampaignCompatibilityWarning>();
        var guard = new LoadLobbyCompatibilityGuard(
            session,
            _ => CampaignWithRoster([
                new PlayerIdentity("Steam:1", "Alice"),
                new PlayerIdentity("Steam:2", "Bob")
            ]),
            () => [new PlayerIdentity("Steam:1", "Alice")],
            warnings.Add);

        var first = guard.ShouldAllowRunToBegin();
        var second = guard.ShouldAllowRunToBegin();

        AssertEx.False(first);
        AssertEx.True(second);
        AssertEx.Equal(1, warnings.Count);
        AssertEx.True(warnings[0].Message.Contains("Missing original players: Bob", StringComparison.Ordinal));
    }

    private static void CompatibilityGuardAcknowledgesAfterWarningDisplay()
    {
        var session = new HostFlowSession();
        session.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        var failingGuard = new LoadLobbyCompatibilityGuard(
            session,
            _ => CampaignWithRoster([
                new PlayerIdentity("Steam:1", "Alice"),
                new PlayerIdentity("Steam:2", "Bob")
            ]),
            () => [new PlayerIdentity("Steam:1", "Alice")],
            _ => throw new InvalidOperationException("warning UI failed"));

        var allowedAfterFailure = failingGuard.ShouldAllowRunToBegin();

        var warnings = new List<CampaignCompatibilityWarning>();
        var workingGuard = new LoadLobbyCompatibilityGuard(
            session,
            _ => CampaignWithRoster([
                new PlayerIdentity("Steam:1", "Alice"),
                new PlayerIdentity("Steam:2", "Bob")
            ]),
            () => [new PlayerIdentity("Steam:1", "Alice")],
            warnings.Add);

        var allowedAfterDisplay = workingGuard.ShouldAllowRunToBegin();
        var allowedAfterAcknowledgement = workingGuard.ShouldAllowRunToBegin();

        AssertEx.True(allowedAfterFailure);
        AssertEx.False(allowedAfterDisplay);
        AssertEx.True(allowedAfterAcknowledgement);
        AssertEx.Equal(1, warnings.Count);
    }

    private static void CompatibilityGuardAllowsWithoutSelectedCampaign()
    {
        var warnings = new List<CampaignCompatibilityWarning>();
        var guard = new LoadLobbyCompatibilityGuard(
            new HostFlowSession(),
            _ => throw new InvalidOperationException("campaign lookup should not run"),
            () => throw new InvalidOperationException("current roster should not run"),
            warnings.Add);

        var allowed = guard.ShouldAllowRunToBegin();

        AssertEx.True(allowed);
        AssertEx.Equal(0, warnings.Count);
    }

    private static void ControllerStartsNewRunThroughContinuation()
    {
        var continuation = new FakeHostFlowContinuation();
        var session = new HostFlowSession();
        var controller = CreateController(new FakeHostFlowSaveBank(), continuation: continuation, session: session);

        var result = controller.SelectStartNewRun(MultiplayerGameMode.Daily);

        AssertEx.True(result.Success);
        AssertEx.Equal(MultiplayerGameMode.Daily, session.SelectedGameMode);
        AssertEx.True(session.IsPendingNewRun);
        AssertEx.Equal(1, continuation.StartNewRunCount);
        AssertEx.Equal(0, continuation.LoadExistingCount);
    }

    private static void ControllerActivatesExistingCampaign()
    {
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation();
        var session = new HostFlowSession();
        var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation, session);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.True(result.Success);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", activator.ActivatedCampaignId);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", session.SelectedCampaignId);
        AssertEx.Equal(0, continuation.StartNewRunCount);
        AssertEx.Equal(1, continuation.LoadExistingCount);
    }

    private static void ControllerRepairsMetadataAfterExistingCampaignActivation()
    {
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation();
        var repair = new FakeActivatedCampaignMetadataRepair();
        var controller = CreateController(
            new FakeHostFlowSaveBank(),
            activator,
            continuation,
            metadataRepair: repair);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.True(result.Success);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", repair.RepairedCampaignId);
        AssertEx.Equal(DateTimeOffset.Parse("2026-05-08T12:00:00Z"), repair.RepairedAtUtc);
        AssertEx.Equal(1, continuation.LoadExistingCount);
    }

    private static void ControllerContinuesExistingCampaignWhenMetadataRepairFails()
    {
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation();
        var repair = new FakeActivatedCampaignMetadataRepair { ThrowOnRepair = true };
        var controller = CreateController(
            new FakeHostFlowSaveBank(),
            activator,
            continuation,
            metadataRepair: repair);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.True(result.Success);
        AssertEx.Equal(1, repair.RepairCount);
        AssertEx.Equal(1, continuation.LoadExistingCount);
    }

    private static void ControllerStopsStartNewWhenPreflightFails()
    {
        var preflight = new FakeActiveSavePreflight { Failure = "Active save has unsynced changes" };
        var continuation = new FakeHostFlowContinuation();
        var session = new HostFlowSession();
        var controller = CreateController(
            new FakeHostFlowSaveBank(),
            continuation: continuation,
            session: session,
            preflight: preflight);

        var result = controller.SelectStartNewRun(MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("Active save has unsynced changes", result.ErrorMessage);
        AssertEx.Equal(0, continuation.StartNewRunCount);
        AssertEx.Equal(null, session.SelectedGameMode);
    }

    private static void ControllerStopsSessionSelectionWhenStartNewFails()
    {
        var continuation = new FakeHostFlowContinuation { StartNewRunFailure = "start failed" };
        var session = new HostFlowSession();
        var controller = CreateController(new FakeHostFlowSaveBank(), continuation: continuation, session: session);

        var result = controller.SelectStartNewRun(MultiplayerGameMode.Daily);

        AssertEx.False(result.Success);
        AssertEx.Equal("start failed", result.ErrorMessage);
        AssertEx.Equal(null, session.SelectedCampaignId);
        AssertEx.Equal(null, session.SelectedGameMode);
        AssertEx.False(session.IsPendingNewRun);
    }

    private static void ControllerStopsWhenActivationFails()
    {
        var activator = new FakeActiveSaveActivator { Failure = "Active save has unsynced changes" };
        var continuation = new FakeHostFlowContinuation();
        var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("Active save has unsynced changes", result.ErrorMessage);
        AssertEx.Equal(0, continuation.StartNewRunCount);
        AssertEx.Equal(0, continuation.LoadExistingCount);
    }

    private static void ControllerStopsExistingCampaignWhenLoadPreflightFails()
    {
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation { PrepareLoadExistingFailure = "menu stack unavailable" };
        var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("menu stack unavailable", result.ErrorMessage);
        AssertEx.Equal(null, activator.ActivatedCampaignId);
        AssertEx.Equal(0, continuation.LoadExistingCount);
    }

    private static void ControllerStopsExistingCampaignWhenActivePreflightFails()
    {
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation();
        var preflight = new FakeActiveSavePreflight { Failure = "Current multiplayer save is not managed" };
        var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation, preflight: preflight);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("Current multiplayer save is not managed", result.ErrorMessage);
        AssertEx.Equal(null, activator.ActivatedCampaignId);
        AssertEx.Equal(0, continuation.LoadExistingCount);
    }

    private static void ActiveSaveActivatorMapsExceptions()
    {
        var activator = new DelegateActiveSaveActivator((_, _) => throw new InvalidOperationException("bad active save"));

        var result = activator.Activate("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", DateTimeOffset.Parse("2026-05-08T12:00:00Z"));

        AssertEx.False(result.Success);
        AssertEx.Equal("bad active save", result.ErrorMessage);
    }

    private static void ControllerRestoresPreviousActiveWhenLoadFails()
    {
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation { LoadExistingFailure = "load failed" };
        var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("load failed", result.ErrorMessage);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", activator.ActivatedCampaignId);
        AssertEx.Equal(1, activator.RestoreCount);
    }

    private static void ControllerReportsRollbackFailureWhenLoadFails()
    {
        var activator = new FakeActiveSaveActivator { RestoreFailure = "restore failed" };
        var continuation = new FakeHostFlowContinuation { LoadExistingFailure = "load failed" };
        var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("load failed; rollback failed: restore failed", result.ErrorMessage);
        AssertEx.Equal(1, activator.RestoreCount);
    }

    private static void ControllerStopsSessionSelectionWhenLoadFails()
    {
        var continuation = new FakeHostFlowContinuation { LoadExistingFailure = "load failed" };
        var session = new HostFlowSession();
        var controller = CreateController(new FakeHostFlowSaveBank(), continuation: continuation, session: session);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("load failed", result.ErrorMessage);
        AssertEx.Equal(null, session.SelectedCampaignId);
        AssertEx.Equal(null, session.SelectedGameMode);
        AssertEx.False(session.IsPendingNewRun);
    }

    private static void SaveSyncNoOpsWithoutSelection()
    {
        var sync = new FakeActiveSaveSync();
        var session = new HostFlowSession();
        var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

        var result = controller.SyncAfterVanillaSave();

        AssertEx.True(result.Success);
        AssertEx.Equal(0, sync.SyncBackCount);
        AssertEx.Equal(0, sync.FinalizeCount);
    }

    private static void SaveSyncSyncsExistingSelection()
    {
        var sync = new FakeActiveSaveSync();
        var session = new HostFlowSession();
        session.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);
        var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

        var result = controller.SyncAfterVanillaSave();

        AssertEx.True(result.Success);
        AssertEx.Equal(1, sync.SyncBackCount);
        AssertEx.Equal(0, sync.FinalizeCount);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", session.SelectedCampaignId);
        AssertEx.False(session.IsPendingNewRun);
    }

    private static void SaveSyncFinalizesPendingNewRun()
    {
        var sync = new FakeActiveSaveSync { FinalizedCampaignId = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb" };
        var session = new HostFlowSession();
        session.SelectNewRun(MultiplayerGameMode.Custom);
        var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

        var result = controller.SyncAfterVanillaSave();

        AssertEx.True(result.Success);
        AssertEx.Equal(0, sync.SyncBackCount);
        AssertEx.Equal(1, sync.FinalizeCount);
        AssertEx.Equal(MultiplayerGameMode.Custom, sync.FinalizedGameMode);
        AssertEx.Equal("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", session.SelectedCampaignId);
        AssertEx.Equal(MultiplayerGameMode.Custom, session.SelectedGameMode);
        AssertEx.False(session.IsPendingNewRun);
    }

    private static void SaveSyncFinalizesPendingNewRunWithMetadata()
    {
        var sync = new FakeActiveSaveSync { FinalizedCampaignId = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb" };
        var session = new HostFlowSession();
        session.SelectNewRun(MultiplayerGameMode.Custom);
        session.CapturePendingNewRunMetadata(new CampaignMetadataSnapshot(
            [new PlayerIdentity("steam:1", "buddy1")],
            "Floor 18"));
        var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

        var result = controller.SyncAfterVanillaSave();

        AssertEx.True(result.Success);
        AssertEx.Equal(1, sync.FinalizeCount);
        AssertEx.Equal("buddy1", sync.FinalizedMetadata?.Roster[0].DisplayName);
        AssertEx.Equal("Floor 18", sync.FinalizedMetadata?.ActOrFloor);
    }

    private static void SaveSyncKeepsPendingNewRunWhenFinalizationFails()
    {
        var sync = new FakeActiveSaveSync { FinalizeFailure = "active save is missing" };
        var session = new HostFlowSession();
        session.SelectNewRun(MultiplayerGameMode.Daily);
        var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

        var result = controller.SyncAfterVanillaSave();

        AssertEx.False(result.Success);
        AssertEx.Equal("active save is missing", result.ErrorMessage);
        AssertEx.Equal(null, session.SelectedCampaignId);
        AssertEx.Equal(MultiplayerGameMode.Daily, session.SelectedGameMode);
        AssertEx.True(session.IsPendingNewRun);
    }

    private static void SaveSyncMapsExceptions()
    {
        var sync = new ThrowingActiveSaveSync();
        var session = new HostFlowSession();
        session.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);
        var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

        var result = controller.SyncAfterVanillaSave();

        AssertEx.False(result.Success);
        AssertEx.Equal("sync exploded", result.ErrorMessage);
    }

    private static void ControllerExposesRecoveryModel()
    {
        var recovery = new FakeActiveSaveRecovery();
        var controller = CreateController(new FakeHostFlowSaveBank(), recovery: recovery);

        var model = controller.BuildRecoveryModel(MultiplayerGameMode.Standard);

        AssertEx.Equal("Active multiplayer save needs attention", model.Title);
        AssertEx.Equal(1, model.Options.Count);
        AssertEx.Equal(ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign, model.Options[0].Kind);
        AssertEx.Equal(MultiplayerGameMode.Standard, recovery.LastBuildGameMode);
    }

    private static void ControllerDuplicatesUnmanagedActiveSaveWithoutStartingNewRun()
    {
        var recovery = new FakeActiveSaveRecovery();
        var continuation = new FakeHostFlowContinuation();
        var controller = CreateController(
            new FakeHostFlowSaveBank(),
            continuation: continuation,
            recovery: recovery);

        var result = controller.RecoverAndSelectStartNewRun(
            ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign,
            MultiplayerGameMode.Custom);

        AssertEx.True(result.Success);
        AssertEx.Equal(ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign, recovery.LastRecoveredAction);
        AssertEx.Equal(MultiplayerGameMode.Custom, recovery.LastRecoveredGameMode);
        AssertEx.Equal(0, continuation.StartNewRunCount);
    }

    private static void ControllerSyncsManagedActiveSaveThenStartsNewRun()
    {
        var recovery = new FakeActiveSaveRecovery();
        var continuation = new FakeHostFlowContinuation();
        var controller = CreateController(
            new FakeHostFlowSaveBank(),
            continuation: continuation,
            recovery: recovery);

        var result = controller.RecoverAndSelectStartNewRun(
            ActiveSaveRecoveryActionKind.SyncActiveToCampaign,
            MultiplayerGameMode.Custom);

        AssertEx.True(result.Success);
        AssertEx.Equal(ActiveSaveRecoveryActionKind.SyncActiveToCampaign, recovery.LastRecoveredAction);
        AssertEx.Equal(MultiplayerGameMode.Custom, recovery.LastRecoveredGameMode);
        AssertEx.Equal(1, continuation.StartNewRunCount);
    }

    private static void ControllerRecoversThenSelectsExistingCampaign()
    {
        var recovery = new FakeActiveSaveRecovery();
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation();
        var controller = CreateController(
            new FakeHostFlowSaveBank(),
            activator,
            continuation,
            recovery: recovery);

        var result = controller.RecoverAndSelectExistingCampaign(
            ActiveSaveRecoveryActionKind.SyncActiveToCampaign,
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            MultiplayerGameMode.Standard);

        AssertEx.True(result.Success);
        AssertEx.Equal(ActiveSaveRecoveryActionKind.SyncActiveToCampaign, recovery.LastRecoveredAction);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", activator.ActivatedCampaignId);
        AssertEx.Equal(1, continuation.LoadExistingCount);
    }

    private static void ControllerStopsWhenRecoveryFails()
    {
        var recovery = new FakeActiveSaveRecovery { Failure = "sync failed" };
        var continuation = new FakeHostFlowContinuation();
        var controller = CreateController(new FakeHostFlowSaveBank(), continuation: continuation, recovery: recovery);

        var result = controller.RecoverAndSelectStartNewRun(
            ActiveSaveRecoveryActionKind.SyncActiveToCampaign,
            MultiplayerGameMode.Daily);

        AssertEx.False(result.Success);
        AssertEx.Equal("sync failed", result.ErrorMessage);
        AssertEx.Equal(0, continuation.StartNewRunCount);
    }

    private static void RecoveryModelReportsUnavailableWhenNoOptionsExist()
    {
        var model = ActiveSaveRecoveryModel.None();

        AssertEx.False(model.HasOptions);
        AssertEx.Equal("No recovery action is available.", model.Message);
    }

    private static void ControllerArchivesSelectedCampaign()
    {
        var bank = new FakeHostFlowSaveBank();

        var result = CreateController(bank).ArchiveCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        AssertEx.True(result.Success);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", bank.ArchivedCampaignId);
        AssertEx.Equal(DateTimeOffset.Parse("2026-05-08T12:00:00Z"), bank.ArchivedAtUtc);
    }

    private static void ControllerReportsArchiveCampaignFailure()
    {
        var bank = new FakeHostFlowSaveBank { ArchiveFailure = "archive failed" };

        var result = CreateController(bank).ArchiveCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        AssertEx.False(result.Success);
        AssertEx.Equal("archive failed", result.ErrorMessage);
    }

    private static void ControllerRenamesSelectedCampaign()
    {
        var bank = new FakeHostFlowSaveBank();

        var result = CreateController(bank).RenameCampaign(
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            "  Friday Poison Run  ");

        AssertEx.True(result.Success);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", bank.RenamedCampaignId);
        AssertEx.Equal("  Friday Poison Run  ", bank.RenamedCustomName);
    }

    private static void ControllerReportsRenameCampaignFailure()
    {
        var bank = new FakeHostFlowSaveBank { RenameFailure = "rename failed" };

        var result = CreateController(bank).RenameCampaign(
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            "Friday Poison Run");

        AssertEx.False(result.Success);
        AssertEx.Equal("rename failed", result.ErrorMessage);
    }

    private static void ControllerRestoresArchivedCampaign()
    {
        var bank = new FakeHostFlowSaveBank();

        var result = CreateController(bank).RestoreArchivedCampaign("20260510123456-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        AssertEx.True(result.Success);
        AssertEx.Equal("20260510123456-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", bank.RestoredArchiveKey);
    }

    private static void ControllerReportsRestoreArchivedCampaignFailure()
    {
        var bank = new FakeHostFlowSaveBank { RestoreArchiveFailure = "restore failed" };

        var result = CreateController(bank).RestoreArchivedCampaign("20260510123456-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        AssertEx.False(result.Success);
        AssertEx.Equal("restore failed", result.ErrorMessage);
    }

    private static void ControllerPermanentlyDeletesActiveCampaign()
    {
        var bank = new FakeHostFlowSaveBank();

        var result = CreateController(bank).DeleteCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        AssertEx.True(result.Success);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", bank.DeletedCampaignId);
    }

    private static void ControllerReportsPermanentActiveDeleteFailure()
    {
        var bank = new FakeHostFlowSaveBank { DeleteCampaignFailure = "delete failed" };

        var result = CreateController(bank).DeleteCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        AssertEx.False(result.Success);
        AssertEx.Equal("delete failed", result.ErrorMessage);
    }

    private static void ControllerPermanentlyDeletesArchivedCampaign()
    {
        var bank = new FakeHostFlowSaveBank();

        var result = CreateController(bank).DeleteArchivedCampaign("20260510123456-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        AssertEx.True(result.Success);
        AssertEx.Equal("20260510123456-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", bank.DeletedArchiveKey);
    }

    private static void ControllerReportsPermanentArchivedDeleteFailure()
    {
        var bank = new FakeHostFlowSaveBank { DeleteArchivedFailure = "delete archive failed" };

        var result = CreateController(bank).DeleteArchivedCampaign("20260510123456-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        AssertEx.False(result.Success);
        AssertEx.Equal("delete archive failed", result.ErrorMessage);
    }


    private static void ControllerClearsDeletedCampaigns()
    {
        var bank = new FakeHostFlowSaveBank();

        var result = CreateController(bank).ClearDeletedCampaigns();

        AssertEx.True(result.Success);
        AssertEx.Equal(1, bank.ClearDeletedCount);
    }

    private static void ControllerReportsClearDeletedFailure()
    {
        var bank = new FakeHostFlowSaveBank { ClearDeletedFailure = "clear failed" };

        var result = CreateController(bank).ClearDeletedCampaigns();

        AssertEx.False(result.Success);
        AssertEx.Equal("clear failed", result.ErrorMessage);
    }

    private static HostFlowController CreateController(
        FakeHostFlowSaveBank? bank = null,
        FakeActiveSaveActivator? activator = null,
        FakeHostFlowContinuation? continuation = null,
        HostFlowSession? session = null,
        FakeActiveSavePreflight? preflight = null,
        FakeActiveSaveRecovery? recovery = null,
        FakeActivatedCampaignMetadataRepair? metadataRepair = null)
    {
        return new HostFlowController(
            bank ?? new FakeHostFlowSaveBank(),
            preflight ?? new FakeActiveSavePreflight(),
            activator ?? new FakeActiveSaveActivator(),
            continuation ?? new FakeHostFlowContinuation(),
            recovery ?? new FakeActiveSaveRecovery(),
            session ?? new HostFlowSession(),
            new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")),
            metadataRepair ?? new FakeActivatedCampaignMetadataRepair());
    }

    private static CampaignMetadata CampaignWithRoster(IReadOnlyList<PlayerIdentity> roster) =>
        new(
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            MultiplayerGameMode.Standard,
            CampaignLabeler.Build(roster),
            roster,
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            DateTimeOffset.Parse("2026-05-08T01:00:00Z"),
            null,
            "checksum",
            "Floor 7");

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }

    private sealed class FakeHostFlowSaveBank : IHostFlowSaveBank
    {
        public IReadOnlyList<CampaignMetadata> Campaigns { get; init; } = [];
        public IReadOnlyList<ArchivedCampaign> ArchivedCampaigns { get; init; } = [];
        public bool HasDeleted { get; init; }
        public string? ArchivedCampaignId { get; private set; }
        public DateTimeOffset? ArchivedAtUtc { get; private set; }
        public string? ArchiveFailure { get; init; }
        public string? RestoredArchiveKey { get; private set; }
        public string? RestoreArchiveFailure { get; init; }
        public string? RenamedCampaignId { get; private set; }
        public string? RenamedCustomName { get; private set; }
        public string? RenameFailure { get; init; }
        public string? DeletedCampaignId { get; private set; }
        public string? DeleteCampaignFailure { get; init; }
        public string? DeletedArchiveKey { get; private set; }
        public string? DeleteArchivedFailure { get; init; }
        public int ClearDeletedCount { get; private set; }
        public string? ClearDeletedFailure { get; init; }

        public IReadOnlyList<CampaignMetadata> ListCampaigns(MultiplayerGameMode gameMode) =>
            Campaigns.Where(campaign => campaign.GameMode == gameMode).ToList();

        public IReadOnlyList<ArchivedCampaign> ListArchivedCampaigns(MultiplayerGameMode gameMode) =>
            ArchivedCampaigns.Where(archived => archived.Metadata.GameMode == gameMode).ToList();

        public bool HasDeletedCampaigns() => HasDeleted;

        public void ArchiveCampaign(string campaignId, DateTimeOffset deletedAtUtc)
        {
            if (ArchiveFailure is not null)
                throw new InvalidOperationException(ArchiveFailure);

            ArchivedCampaignId = campaignId;
            ArchivedAtUtc = deletedAtUtc;
        }

        public CampaignMetadata RestoreArchivedCampaign(string archiveKey)
        {
            if (RestoreArchiveFailure is not null)
                throw new InvalidOperationException(RestoreArchiveFailure);

            RestoredArchiveKey = archiveKey;
            return ArchivedCampaigns.FirstOrDefault(archived => archived.ArchiveKey == archiveKey)?.Metadata
                ?? CampaignWithRoster([]);
        }

        public CampaignMetadata RenameCampaign(string campaignId, string? customName)
        {
            if (RenameFailure is not null)
                throw new InvalidOperationException(RenameFailure);

            RenamedCampaignId = campaignId;
            RenamedCustomName = customName;
            return Campaigns.FirstOrDefault(campaign => campaign.CampaignId == campaignId)
                ?? CampaignWithRoster([]);
        }

        public void DeleteCampaign(string campaignId)
        {
            if (DeleteCampaignFailure is not null)
                throw new InvalidOperationException(DeleteCampaignFailure);

            DeletedCampaignId = campaignId;
        }

        public void DeleteArchivedCampaign(string archiveKey)
        {
            if (DeleteArchivedFailure is not null)
                throw new InvalidOperationException(DeleteArchivedFailure);

            DeletedArchiveKey = archiveKey;
        }

        public void ClearDeletedCampaigns()
        {
            if (ClearDeletedFailure is not null)
                throw new InvalidOperationException(ClearDeletedFailure);

            ClearDeletedCount++;
        }
    }

    private sealed class FakeActiveSaveActivator : IActiveSaveActivator
    {
        public string? ActivatedCampaignId { get; private set; }
        public string? Failure { get; init; }
        public string? RestoreFailure { get; init; }
        public int RestoreCount { get; private set; }

        public OperationResult Activate(string campaignId, DateTimeOffset nowUtc)
        {
            if (Failure is not null)
                return OperationResult.Fail(Failure);

            ActivatedCampaignId = campaignId;
            return OperationResult.Ok();
        }

        public OperationResult RestorePreviousActive(DateTimeOffset nowUtc)
        {
            RestoreCount++;
            if (RestoreFailure is not null)
                return OperationResult.Fail(RestoreFailure);

            return OperationResult.Ok();
        }
    }

    private sealed class FakeHostFlowContinuation : IHostFlowContinuation
    {
        public int StartNewRunCount { get; private set; }
        public int PrepareLoadExistingCount { get; private set; }
        public int LoadExistingCount { get; private set; }
        public string? StartNewRunFailure { get; init; }
        public string? PrepareLoadExistingFailure { get; init; }
        public string? LoadExistingFailure { get; init; }

        public OperationResult StartNewRun(MultiplayerGameMode gameMode)
        {
            StartNewRunCount++;
            if (StartNewRunFailure is not null)
                return OperationResult.Fail(StartNewRunFailure);

            return OperationResult.Ok();
        }

        public OperationResult PrepareLoadExistingRun()
        {
            PrepareLoadExistingCount++;
            if (PrepareLoadExistingFailure is not null)
                return OperationResult.Fail(PrepareLoadExistingFailure);

            return OperationResult.Ok();
        }

        public OperationResult LoadExistingRun()
        {
            LoadExistingCount++;
            if (LoadExistingFailure is not null)
                return OperationResult.Fail(LoadExistingFailure);

            return OperationResult.Ok();
        }
    }

    private sealed class FakeActiveSavePreflight : IActiveSavePreflight
    {
        public string? Failure { get; init; }
        public int FailuresBeforeSuccess { get; init; } = int.MaxValue;
        private int _calls;

        public OperationResult EnsureActiveSaveCanBeReplaced()
        {
            _calls++;
            return Failure is not null && _calls <= FailuresBeforeSuccess
                ? OperationResult.Fail(Failure)
                : OperationResult.Ok();
        }
    }

    private sealed class FakeActivatedCampaignMetadataRepair : IActivatedCampaignMetadataRepair
    {
        public string? RepairedCampaignId { get; private set; }
        public DateTimeOffset? RepairedAtUtc { get; private set; }
        public bool ThrowOnRepair { get; init; }
        public int RepairCount { get; private set; }

        public void RepairActivatedCampaign(string campaignId, DateTimeOffset nowUtc)
        {
            RepairCount++;
            RepairedCampaignId = campaignId;
            RepairedAtUtc = nowUtc;
            if (ThrowOnRepair)
                throw new InvalidOperationException("repair failed");
        }
    }

    private sealed class FakeActiveSaveRecovery : IActiveSaveRecovery
    {
        public string? Failure { get; init; }
        public MultiplayerGameMode? LastBuildGameMode { get; private set; }
        public ActiveSaveRecoveryActionKind? LastRecoveredAction { get; private set; }
        public MultiplayerGameMode? LastRecoveredGameMode { get; private set; }

        public ActiveSaveRecoveryModel BuildRecoveryModel(MultiplayerGameMode gameMode)
        {
            LastBuildGameMode = gameMode;
            return new ActiveSaveRecoveryModel(
                "Active multiplayer save needs attention",
                "Choose how to protect the current active multiplayer save before continuing.",
                [
                    new ActiveSaveRecoveryOption(
                        ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign,
                        "Duplicate Active Save",
                        "Copy the current active save into the Multiplayer Save Slots bank.")
                ]);
        }

        public OperationResult Recover(ActiveSaveRecoveryActionKind action, MultiplayerGameMode gameMode, DateTimeOffset nowUtc)
        {
            if (Failure is not null)
                return OperationResult.Fail(Failure);

            LastRecoveredAction = action;
            LastRecoveredGameMode = gameMode;
            return OperationResult.Ok();
        }
    }

    private sealed class FakeActiveSaveSync : IActiveSaveSync
    {
        public int SyncBackCount { get; private set; }
        public int FinalizeCount { get; private set; }
        public string FinalizedCampaignId { get; init; } = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
        public string? FinalizeFailure { get; init; }
        public MultiplayerGameMode? FinalizedGameMode { get; private set; }
        public CampaignMetadataSnapshot? FinalizedMetadata { get; private set; }

        public OperationResult SyncBack(DateTimeOffset nowUtc)
        {
            SyncBackCount++;
            return OperationResult.Ok();
        }

        public OperationResult<string> FinalizePendingNewRun(
            MultiplayerGameMode gameMode,
            CampaignMetadataSnapshot metadata,
            DateTimeOffset nowUtc)
        {
            FinalizeCount++;
            FinalizedGameMode = gameMode;
            FinalizedMetadata = metadata;
            return FinalizeFailure is null
                ? OperationResult<string>.Ok(FinalizedCampaignId)
                : OperationResult<string>.Fail(FinalizeFailure);
        }
    }

    private sealed class ThrowingActiveSaveSync : IActiveSaveSync
    {
        public OperationResult SyncBack(DateTimeOffset nowUtc) => throw new InvalidOperationException("sync exploded");

        public OperationResult<string> FinalizePendingNewRun(
            MultiplayerGameMode gameMode,
            CampaignMetadataSnapshot metadata,
            DateTimeOffset nowUtc) =>
            throw new InvalidOperationException("finalize exploded");
    }
}
