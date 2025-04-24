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

            if (initialized || !YeosuQoLMod.settings.autoWorkPriority)
                return;

            initialized = true;

            if (!Find.PlaySettings.useWorkPriorities)
            {
                Find.PlaySettings.useWorkPriorities = true;
            }

            AutoWorkManager.ApplyWorkPriorities();

            // 게임 시작 초반 (불러온 게임은 제외)
            if (Find.TickManager.TicksGame < 10)
            {
                Messages.Message("[YeosuQoL+] 작업 우선순위 자동 설정이 적용되었습니다.", MessageTypeDefOf.PositiveEvent);
            }
        }
    }
}
