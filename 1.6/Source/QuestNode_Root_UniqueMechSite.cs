using RimWorld.Planet;
using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Bastion
{
    public class QuestNode_Root_UniqueMechSite : QuestNode
    {
        private const string SitePartTag = "UniqueMechSite";

        private const int MaxDistanceFromColony = 9;

        private const int MinDistanceFromColony = 3;

        private const float MinPoints = 100f;

        private const int TimeoutTicks = 900000;

        private const float EmpireSitePointsThreshold = 2000f;

        protected override bool TestRunInt(Slate slate)
        {
            if (!Find.Storyteller.difficulty.allowViolentQuests)
            {
                return false;
            }
            QuestGenUtility.TestRunAdjustPointsForDistantFight(slate);
            float num = slate.Get("points", 0f);
            if (num < MinPoints)
            {
                num = MinPoints;
            }
            Faction faction;
            if (TryFindSiteTile(out var _))
            {
                return TryFindFaction(num, out faction);
            }
            return false;
        }

        protected override void RunInt()
        {
            Quest quest = QuestGen.quest;
            Slate slate = QuestGen.slate;
            float num = slate.Get("points", 0f);
            if (num < MinPoints)
            {
                num = MinPoints;
            }
            TryFindSiteTile(out var tile);
            TryFindFaction(num, out var faction);
            slate.Set("faction", faction);
            IEnumerable<SitePartDef> source = DefDatabase<SitePartDef>.AllDefs.Where((SitePartDef def) => def.tags != null && def.tags.Contains("UniqueMechSite") && typeof(SitePartWorker_UniqueMechSite).IsAssignableFrom(def.workerClass));
            Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[1]
            {
            new SitePartDefWithParams(source.RandomElementByWeight((SitePartDef sp) => sp.selectionWeight), new SitePartParams{


                threatPoints = num
            })
            }, tile, faction);
            quest.SpawnWorldObject(site);
            slate.Set("site", site);
            string inSignalEnable = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
            string text = QuestGenUtility.HardcodedSignalWithQuestID("site.AllEnemiesDefeated");
            string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
            QuestPart_Choice questPart_Choice = quest.RewardChoice();
            QuestPart_Choice.Choice item = new QuestPart_Choice.Choice
            {
                rewards = { (Reward)new Reward_CampLoot() }
            };
            quest.WorldObjectTimeout(site, TimeoutTicks);
            quest.Delay(TimeoutTicks, delegate
            {
                QuestGen_End.End(quest, QuestEndOutcome.Fail);
            });
            Quest quest3 = quest;
            string inSignal3 = text;
            quest3.Notify_PlayerRaidedSomeone(null, site, inSignal3);
            quest.End(QuestEndOutcome.Success, 0, null, inSignal);
        }

        private bool TryFindFaction(float points, out Faction faction)
        {
            return Find.FactionManager.AllFactions.Where((Faction f) => FactionUsable(f, points)).TryRandomElement(out faction);
        }

        private bool FactionUsable(Faction f, float points)
        {
            if (ModsConfig.RoyaltyActive && points < EmpireSitePointsThreshold && f == Faction.OfEmpire)
            {
                return false;
            }
            if (f.def.humanlikeFaction && !f.def.pawnGroupMakers.NullOrEmpty() && f.AllyOrNeutralTo(Faction.OfPlayer))
            {
                return !f.def.permanentEnemy;
            }
            return false;
        }

        private bool TryFindSiteTile(out PlanetTile tile)
        {
            return TileFinder.TryFindNewSiteTile(out tile, MinDistanceFromColony, MaxDistanceFromColony);
        }
    }
}
