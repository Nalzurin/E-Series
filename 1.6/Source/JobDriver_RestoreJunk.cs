using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse.Sound;
using Verse;
using VFECore;
using static UnityEngine.GraphicsBuffer;

namespace Bastion
{
    public class Bastion_JobDriver_RestoreJunk : JobDriver
    {
        public Bastion_AncientJunkComp Comp => job.targetA.Thing.TryGetComp<Bastion_AncientJunkComp>();
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            this.FailOn(FailCondition);
            if (job.count == 1)
            {
                Log.Message("Pawn is null");
                Toil jumpToil = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B);
                yield return jumpToil;
                yield return Toils_Haul.TakeToInventory(TargetIndex.B, job.count);
                Toil toilRepeatable = ToilMaker.MakeToil("MakeNewToils");
                toilRepeatable.initAction = delegate
                {
                    if (job.targetQueueA.Count > 0)
                    {
                        LocalTargetInfo targetB = job.targetQueueA[0];
                        job.targetQueueA.RemoveAt(0);
                        job.targetB = targetB;
                        JumpToToil(jumpToil);
                    }
                };
                yield return toilRepeatable;
            }

            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedOrNull(TargetIndex.A);
            Toil toil = Toils_General.WaitWith(TargetIndex.A, 600, useProgressBar: true, maintainPosture: true, maintainSleep: false, TargetIndex.A);
            /*            toil.WithProgressBarToilDelay(TargetIndex.A);
                        toil.FailOnDespawnedOrNull(TargetIndex.A);
                        toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);*/
            yield return toil;
            yield return new Toil
            {
                initAction = delegate
                {
                    Comp.RestoreMech(pawn);
                    pawn.inventory.RemoveCount(job.thingDefToCarry, 2, true);
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }
        public bool FailCondition()
        {
            return base.Map.designationManager.DesignationOn(base.TargetThingA, Definitions.Bastion_RestoreJunk) == null;
        }
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (!ReservationUtility.Reserve(pawn, TargetA, job, 1, 1, (ReservationLayerDef)null, errorOnFailed))
            {
                return false;
            }
            if(job.count == 1)
            {
                int count = job.thingDefToCarry == ThingDefOf.AIPersonaCore ? 1 : 2;
                int stack = count >= TargetB.Thing.stackCount ? TargetB.Thing.stackCount : count;
                count -= stack;
                if (!ReservationUtility.Reserve(pawn, TargetA, job, 1, count, (ReservationLayerDef)null, errorOnFailed))
                {
                    return false;
                }
                foreach (LocalTargetInfo target in job.targetQueueA)
                {
                    stack = count >= target.Thing.stackCount ? target.Thing.stackCount : count;
                    if (!ReservationUtility.Reserve(pawn, TargetA, job, 1, count, (ReservationLayerDef)null, errorOnFailed))
                    {
                        return false;
                    }
                    count -= stack;
                }
            }
            
            return true;
        }
    }
}
