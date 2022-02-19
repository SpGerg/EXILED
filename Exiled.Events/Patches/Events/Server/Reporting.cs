// -----------------------------------------------------------------------
// <copyright file="Reporting.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Server
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using Exiled.Events.EventArgs;
    using Exiled.Events.Handlers;

    using HarmonyLib;

    using NorthwoodLib.Pools;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="CheaterReport.IssueReport(GameConsoleTransmission, string, string, string, string, string, string, ref string, ref byte[], string, int, string, string)"/>
    /// and <see cref="CheaterReport.UserCode_CmdReport(int, string, byte[], bool)"/>.
    /// Adds the <see cref="Server.ReportingCheater"/> and <see cref="Server.LocalReporting"/> events.
    /// </summary>
    [HarmonyPatch(typeof(CheaterReport), nameof(CheaterReport.UserCode_CmdReport))]
    internal static class Reporting
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            LocalBuilder mem_0x01 = generator.DeclareLocal(typeof(LocalReportingEventArgs));
            LocalBuilder mem_0x02 = generator.DeclareLocal(typeof(ReportingCheaterEventArgs));

            const int offset = -4;
            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Ldc_I4_7);

            Label ret = generator.DefineLabel();

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldloc_3),
                new CodeInstruction(OpCodes.Call, Method(typeof(API.Features.Player), nameof(API.Features.Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Ldloc_2),
                new CodeInstruction(OpCodes.Call, Method(typeof(API.Features.Player), nameof(API.Features.Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Ldarg_2),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(LocalReportingEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Stloc_S, mem_0x01.LocalIndex),
                new CodeInstruction(OpCodes.Call, Method(typeof(Server), nameof(Handlers.Server.OnLocalReporting))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(LocalReportingEventArgs), nameof(LocalReportingEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brfalse_S, ret),
                new CodeInstruction(OpCodes.Ldloc_S, mem_0x01.LocalIndex),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(LocalReportingEventArgs), nameof(LocalReportingEventArgs.Reason))),
                new CodeInstruction(OpCodes.Starg_S, 2),
            });

            index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Ldarg_0) + offset;

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldloc_3).MoveLabelsFrom(newInstructions[index]),
                new CodeInstruction(OpCodes.Call, Method(typeof(API.Features.Player), nameof(API.Features.Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Ldloc_2),
                new CodeInstruction(OpCodes.Call, Method(typeof(API.Features.Player), nameof(API.Features.Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Ldsfld, Field(typeof(ServerConsole), nameof(ServerConsole.Port))),
                new CodeInstruction(OpCodes.Ldarg_2),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(ReportingCheaterEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Stloc_S, mem_0x02.LocalIndex),
                new CodeInstruction(OpCodes.Call, Method(typeof(Server), nameof(Handlers.Server.OnReportingCheater))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ReportingCheaterEventArgs), nameof(ReportingCheaterEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brfalse_S, ret),
                new CodeInstruction(OpCodes.Ldloc_S, mem_0x02.LocalIndex),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ReportingCheaterEventArgs), nameof(ReportingCheaterEventArgs.Reason))),
                new CodeInstruction(OpCodes.Starg_S, 2),
            });

            newInstructions[newInstructions.Count - 1].labels.Add(ret);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}