using System.ComponentModel;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using Action = Lumina.Excel.Sheets.Action;

namespace KirboPublicRotations.Helpers;

[Api(6)]
internal unsafe class CustomRotationEx : MachinistRotation
{
    #region Limit break
    [System.ComponentModel.Description("Bar Count")]
    internal unsafe static byte CurrentBarCount
    {
        get
        {
            FFXIVClientStructs.FFXIV.Client.Game.UI.LimitBreakController limitBreakController = FFXIVClientStructs.FFXIV.Client.Game.UI.UIState.Instance()->LimitBreakController;
            byte barCount = *&limitBreakController.BarCount;

            return barCount;
        }
    }

    [System.ComponentModel.Description("Limit Break Level")]
    internal unsafe static byte CurrentLimitBreakLevel
    {
        get
        {
            FFXIVClientStructs.FFXIV.Client.Game.UI.LimitBreakController limitBreakController = FFXIVClientStructs.FFXIV.Client.Game.UI.UIState.Instance()->LimitBreakController;
            ushort currentUnits = *&limitBreakController.CurrentUnits;

            if (currentUnits >= 9000)
            {
                return 3;
            }
            else if (currentUnits >= 6000)
            {
                return 2;
            }
            else if (currentUnits >= 3000)
            {
                return 1;
            }
            else
            {
                return 0; // Assuming 0 is the default or undefined state.
            }
        }
    }

    [System.ComponentModel.Description("Current Units")]
    internal unsafe static ushort CurrentCurrentUnits
    {
        get
        {
            FFXIVClientStructs.FFXIV.Client.Game.UI.LimitBreakController limitBreakController = FFXIVClientStructs.FFXIV.Client.Game.UI.UIState.Instance()->LimitBreakController;
            ushort currentUnits = *&limitBreakController.CurrentUnits;

            return currentUnits;
        }
    }

    [System.ComponentModel.Description("Is PvP")]
    internal unsafe static bool IsCurrentPvP
    {
        get
        {
            FFXIVClientStructs.FFXIV.Client.Game.UI.LimitBreakController limitBreakController = FFXIVClientStructs.FFXIV.Client.Game.UI.UIState.Instance()->LimitBreakController;
            bool isPvP = *&limitBreakController.IsPvP;

            return isPvP;
        }
    }
    #endregion

    /// <summary>
    /// Property
    /// Dynamically evaluates if the current target is a BattleNpc.
    /// </summary>
    internal static bool IsTargetNPC => CurrentTarget?.ObjectKind == Dalamud.Game.ClientState.Objects.Enums.ObjectKind.BattleNpc;

    #region PvP NPC/PC Names
    // Centralized collection of NPC names
    private static readonly HashSet<string> _npcNames = new HashSet<string>
    {
        "Raven Magus",
        "Raven Viking",
        "Icebound Tomelith A1",
        "Icebound Tomelith A2",
        "Icebound Tomelith A3",
        "Icebound Tomelith A4",
        "Icebound Tomelith B1",
        "Icebound Tomelith B2",
        "Icebound Tomelith B3",
        "Icebound Tomelith B4",
        "Icebound Tomelith B5",
        "Icebound Tomelith B6",
        "Icebound Tomelith B7",
        "Icebound Tomelith B8",
        "Icebound Tomelith B9",
        "Icebound Tomelith B10",
        "Icebound Tomelith B11",
        "Icebound Tomelith B12",
        "Icebound Tomelith B13",
        "Icebound Tomelith B14",
        "Icebound Tomelith B15",
        //"Striking Dummy",
        // Add more NPC names here
    };

    /// <summary>
    /// Checks if the given name belongs to an NPC.
    /// </summary>
    /// <param name="name">The name to check.</param>
    /// <returns>True if the name is in the NPC collection, otherwise false.</returns>
    internal static bool IsPvPNpc(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && _npcNames.Contains(name);
    }
    #endregion

    #region PvP limit breaks
    /// <summary>
    /// PLD
    /// </summary>
    internal static IBaseAction PhalanxPvP { get; } = new BaseAction((ActionID)29069);

    /// <summary>
    /// WAR
    /// </summary>
    internal static IBaseAction PrimalScreamPvP { get; } = new BaseAction((ActionID)29083);

