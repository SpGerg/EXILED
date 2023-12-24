// -----------------------------------------------------------------------
// <copyright file="SpawnReason.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Enums
{
    /// <summary>
    /// Possible spawn reasons.
    /// </summary>
    public enum SpawnReason : byte // TOTO: Remove this file and use Basegame
    {
        /// <summary>
        /// No reason specified.
        /// </summary>
        None,

        /// <summary>
        /// The round has just started.
        /// </summary>
        RoundStart,

        /// <summary>
        /// The player joined and the round recently started.
        /// </summary>
        LateJoin,

        /// <summary>
        /// The player was dead and is respawning.
        /// </summary>
        Respawn,

        /// <summary>
        /// The player has died.
        /// </summary>
        Died,

        /// <summary>
        /// The player has escaped.
        /// </summary>
        Escaped,

        /// <summary>
        /// The player was revived by SCP-049.
        /// </summary>
        Revived,

        /// <summary>
        /// The player's role was changed by an admin command or plugin.
        /// </summary>
        ForceClass,

        /// <summary>
        /// The player will be destroyed.
        /// </summary>
        Destroyed,

        /// <summary>
        /// The player was spawned due to the usage of an item.
        /// </summary>
        ItemUsage,

        /// <summary>
        /// The player was revived by SCP-1507.
        /// </summary>
        Vocalize,
    }
}
