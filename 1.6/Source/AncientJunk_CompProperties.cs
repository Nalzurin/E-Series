using Verse;

namespace Bastion
{
    public class Bastion_AncientJunk_CompProperties : CompProperties
    {
        public ThingDef mechDef;
        public Rot4? spawnRotation;
        public Bastion_AncientJunk_CompProperties()
        {
            compClass = typeof(Bastion_AncientJunkComp);
        }
    }
}
