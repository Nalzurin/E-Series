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

    public class Bastion_TurretHediff : Hediff
    {
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
            if (pawn.Downed)
            {
                Severity = 0f;
            }
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            ThingWithComps gun = ThingMaker.MakeThing(Definitions.Bastion_Minigun) as ThingWithComps;

            pawn.equipment.DestroyAllEquipment();
            pawn.equipment.AddEquipment(gun);
            pawn.Drawer.renderer.SetAllGraphicsDirty();
        }

        public override void PostRemoved()
        {
            pawn.equipment.DestroyAllEquipment();
            ThingWithComps gun = ThingMaker.MakeThing(Definitions.Bastion_ReconArmGun) as ThingWithComps;
            pawn.equipment.AddEquipment(gun);
            pawn.Drawer.renderer.SetAllGraphicsDirty();
        }

        public override void Tick()
        {
            pawn.pather.StopDead();
            base.Tick();
        }
    }
}
