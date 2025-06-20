using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Bastion
{
    public class Bastion_CancelTurretAbility : Ability
    {
        public bool Enabled => GetHediff() != null;

        public Bastion_CancelTurretAbility()
        {
        }

        public Bastion_CancelTurretAbility(Pawn pawn)
            : base(pawn)
        {
        }

        public Bastion_CancelTurretAbility(Pawn pawn, Precept sourcePrecept)
            : base(pawn, sourcePrecept)
        {
        }

        public Bastion_CancelTurretAbility(Pawn pawn, AbilityDef def)
            : base(pawn, def)
        {
        }

        public Bastion_CancelTurretAbility(Pawn pawn, Precept sourcePrecept, AbilityDef def)
            : base(pawn, sourcePrecept, def)
        {
        }

        public Hediff? GetHediff()
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(Definitions.Bastion_Turret);
        }

        public override bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Activate(target, dest);
            Pawn_HealthTracker health = pawn.health;
            Hediff hediff = GetHediff();
            if (hediff != null)
            {
                health.RemoveHediff(hediff);
            }
            return true;
        }
    }
}
