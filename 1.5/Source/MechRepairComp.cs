using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Bastion
{

    public class Bastion_MechRepairComp : HediffComp
    {
        private int ticksLeft;

        public Bastion_MechRepair_CompProperties Properties => props as Bastion_MechRepair_CompProperties;

        public new Pawn Pawn => parent.pawn;

        public int RepairPerTicks => Mathf.RoundToInt(1f / Pawn.GetStatValue(StatDefOf.MechRepairSpeed) * 120f);

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (ticksLeft <= 0)
            {
                Repair(Pawn, Properties.Factor);
                ticksLeft = RepairPerTicks;
            }
            else
            {
                ticksLeft--;
            }
        }

        public static void Repair(Pawn pawn, float factor = 1f)
        {
            Hediff hediff = MechRepairUtilityBastion.GetHediffToHeal(pawn);
            if (hediff == null)
            {
                MechRepairUtility.GenerateWeapon(pawn);
            }
            else if (hediff is Hediff_MissingPart missingPart)
            {
                pawn.health.RemoveHediff(missingPart);
            }
            else
            {
                hediff.Heal(factor);
            }
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref ticksLeft, "ticksLeft", 0);
        }
    }
}
