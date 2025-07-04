using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirboPublicRotations.Helpers;

[Rotation("Dummy", CombatType.None, GameVersion = "9.99")]
[Api(5)]
public sealed class DummyRotation : MachinistRotation
{


    #region Main Methods
    protected override IAction? CountDownAction(float remainTime)
    {
        return base.CountDownAction(remainTime);
    }

    // Emergency Ability: These always fire off, with utmost priority, if they can be used.
    protected override bool EmergencyAbility(IAction nextGCD, out IAction? act)
    {
        return base.EmergencyAbility(nextGCD, out act);
    }

    // AttackAbility: If your GCD is cooling down, you will use one of these attack oGCDs.
    protected override bool AttackAbility(IAction nextGCD, out IAction? act)
    {
        return base.AttackAbility(nextGCD, out act);
    }

    // GeneralGCD: If you can use a GCD, you will use something from here.
    protected override bool GeneralGCD(out IAction? act)
    {
        return base.GeneralGCD(out act);
    }
    #endregion

    #region Extra Methods
    /// <summary>
    /// Gets whether this rotation is in burst window
    /// Example: Player is bursting if player has "..." status
    /// </summary>
    public override bool IsBursting()
    {
        return base.IsBursting();
    }

    /// <summary>
    /// Updates the custom fields.
    /// Example: Can be used to update a user defined property's value
    /// </summary>
    protected override void UpdateInfo()
    {
        base.UpdateInfo();
    }

    /// <summary>
    /// Handles actions when the territory changes.
    /// Example: Can be useful to set or reset user defined value's
    /// </summary>
    public override void OnTerritoryChanged()
    {
        base.OnTerritoryChanged();
    }

    /// <summary>
    /// Displays extra status information.
    /// Example: Can be useful to keep track of user defined value's, or when debugging 
    /// </summary>
    public override void DisplayStatus()
    {
        base.DisplayStatus();
    }
    #endregion


}
