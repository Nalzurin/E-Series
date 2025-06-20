using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Bastion
{
    public static class MechRepairUtilityBastion
    {
        public static Hediff GetHediffToHeal(Pawn mech)
        {
            Hediff hediff = null;
            float num = float.PositiveInfinity;
            foreach (Hediff hediff2 in mech.health.hediffSet.hediffs)
            {
                if (hediff2 is Hediff_Injury && hediff2.Severity < num)
                {
                    num = hediff2.Severity;
                    hediff = hediff2;
                }
            }

            if (hediff != null)
            {
                return hediff;
            }

            foreach (Hediff hediff3 in mech.health.hediffSet.hediffs)
            {
                if (hediff3 is Hediff_MissingPart)
                {
                    return hediff3;
                }
            }

            return null;
        }
    }
}
