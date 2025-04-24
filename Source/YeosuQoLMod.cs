using Verse;
using RimWorld;
using UnityEngine;
using HarmonyLib;

namespace YeosuQoLPlus
{
    public class YeosuQoLMod : Mod
    {
        public static YeosuQoLSettings settings;

        public YeosuQoLMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<YeosuQoLSettings>();

            var harmony = new Harmony("YeosuQoLPlus");
            harmony.PatchAll();
        }

        public override string SettingsCategory() => "Yeosu QoL+";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard list = new Listing_Standard();
            list.Begin(inRect);
            list.CheckboxLabeled("자동 작업 우선순위 설정", ref settings.autoWorkPriority);
            list.CheckboxLabeled("성숙 작물 자동 수확", ref settings.autoHarvest);
            list.CheckboxLabeled("포로 자동 처리 UI", ref settings.autoPrison);
            list.CheckboxLabeled("자원 우선 사용 알고리즘", ref settings.smartMaterials);
            list.CheckboxLabeled("Pawn 요약 HUD 활성화", ref settings.pawnHUD);
            list.End();
            settings.Write();
        }
    }
}