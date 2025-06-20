using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Bastion
{
    public class GenStep_ScatterChance : GenStep_ScatterLayout
    {
        public float chance;

        protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
        {
            if (Rand.Chance(chance))
            {
                base.ScatterAt(loc, map, parms, count);
            }
        }
    }
}
