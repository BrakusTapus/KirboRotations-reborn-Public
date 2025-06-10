using FFXIVClientStructs.FFXIV.Client.Game.Object;
using KirboPublicRotations.Helpers;

namespace KirboPublicRotations.PvPRotations.Ranged;

[BetaRotation]
[Rotation("MCH Kirbo PvP\n  [Public]", CombatType.PvP, GameVersion = "7.25", Description = "Kirbo's public PvP Rotation for MCH\nUses LB\nUses Turret")]
[Api(4)]
internal class MchKirboPvpPublic : MachinistRotation
{
    /// <summary>
    ///     Gets the current Heat Stacks.
    /// </summary>
    private static byte PvP_OverheatedStacks
    {
        get
        {
            byte pvP_OverheatedStacks = CustomRotation.Player.StatusStack(true, StatusID.Heat);
            if (pvP_OverheatedStacks != byte.MaxValue)
            {
                return pvP_OverheatedStacks;
            }

            return 4;
        }
    }
    private static bool IsPvPOverheated => Player.HasStatus(true, StatusID.Overheated_3149);
    private static float OverheatedStatusTime => Player.StatusTime(true, StatusID.Overheated_3149);
    private static bool PlayerHasWildfire => Player.HasStatus(true, StatusID.Wildfire_2018);
    private static float PlayerWildfireStatusTime => Player.StatusTime(true, StatusID.Wildfire_2018);
    private static bool PvPTargetHasWildfire => CurrentTarget != null && CurrentTarget.HasStatus(true, StatusID.Wildfire_1323);
#pragma warning disable CS8604 // Possible null reference argument.
    private static float PvPTargetWildfireStatusTime => CurrentTarget.StatusTime(true, StatusID.Wildfire_1323);
#pragma warning restore CS8604 // Possible null reference argument.
    private static float AnalysisStatusTime => Player.StatusTime(true, StatusID.Analysis);
    //MCH LB
    private static IBaseAction MarksmansSpitePvP { get; } = new BaseAction((ActionID)29415);

    private IBaseAction MarksmansSpitePvP2 => _MarksmansSpitePvPCreator.Value;
    private readonly Lazy<IBaseAction> _MarksmansSpitePvPCreator = new Lazy<IBaseAction>(delegate
    {
        IBaseAction action40 = new BaseAction((ActionID)29415);
        ActionSetting setting40 = action40.Setting;
        setting40.RotationCheck = () =>
        CustomRotationEx.CurrentLimitBreakLevel == 1 &&
        action40.Target.Target.CurrentHp <= 30000 &&
            (
                action40.Target.Target.IsJobCategory(JobRole.RangedMagical) ||
                action40.Target.Target.IsJobCategory(JobRole.RangedPhysical) ||
                action40.Target.Target.IsJobCategory(JobRole.Healer)
            );
        setting40.TargetType = TargetType.LowHP;
        action40.Setting = setting40;
        return action40;
    });

    #region Rotation Config
    [RotationConfig(CombatType.PvP, Name = "GuardCancel")]
    public bool GuardCancel { get; set; } = true;

    [RotationConfig(CombatType.PvP, Name = "Emergency Healing")]
    public bool EmergencyHealing { get; set; } = false;

    [RotationConfig(CombatType.PvP, Name = "LowHPNoBlastCharge")]
    public bool LowHPNoBlastCharge { get; set; } = true;

    [RotationConfig(CombatType.PvP, Name = "LowHPNoBlastChargeThreshold")]
    public int LowHPNoBlastChargeThreshold { get; set; } = 22500;

    [RotationConfig(CombatType.PvP, Name = "AnalysisOnDrill")]
    public bool AnalysisOnDrill { get; set; } = true;

    [RotationConfig(CombatType.PvP, Name = "AnalysisOnAirAnchor")]
    public bool AnalysisOnAirAnchor { get; set; } = false;

    [RotationConfig(CombatType.PvP, Name = "AnalysisOnBioBlaster")]
    public bool AnalysisOnBioBlaster { get; set; } = false;

    [RotationConfig(CombatType.PvP, Name = "AnalysisOnChainsaw")]
    public bool AnalysisOnChainsaw { get; set; } = true;

    [RotationConfig(CombatType.PvP, Name = "ManualBishop")]
    public bool ManualBishop { get; set; } = false;

    [RotationConfig(CombatType.PvP, Name = "Use Purify")]
    public bool UsePurifyPvP { get; set; } = true;

