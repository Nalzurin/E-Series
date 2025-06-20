using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Bastion
{
    public static class Extensions
    {
        public const string LogPrefix = "[Bastion] ";

        public static void DebugMessage(object message, Action<string> logAction = null, Color? color = null)
        {
            if (logAction == null)
            {
                logAction = Log.Message;
            }
            string text = "[Bastion] " + message;
            if (color.HasValue)
            {
                text = "<color=#" + ColorUtility.ToHtmlStringRGBA(color.Value) + ">" + text + "</color>";
            }
            logAction(text);
        }

        public static IEnumerable<Pawn> GetSelectedDraftedMechs(Pawn _mechanitor)
        {
            Pawn mechanitor = _mechanitor;
            return Find.Selector.SelectedPawns.Where((Pawn x) => x.GetOverseer() == mechanitor && x.Drafted);
        }
    }
}
