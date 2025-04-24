using Verse;
using RimWorld;

namespace YeosuQoLPlus
{
    public class YeosuQoLGameComponent : GameComponent
    {
        private bool initialized = false;

        public YeosuQoLGameComponent(Game game) { }

        public override void FinalizeInit()
        {
            base.FinalizeInit();

            if (!initialized && YeosuQoLMod.settings.autoWorkPriority)
            {
                initialized = true;

                if (!Find.PlaySettings.useWorkPriorities)
                {
                    Find.PlaySettings.useWorkPriorities = true;
                }

                Messages.Message("[YeosuQoL+] 작업 우선순위 자동 설정이 적용되었습니다.", MessageTypeDefOf.PositiveEvent);
                AutoWorkManager.ApplyWorkPriorities();
            }
        }
    }
}