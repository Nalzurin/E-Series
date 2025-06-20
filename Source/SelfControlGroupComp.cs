using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Bastion
{
    public class SelfControlGroupComp : ThingComp
    {
        private Pawn mechanitor;

        private Pawn? pawn;

        public Pawn Mechanitor => mechanitor;

        public Pawn_MechanitorTracker Tracker => mechanitor.mechanitor;

        public MechanitorControlGroup Group => Tracker.GetControlGroup(Pawn);

        public Pawn Pawn => pawn ?? (pawn = parent as Pawn);

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look(ref mechanitor, "mechanitor");
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
        }

        private void SetOverseer(Pawn newOverseer)
        {
            Pawn pawn = Pawn;
            pawn.GetOverseer()?.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, pawn);
            newOverseer.relations.AddDirectRelation(PawnRelationDefOf.Overseer, pawn);
        }
    }
}
