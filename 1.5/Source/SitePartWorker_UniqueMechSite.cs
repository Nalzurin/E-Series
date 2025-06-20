using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static System.Collections.Specialized.BitVector32;

namespace Bastion
{
    public class SitePartWorker_UniqueMechSite : SitePartWorker
    {
        private int TicksUntilRaid = 625;
        private int currentTicks = 0;
        private bool tick = false;

        public override void Init(Site site, SitePart sitePart)
        {
            base.Init(site, sitePart);
        }
        public override void SitePartWorkerTick(SitePart sitePart)
        {
            base.SitePartWorkerTick(sitePart);
            if (tick)
            {
                currentTicks++;
                if (currentTicks == TicksUntilRaid)
                {
                    tick = false;
                    IncidentParms incidentParms = new IncidentParms();
                    incidentParms.target = sitePart.site.Map;
                    incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
                    incidentParms.points = sitePart.site.ActualThreatPoints;
                    incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
                    incidentParms.spawnRotation = Rot4.Random;
                    incidentParms.faction = Find.FactionManager.RandomRaidableEnemyFaction(true, true, true);
                    if (incidentParms.faction == Faction.OfEntities)
                    {
                        incidentParms.pawnGroupKind = PawnGroupKindDefOf.Fleshbeasts;
                    }
                    IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
                }
            }


        }
        public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
        {
            return (string)("Hostiles".Translate() + ": " + "Unknown".Translate().CapitalizeFirst()) + ("\n" + "Contains".Translate() + ": " + "Unknown".Translate().CapitalizeFirst());
        }
        public override void PostMapGenerate(Map map)
        {

            Site site = map.Parent as Site;
            foreach (Thing allThing in map.listerThings.AllThings)
            {
                if (allThing.def.category == ThingCategory.Item)
                {
                    CompForbiddable compForbiddable = allThing.TryGetComp<CompForbiddable>();
                    if (compForbiddable != null && !compForbiddable.Forbidden)
                    {
                        allThing.SetForbidden(value: true, warnOnFail: false);
                    }



                }
            }
        }
    }
}
