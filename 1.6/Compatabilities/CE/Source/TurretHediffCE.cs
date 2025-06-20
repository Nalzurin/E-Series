using Bastion;
using CombatExtended;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BastionCE
{

    public class Bastion_TurretHediffCE : Hediff
    {
        int ammoCount;
        AmmoDef ammoDef;
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

            if (ModLister.HasActiveModWithName("Combat Extended"))
            {
                CombatExtended.CompAmmoUser ammo = gun.GetComp<CombatExtended.CompAmmoUser>();
                ammo.CurrentAmmo = DefDatabase<AmmoDef>.GetNamed("Ammo_762x51mmNATO_HE");
                ammo.ResetAmmoCount();
            }


            CombatExtended.CompAmmoUser ammoRecon = pawn.equipment.AllEquipmentListForReading.Where((c) => { return c.def == Definitions.Bastion_ReconArmGun; }).First().GetComp<CombatExtended.CompAmmoUser>();
            ammoCount = ammoRecon.CurMagCount;
            Log.Message(ammoRecon.MagAmmoCount);
            ammoDef = ammoRecon.CurrentAmmo;
            pawn.equipment.DestroyAllEquipment();
            pawn.equipment.AddEquipment(gun);
            pawn.Drawer.renderer.SetAllGraphicsDirty();
        }

        public override void PostRemoved()
        {
            ThingWithComps gun = ThingMaker.MakeThing(Definitions.Bastion_ReconArmGun) as ThingWithComps;
            CombatExtended.CompAmmoUser ammo = gun.GetComp<CombatExtended.CompAmmoUser>();
            ammo.CurrentAmmo = ammoDef;
            ammo.CurMagCount = ammoCount;
            pawn.equipment.DestroyAllEquipment();
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
