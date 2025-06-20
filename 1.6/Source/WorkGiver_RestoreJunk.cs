using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using VFECore;
using RimWorld;

namespace Bastion
{
    public class Bastion_WorkGiver_RestoreJunk : WorkGiver_Scanner
    {
        public override PathEndMode PathEndMode => PathEndMode.Touch;
        private List<Thing> reachableThings = new();
        bool allInInventory = false;
        int count = 0;
        public override Danger MaxPathDanger(Pawn pawn)
        {
            return Danger.Deadly;
        }

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            foreach (Designation d in Find.CurrentMap.designationManager.SpawnedDesignationsOfDef(Definitions.Bastion_RestoreJunk))
            {
                yield return (Thing)d.target;
            }
        }
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Log.Message("COunt of components: " + Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.ComponentSpacer).Count());
            if (!pawn.CanReserveAndReach(t, PathEndMode, MaxPathDanger(pawn), 1, 1, null, forced))
            {
                return false;
            }
            if (pawn.mechanitor == null)
            {
                return false;
            }
            List<Thing> pawnItems;
            if (t.def == Definitions.Bastion_AncientBastionJunk)
            {
                pawnItems = pawn.inventory.innerContainer.InnerListForReading.Where(c => c.def == ThingDefOf.ComponentSpacer).ToList();

                Log.Message("Pawn items: " + pawnItems.Count);
                foreach (Thing thing in pawnItems)
                {
                    Log.Message(thing.Label);
                    count += thing.stackCount;
                }
                if (count >= 2)
                {
                    allInInventory = true;
                    return true;
                }
                List<Thing> things = pawn.MapHeld.listerThings.ThingsOfDef(ThingDefOf.ComponentSpacer);
                foreach (Thing thing in things)
                {
                    count += thing.stackCount;
                    reachableThings.Add(thing);
                    if (count >= 2)
                    {
                        break;
                    }
                }
            }
            if (t.def == Definitions.Bastion_AncientGammaJunk && !pawn.MapHeld.listerThings.ThingsOfDef(ThingDefOf.AIPersonaCore).Any())
            {
                return false;
            }
            pawnItems = pawn.inventory.innerContainer.InnerListForReading.Where(c => c.def == ThingDefOf.AIPersonaCore).ToList();

            foreach (Thing thing in pawnItems)
            {
                Log.Message(thing.Label);
                count += thing.stackCount;
            }
            if (count >= 1)
            {
                allInInventory = true;
                return true;
            }
            if (t.Map.designationManager.DesignationOn(t, Definitions.Bastion_RestoreJunk) == null)
            {
                return false;
            }
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Job job;
            //Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.ComponentSpacer).First();
            if (t.def == Definitions.Bastion_AncientBastionJunk)
            {
                job = JobMaker.MakeJob(Definitions.Bastion_RestoreMechJunk);
                job.targetA = t;
                job.thingDefToCarry = ThingDefOf.ComponentSpacer;
                if (allInInventory)
                {
                    job.targetC = pawn;
                }
                else
                {
                    job.targetQueueA = new();
                    GetNearbyComponents(pawn, t, ThingDefOf.ComponentSpacer, count, 2);
                    job.targetB = reachableThings[0];
                    job.targetC = null;
                    job.count = 1;
                    reachableThings.RemoveAt(0);
                    foreach (Thing c in reachableThings)
                    {
                        job.targetQueueA.Add(c);
                    }
                }
            }
            else
            {
                job = JobMaker.MakeJob(Definitions.Bastion_RestoreMechJunk);
                job.targetA = t;
                job.thingDefToCarry = ThingDefOf.AIPersonaCore;
                if (allInInventory)
                {
                    job.targetC = pawn;
                }
                else
                {
                    job.targetQueueA = new();
                    GetNearbyComponents(pawn, t, ThingDefOf.AIPersonaCore, count, 1);
                    job.targetB = reachableThings[0];
                    job.targetC = null;
                    job.count = 1;
                    reachableThings.RemoveAt(0);
                    foreach (Thing c in reachableThings)
                    {
                        job.targetQueueA.Add(c);
                    }
                }

            }
            return job;

        }
        private void GetNearbyComponents(Pawn p, Thing t, ThingDef def, int currentCount, int neededCount)
        {

            foreach (Thing item in GenRadial.RadialDistinctThingsAround(t.Position, t.MapHeld, 32f, useCenter: false))
            {
                if (currentCount >= neededCount)
                {
                    break;
                }
                if (item.def == def && GenAI.CanUseItemForWork(p, item))
                {
                    currentCount += item.stackCount;
                    reachableThings.Add(item);
                }
            }
        }
    }
}