    private bool DoPurify(out IAction? action)
    {
        action = null;
        if (!UsePurifyPvP) return false;

        var purifiableStatusesIDs = new List<int>
        {
            // Stun, DeepFreeze, HalfAsleep, Sleep, Bind, Heavy, Silence
            1343, 3219, 3022, 1348, 1345, 1344, 1347
        };

        if (purifiableStatusesIDs.Any(id => Player.HasStatus(false, (StatusID)id)))
        {
            return PurifyPvP.CanUse(out action);
        }

        return false;
    }

    [RotationConfig(CombatType.PvP, Name = "Enable experimental features.")]
    private bool ExperimentalFeature { get; set; } = false;

    [RotationConfig(CombatType.PvP, Name = "Enable experimental LB features.")]
    private bool ExperimentalLBFeature { get; set; } = false;
    #endregion Rotation Config

    #region Status Display
    public override void DisplayStatus()
    {
        ImGui.TextWrapped("Player HPP: " + Player.GetHealthRatio());
        ImGui.TextWrapped("LimitBreakLevel: " + CustomRotationEx.CurrentLimitBreakLevel);
        ImGuiToolTipsKirbo.HoveredTooltip("CurrentUnits: " + CustomRotationEx.CurrentCurrentUnits);
        ImGui.NewLine();

        ImGui.TextWrapped("HeatStacks: " + PvP_OverheatedStacks);
        ImGui.TextWrapped("Status Time Analysis: " + AnalysisStatusTime.ToString("F2") + "s");
        ImGui.NewLine();

        ImGui.TextWrapped("IsPvPOverheated (Player): " + IsPvPOverheated);
        ImGui.TextWrapped("Overheated StatusTime: " + OverheatedStatusTime.ToString("F2") + "s");
        ImGui.NewLine();

        ImGui.TextWrapped("PlayerHasWildfire: " + PlayerHasWildfire);
        ImGui.TextWrapped("PlayerWildfireStatusTime: " + PlayerWildfireStatusTime.ToString("F2") + "s");
        ImGui.NewLine();

        ImGui.TextWrapped("PvPTargetHasWildfire: " + PvPTargetHasWildfire);
        ImGui.TextWrapped("PvPTargetWildfireStatusTime: " + PvPTargetWildfireStatusTime.ToString("F2") + "s");
        ImGui.NewLine();

        ImGui.TextWrapped("BlastChargePvP Target: " + BlastChargePvP.Target.Target?.ToString());
        ImGui.TextWrapped("BishopAutoturretPvP Target: " + BishopAutoturretPvP.Target.Target?.ToString());
        ImGui.TextWrapped("BioblasterPvP Target: " + BioblasterPvP.Target.Target?.ToString());
        ImGui.NewLine();

        if (CurrentTarget != null)
        {
            ImGui.TextWrapped("Target HP ratio: " + CurrentTarget.GetHealthRatio());
            ImGui.TextWrapped("Distance: " + CurrentTarget.DistanceToPlayer().ToString("F1") + "y");
        }
        else
        {
            ImGui.TextWrapped("Target HP ratio: 0");
            ImGui.TextWrapped("Distance: " + "no target");
        }
    }
    #endregion

    #region oGCD Logic
    protected override bool EmergencyAbility(IAction nextGCD, out IAction? act)
    {
        act = null;
        // Should prevent any actions if the option 'guardCancel' is enabled and Player has the Guard buff up
        if (GuardCancel && Player.HasStatus(true, StatusID.Guard))
        {
            return false;
        }

        if (DoPurify(out act)) return true;

        if (EmergencyHealing && EmergencyLowHP(out act))
        {
            return true;
        }

        if (BraveryPvP.CanUse(out act) && NumberOfAllHostilesInRange > 0)
        {
            return true;
        }

        // Bishop Turret should be used off cooldown
        // Note: Could prolly be improved using 'ChoiceTarget' in the IBaseAction
        if (BishopAutoturretPvP.Target.AffectedTargets.Length >= 2 && BishopAutoturretPvP.CanUse(out act, skipAoeCheck: true)) // Without MustUse, returns CastType 7 invalid // BishopAutoturretPvP.action.CastType
        {
            return true;
        }

        // PvE_Wildfire Should be used only right after getting the 5th Heat Stacks
        if ((IsLastGCD((ActionID)41469) || IsPvPOverheated) &&
            !Player.WillStatusEnd(2f, true, StatusID.Overheated_3149) &&
            CurrentTarget != null &&
            CurrentTarget.GetHealthRatio() >= 0.5 &&
            !CustomRotationEx.IsPvPNpc(CurrentTarget.Name.ToString())
            && WildfirePvP.CanUse(out act))
        {
            return true;
        }



        // Analysis should be used on any of the tools depending on which options are enabled
        if (AnalysisPvP.CanUse(out act, usedUp: true) && NumberOfAllHostilesInRange > 0 && /*!IsPvPOverheated &&*/ !Player.HasStatus(true, StatusID.Analysis) && !IsLastAction(ActionID.AnalysisPvP))
        {
            if (AnalysisOnDrill && nextGCD.IsTheSameTo(true, DrillPvP) && Player.HasStatus(true, StatusID.DrillPrimed))
            {
                return true;
            }
            if (AnalysisOnChainsaw && nextGCD.IsTheSameTo(true, ChainSawPvP) && Player.HasStatus(true, StatusID.ChainSawPrimed))
            {
                return true;
            }
            if (AnalysisOnBioBlaster && nextGCD.IsTheSameTo(true, BioblasterPvP) && Player.HasStatus(true, StatusID.BioblasterPrimed))
            {
                return true;
            }
            if (AnalysisOnAirAnchor && nextGCD.IsTheSameTo(true, AirAnchorPvP) && Player.HasStatus(true, StatusID.AirAnchorPrimed))
            {
                return true;
            }
        }

        return base.EmergencyAbility(nextGCD, out act);
    }

