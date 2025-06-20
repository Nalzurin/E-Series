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

namespace Bastion
{
    public class Bastion_JobDriver_RestoreJunk : JobDriver
    {
        public Bastion_AncientJunkComp Comp => job.targetA.Thing.TryGetComp<Bastion_AncientJunkComp>();

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            this.FailOn(FailCondition);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_Haul.TakeToInventory(TargetIndex.B, job.count);
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
                    pawn.inventory.RemoveCount(job.targetB.Thing.def, job.count, true);
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
            return ReservationUtility.Reserve(pawn, TargetA, job, 1, 1, (ReservationLayerDef)null, errorOnFailed) && ReservationUtility.Reserve(pawn, (LocalTargetInfo)TargetB, job, 1, 1, (ReservationLayerDef)null, errorOnFailed);
        }
    }
}
