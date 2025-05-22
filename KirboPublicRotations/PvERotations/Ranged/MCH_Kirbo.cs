namespace KirboPublicRotations.PvERotations.Ranged;

[BetaRotation]
[Rotation("Kirbo's MCH", CombatType.PvE, GameVersion = "7.21", Description = "")]
[Api(4)]
public class MchKirbo : MachinistRotation
{
    //GCD actions here.
    protected override bool GeneralGCD(out IAction? act)
    {
        return base.GeneralGCD(out act);
    }

    //0GCD actions here.
    protected override bool AttackAbility(IAction nextGCD, out IAction? act)
    {
        return base.AttackAbility(nextGCD, out act);
    }
}
