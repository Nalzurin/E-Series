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
    public class Bastion_GenStep_SpawnWreck : GenStep
    {
        private ThingDef wreckDef;
        public override int SeedPart => 1234731256;

        public override void Generate(Map map, GenStepParams parms)
        {

            CellFinder.TryFindRandomCell(map, (IntVec3 c) => Validator(c, map), out var result);
            foreach (IntVec3 item in GridShapeMaker.IrregularLump(result, map, 9))
            {
                FilthMaker.TryMakeFilth(item, map, ThingDefOf.Filth_MachineBits);
            }
            Thing wreck = ThingMaker.MakeThing(wreckDef); 
            GenSpawn.Spawn(wreck, result, map);
        }
        private bool Validator(IntVec3 c, Map map)
        {
            if (!c.Standable(map))
            {
                return false;
            }
            if (c.DistanceToEdge(map) <= 2)
            {
                return false;
            }
            if (!c.GetRoom(map).ProperRoom || c.GetRoom(map) != null)
            {
                return false;
            }
            if (!map.generatorDef.isUnderground && !map.reachability.CanReachMapEdge(c, TraverseMode.PassDoors))
            {
                return false;
            }
            if (!map.reachability.CanReachMapEdge(c, TraverseMode.ByPawn))
            {
                return false;
            }
            return true;
        }
    }

    
}