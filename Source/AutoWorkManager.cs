using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib; // 추가

namespace YeosuQoLPlus
{
    public static class AutoWorkManager
    {
        public static void ApplyWorkPriorities()
        {
            List<Pawn> pawns = PawnsFinder.AllMaps_FreeColonists.ToList();

            foreach (Pawn pawn in pawns)
            {
                if (pawn.workSettings == null)
                    continue;

                pawn.workSettings.EnableAndInitialize();

                // 무조건 1순위 고정
                ForcePriority(pawn, WorkTypeDefOf.Firefighter);
                ForcePriority(pawn, WorkTypeDefOf.Cleaning);
                ForcePriority(pawn, WorkTypeDefOf.Hauling);

                TryForcePriorityByDefName(pawn, "Patient");
                TryForcePriorityByDefName(pawn, "PatientBedRest");
                TryForcePriorityByDefName(pawn, "BasicWorker");

                // Biotech 육아 처리
                if (ModsConfig.BiotechActive && DefDatabase<WorkTypeDef>.AllDefs.Any(d => d.defName == "Childcare"))
                {
                    WorkTypeDef childcare = DefDatabase<WorkTypeDef>.AllDefs.First(d => d.defName == "Childcare");
                    SetPriorityIfNotDisabled(pawn, childcare, 4);
                }
            }

            foreach (WorkTypeDef work in DefDatabase<WorkTypeDef>.AllDefsListForReading)
            {
                if (work == WorkTypeDefOf.Firefighter ||
                    work == WorkTypeDefOf.Cleaning ||
                    work == WorkTypeDefOf.Hauling ||
                    work.defName == "Patient" ||
                    work.defName == "PatientBedRest" ||
                    work.defName == "BasicWorker")
                {
                    continue;
                }

                List<(Pawn pawn, int skill)> scored = new();
                foreach (Pawn pawn in pawns)
                {
                    if (pawn.WorkTypeIsDisabled(work)) continue;

                    int skill = 0;
                    if (work.relevantSkills != null && work.relevantSkills.Count > 0)
                    {
                        var skillDef = work.relevantSkills[0];
                        skill = pawn.skills?.GetSkill(skillDef)?.Level ?? 0;
                    }
                    scored.Add((pawn, skill));
                }

                if (scored.Count == 0) continue;
                scored = scored.OrderByDescending(s => s.skill).ToList();
                int topScore = scored.First().skill;

                for (int i = 0; i < scored.Count; i++)
                {
                    var (pawn, skill) = scored[i];

                    if (skill == 0 || (skill <= 2 && i > 0))
                    {
                        pawn.workSettings.SetPriority(work, 0);
                        continue;
                    }

                    int priority = Mathf.Clamp(1 + (i * 3 / scored.Count), 1, 4);

                    if (work == WorkTypeDefOf.Doctor)
                    {
                        if (skill < 4)
                        {
                            pawn.workSettings.SetPriority(work, 0);
                            continue;
                        }
                    }

                    pawn.workSettings.SetPriority(work, priority);
                }
            }
        }

        private static void ForcePriority(Pawn pawn, WorkTypeDef work)
        {
            if (!pawn.WorkTypeIsDisabled(work))
                pawn.workSettings.SetPriority(work, 1);
        }

        private static void TryForcePriorityByDefName(Pawn pawn, string defName)
        {
            var work = DefDatabase<WorkTypeDef>.AllDefs.FirstOrDefault(w => w.defName == defName);
            if (work != null && !pawn.WorkTypeIsDisabled(work))
                pawn.workSettings.SetPriority(work, 1);
        }

        private static void SetPriorityIfNotDisabled(Pawn pawn, WorkTypeDef work, int priority)
        {
            if (!pawn.WorkTypeIsDisabled(work))
                pawn.workSettings.SetPriority(work, priority);
        }
    }
    }