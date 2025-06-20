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
    public class Bastion_PawnRenderNode_Bastion : PawnRenderNode_AnimalPart
    {
        public Bastion_PawnRenderNode_Bastion(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
            : base(pawn, props, tree)
        {
        }

        public override Graphic GraphicFor(Pawn pawn)
        {
            if (hasHediff(pawn))
            {
                Graphic graphic = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.Graphic;
                return GraphicDatabase.Get<Graphic_Single>(graphic.path + "_turret", ShaderDatabase.Cutout, graphic.drawSize, Color.white);
            }
            return base.GraphicFor(pawn);
        }

        public bool hasHediff(Pawn pawn)
        {
            bool var = pawn.health.hediffSet.HasHediff(Definitions.Bastion_Turret);
            return var;
        }
    }
}