    /// <summary>
    /// DRK
    /// </summary>
    internal static IBaseAction EvenTidePvP { get; } = new BaseAction((ActionID)29097);

    /// <summary>
    /// GNB
    /// </summary>
    internal static IBaseAction TerminalTriggerPvP { get; } = new BaseAction((ActionID)29469);

    /// <summary>
    /// WHM
    /// </summary>
    internal static IBaseAction AfflatusPurgationPvP { get; } = new BaseAction((ActionID)29230);

    /// <summary>
    /// AST
    /// </summary>
    internal static IBaseAction CelestialRiverPvP { get; } = new BaseAction((ActionID)29255);

    /// <summary>
    /// SCH
    /// </summary>
    internal static IBaseAction SummonSeraphPvP { get; } = new BaseAction((ActionID)29237);

    /// <summary>
    /// SCH
    /// </summary>
    internal static IBaseAction SeraphFlightPvP { get; } = new BaseAction((ActionID)29239);

    /// <summary>
    /// SGE
    /// </summary>
    internal static IBaseAction MesotesPvP { get; } = new BaseAction((ActionID)29266);

    /// <summary>
    /// NIN
    /// </summary>
    internal static IBaseAction SeitonTenchuPvP { get; } = new BaseAction((ActionID)29515);

    /// <summary>
    /// MNK
    /// </summary>
    internal static IBaseAction MeteoDivePvP { get; } = new BaseAction((ActionID)29485);

    /// <summary>
    /// DRG
    /// </summary>
    internal static IBaseAction SkyHighPvP { get; } = new BaseAction((ActionID)29497);

    /// <summary>
    /// DRG
    /// </summary>
    internal static IBaseAction SkyShatterPvP { get; } = new BaseAction((ActionID)29499);

    /// <summary>
    /// SAM
    /// </summary>
    internal static IBaseAction ZantetsukenPvP { get; } = new BaseAction((ActionID)29537);

    /// <summary>
    /// RPR
    /// </summary>
    internal static IBaseAction TenebraelemuruPvP { get; } = new BaseAction((ActionID)29553);

    /// <summary>
    /// BRD
    /// </summary>
    internal static IBaseAction FinalFantasiaPvP { get; } = new BaseAction((ActionID)29401);

    /// <summary>
    /// MCH
    /// </summary>
    internal static IBaseAction MarksmansSpitePvP { get; } = new BaseAction((ActionID)29415);

    /// <summary>
    /// DNC
    /// </summary>
    internal static IBaseAction ContraDancePvP { get; } = new BaseAction((ActionID)29432);

    /// <summary>
    /// BLM
    /// </summary>
    internal static IBaseAction SoulResonancePvP { get; } = new BaseAction((ActionID)29662);


    /// <summary>
    /// SMN
    /// </summary>
    internal static IBaseAction SummonBahamutPvP { get; } = new BaseAction((ActionID)29673);

    /// <summary>
    /// SMN
    /// </summary>
    internal static IBaseAction SummonPhoenixPvP { get; } = new BaseAction((ActionID)29678);

    /// <summary>
    /// SMN
    /// </summary>
    internal static IBaseAction MegaflarePvP { get; } = new BaseAction((ActionID)29675);

    /// <summary>
    /// SMN
    /// </summary>
    internal static IBaseAction EverlastingPvP { get; } = new BaseAction((ActionID)29680);

    /// <summary>
    /// RDM
    /// </summary>
    internal static IBaseAction SouthernCrossPvP { get; } = new BaseAction((ActionID)29704);
    #endregion

    #region Logging
    private const string KirboLogMessage = "[Kirbo Public Rotations]";

    /// <summary>
    /// Sends a debug level message to the Dalamud log console.
    /// </summary>
    /// <param name="message"></param>
    internal static void Debug(string message) => Serilog.Log.Debug("{KirboLogMessage} {Message}", KirboLogMessage, message);

    /// <summary>
    /// Sends a warning level message to the Dalamud log console.
    /// </summary>
    /// <param name="message"></param>
    internal static void Warning(string message) => Serilog.Log.Warning("{KirboLogMessage} {Message}", KirboLogMessage, message);
    #endregion

}