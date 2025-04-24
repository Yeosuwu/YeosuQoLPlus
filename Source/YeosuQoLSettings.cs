using Verse;

namespace YeosuQoLPlus
{
    public class YeosuQoLSettings : ModSettings
    {
        public bool autoWorkPriority = true;
        public bool autoHarvest = true;
        public bool autoPrison = true;
        public bool smartMaterials = true;
        public bool pawnHUD = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref autoWorkPriority, "autoWorkPriority", true);
            Scribe_Values.Look(ref autoHarvest, "autoHarvest", true);
            Scribe_Values.Look(ref autoPrison, "autoPrison", true);
            Scribe_Values.Look(ref smartMaterials, "smartMaterials", true);
            Scribe_Values.Look(ref pawnHUD, "pawnHUD", true);
        }
    }
}