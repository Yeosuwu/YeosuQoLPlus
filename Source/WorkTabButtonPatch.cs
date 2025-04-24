using RimWorld;
using UnityEngine;
using Verse;
using HarmonyLib;

namespace YeosuQoLPlus
{
    [HarmonyPatch(typeof(MainTabWindow_Work), "DoWindowContents")]
    public static class WorkTabButtonPatch
    {
        public static void Postfix(Rect __0) // 'inRect' 대신 '__0' 사용
        {
            float buttonWidth = 120f;
            float buttonHeight = 32f;

            Rect buttonRect = new Rect(
                __0.xMax - buttonWidth - 10f,
                __0.yMax - buttonHeight - 10f,
                buttonWidth,
                buttonHeight
            );

            if (Widgets.ButtonText(buttonRect, "작업 자동 설정"))
            {
                AutoWorkManager.ApplyWorkPriorities();
                Messages.Message("[YeosuQoL+] 작업 우선순위가 수동으로 적용되었습니다.", MessageTypeDefOf.PositiveEvent);
            }

            TooltipHandler.TipRegion(buttonRect, "정착민들의 작업 우선순위를 자동으로 설정합니다.");
        }
    }
}
