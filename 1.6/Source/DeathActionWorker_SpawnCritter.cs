using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI.Group;
using static System.Collections.Specialized.BitVector32;
using Verse.Noise;

namespace Bastion
{
    public class DeathActionWorker_SpawnCritter : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse, Lord prevLord)
        {
            List<PawnKindDef> things = DefDatabase<PawnKindDef>.AllDefs.Where((c) => { return c.defName == "Hare" || c.defName == "Snowhare" || c.defName == "Rat" || c.defName == "Boomrat"  || c.defName.Contains("Iguana") || c.defName.Contains("Squirrel") || c.defName.Contains("Guinea") || c.defName.Contains("Duck") || c.defName.Contains("Chinchilla") || c.defName.Contains("Chicken");}).ToList();
            PawnKindDef animal = things.RandomElement();
            Faction faction = FactionUtility.DefaultFactionFrom(animal.defaultFactionDef);
            Pawn pawn = PawnGenerator.GeneratePawn(animal, faction);
            GenSpawn.Spawn(pawn, corpse.PositionHeld, corpse.MapHeld, WipeMode.VanishOrMoveAside);
        }
    }
}
