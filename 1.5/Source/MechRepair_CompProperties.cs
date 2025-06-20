using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Bastion
{
    public class Bastion_MechRepair_CompProperties : HediffCompProperties
    {
        public float Factor = 1f;

        public Bastion_MechRepair_CompProperties()
        {
            compClass = typeof(Bastion_MechRepairComp);
        }
    }
}
