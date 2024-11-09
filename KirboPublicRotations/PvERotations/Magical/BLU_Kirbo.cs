namespace KirboPublicRotations.PvERotations.Magical;

[BetaRotation]
[Rotation("Kirbo's BLU", CombatType.PvE, GameVersion = "7.05", Description = "A BLU rotation that causes depression")]
[Api(4)]
public class BluKirbo : BlueMageRotation
{
    [RotationConfig(CombatType.PvE, Name = "Just fucking self destruct, lmao")]
    public bool Selfdestruct { get; set; } = false;

    private static IBaseAction SelfDestructPvE { get; } = new BaseAction((ActionID)11408);

    //GCD actions here.
    protected override bool GeneralGCD(out IAction? act)
    {
        act = null;
        if (Selfdestruct && !Player.HasStatus(true, StatusID.BrushWithDeath) && SelfDestructPvE.Use())
        {
            return true;
        }
        return base.GeneralGCD(out act);
    }

    //0GCD actions here.
    protected override bool AttackAbility(IAction nextGCD, out IAction? act)
    {
        return base.AttackAbility(nextGCD, out act);
    }
}