    protected override bool DefenseSingleAbility(IAction nextGCD, out IAction? action)
    {
        action = null;
        if (GuardCancel && Player.HasStatus(true, StatusID.Guard)) return false;

        return base.DefenseSingleAbility(nextGCD, out action);
    }
    #endregion oGCD Logic

    #region GCD Logic
    protected override bool GeneralGCD(out IAction? act)
    {
        act = null;
        // Should prevent any actions if the option 'guardCancel' is enabled and Player has the Guard buff up
        if (GuardCancel && Player.HasStatus(true, StatusID.Guard))
        {
            return false;
        }

        if (EmergencyHealing && EmergencyLowHP(out act))
        {
            return true;
        }

        // Uses BioBlaster automatically when a Target is in range
        if (!IsPvPOverheated && BioblasterPvP.CanUse(out act, usedUp: true, skipAoeCheck: true) && CurrentTarget != null && Player.HasStatus(true, StatusID.BioblasterPrimed) && CurrentTarget.DistanceToPlayer() <= 12)
        {
            return true;
        }

        if (IsPvPOverheated && BlazingShotPvP.CanUse(out act, skipCastingCheck: true))
        {
            return true;
        }

        if (ExperimentalFeature)
        {
            if (ExperimentalLBFeature)
            {
                if (UseMCHLB(out act))
                {
                    return true;
                }
            }
            else
            {
                if (MarksmansSpitePvP2.CanUse(out act) && CustomRotationEx.CurrentLimitBreakLevel == 1)
                {
                    return true;
                }
            }
        }
        else
        {

            var battleNPC = Dalamud.Game.ClientState.Objects.Enums.ObjectKind.BattleNpc;
            if (MarksmansSpitePvP2.CanUse(out act) &&
                CustomRotationEx.CurrentLimitBreakLevel == 1 &&
                CurrentTarget != null &&
                CurrentTarget.ObjectKind != battleNPC &&
                !CustomRotationEx.IsPvPNpc(CurrentTarget.Name.ToString()))
            {
                //TODO: instead of checking isjobcategory should use enemy names like "Summoner" or "Paladin" as it will be more acurate
                if (CurrentTarget.IsJobCategory(JobRole.RangedMagical) && CurrentTarget.CurrentHp >= 10000 && CurrentTarget.CurrentHp <= 32400) return true;
                else if (CurrentTarget.IsJobCategory(JobRole.Healer) && CurrentTarget.CurrentHp >= 10000 && CurrentTarget.CurrentHp <= 30000) return true;
                else if (CurrentTarget.IsJobCategory(JobRole.RangedPhysical) && CurrentTarget.CurrentHp >= 10000 && CurrentTarget.CurrentHp <= 32400) return true;
                else if (CurrentTarget.IsJobCategory(JobRole.Melee) && CurrentTarget.CurrentHp >= 12000 && CurrentTarget.CurrentHp <= 15000) return true;
                else if (CurrentTarget.IsJobCategory(JobRole.Tank) && CurrentTarget.CurrentHp >= 12000 && CurrentTarget.CurrentHp <= 15000) return true;
                else if (PvPTargetHasWildfire && PvPTargetWildfireStatusTime <= 2f && CurrentTarget.CurrentHp >= 12000 && CurrentTarget.CurrentHp <= 35000) return true;
                else if (CurrentTarget.HasStatus(true, StatusID.ChainSaw) && CurrentTarget.CurrentHp >= 12000 && CurrentTarget.CurrentHp <= 40000) return true;

            }
        }

        // Chainsaw
        if (!IsPvPOverheated && ChainSawPvP.CanUse(out act, usedUp: false, skipAoeCheck: true) && Player.HasStatus(true, StatusID.ChainSawPrimed))
        {
            return true;
        }

        // Drill old
        if (!IsPvPOverheated && HasHostilesInRange && DrillPvP.CanUse(out act, usedUp: true) && Player.HasStatus(true, StatusID.DrillPrimed))
        {
            return true;
        }

        if (!IsPvPOverheated && !Player.HasStatus(true, StatusID.Analysis) && FullMetalFieldPvP.CanUse(out act, skipAoeCheck: true))
        {
            return true;
        }

        // Air Anchor is used if Player is not overheated and available
        if (!IsPvPOverheated && AirAnchorPvP.CanUse(out act/*, usedUp: true*/) && Player.HasStatus(true, StatusID.AirAnchorPrimed))
        {
            return true;
        }

        // Scattergun is used if Player is not overheated and available
        if (!IsPvPOverheated && ScattergunPvP.CanUse(out act, usedUp: true, skipAoeCheck: true) /*&& ScattergunPvP.Target.Target.DistanceToPlayer() <= 5*/)
        {
            return true;
        }

        // Blast Charge is used if available
        // Note: Stop Using Blast Charge if Player's HP is low + moving + not overheated (since our movement slows down a lot we do this to be able retreat)
        if (BlastChargePvP.CanUse(out act, skipCastingCheck: true) /*&& CurrentTarget != null && CurrentTarget.DistanceToPlayer() < 20*/)
        {
            if (Player.CurrentHp <= LowHPNoBlastChargeThreshold && HasHostilesInRange && LowHPNoBlastCharge && IsMoving) // Maybe add InCombat as well
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        return base.GeneralGCD(out act);
    }

    #endregion GCD Logic

    private bool EmergencyLowHP(out IAction? act)
    {
        if (Player.HasStatus(true, StatusID.Guard))
        {
            act = null;
            return false;
        }

        if (Player.CurrentHp <= 25000 && GuardPvP.CanUse(out _) && !Player.HasStatus(true, StatusID.Guard) && NumberOfHostilesInMaxRange >= 1)
        {
            return GuardPvP.CanUse(out act);
        }

        if (Player.CurrentMp == Player.MaxMp && Player.CurrentHp <= 37500 && !Player.HasStatus(true, StatusID.Guard) && RecuperatePvP.CanUse(out _))
        {
            return RecuperatePvP.CanUse(out act);
        }

        if (Player.CurrentMp >= 7500 && Player.CurrentHp <= 37500 && !Player.HasStatus(true, StatusID.Guard) && RecuperatePvP.CanUse(out _))
        {
            return RecuperatePvP.CanUse(out act);
        }

        if (Player.CurrentMp >= 5000 && Player.CurrentHp <= 32000 && !Player.HasStatus(true, StatusID.Guard) && RecuperatePvP.CanUse(out _))
        {
            return RecuperatePvP.CanUse(out act);
        }

        if (Player.CurrentMp >= 2500 && Player.CurrentHp <= 25000 && GuardPvP.Cooldown.IsCoolingDown && !Player.HasStatus(true, StatusID.Guard) && RecuperatePvP.CanUse(out _))
        {
            return RecuperatePvP.CanUse(out act);
        }
        act = null;
        return false;
    }

    private bool UseMCHLB(out IAction? action)
    {
        // Exit early if LB level is below 1
        if (CustomRotationEx.CurrentLimitBreakLevel == 0)
        {
            action = null;
            return false;
        }

        // Get all valid hostile targets
        List<IBattleChara> validTargets = CustomRotation.AllHostileTargets
        .Where(obj => obj.CurrentHp >= 15000 && obj.CurrentHp <= 30000 && (obj.IsJobCategory(JobRole.Healer) || obj.IsJobCategory(JobRole.RangedPhysical) || obj.IsJobCategory(JobRole.RangedMagical))) // Valid job categories only
        //.Where(obj => obj.CurrentHp >= 17000 && obj.CurrentHp <= 30000) // HP range for LB
        .Where(obj => obj.DistanceToPlayer() <= 50) // Exclude targets beyond 50 yalms
        //.Where(obj => obj.CurrentMp < 5000) // MP condition (low MP prevents healing)
        .OrderBy(obj => obj.CurrentHp) // Prioritize lowest HP first for kill confirmation
        //.ThenBy(obj => obj.CurrentMp) // Prioritize lowest MP next
        .ToList(); // Convert to a list for iteration

        // If we have a valid target, use Marksman's Spite LB
        if (validTargets.Count > 0 && MarksmansSpitePvP.CanUse(out action))
        {
            foreach (IBattleChara obj in validTargets)
            {
                MarksmansSpitePvP.Target = new TargetResult(obj, [obj], obj.Position);
                return true;
            }
            return true;
        }
        action = null;
        return false;
    }
}