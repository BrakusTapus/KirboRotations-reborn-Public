namespace KirboPublicRotations.PvERotations.Ranged;

[BetaRotation]
[Rotation("Kirbo's MCH\n [Public]", CombatType.PvE, GameVersion = "7.25", Description = "")]
[Api(4)]
public class MchKirboPublic : MachinistRotation
{
    #region Countdown Logic
    // Override the method for actions to be taken during the countdown phase of combat
    protected override IAction? CountDownAction(float remainTime)
    {
        if (remainTime > 5) return base.CountDownAction(remainTime);
        if (remainTime < 5 && ReassemblePvE.CanUse(out var act)) return act;
        return base.CountDownAction(remainTime);
    }
    #endregion

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

    private bool ShouldUseReassemble(out IAction? act, IAction nextGCD)
    {


        act = null;
        return false;
    }
}
