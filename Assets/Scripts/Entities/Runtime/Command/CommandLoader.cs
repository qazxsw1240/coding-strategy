using System;
using System.Collections.Generic;

using UnityEngine;

namespace CodingStrategy.Entities.Runtime.Command
{
    public static class CommandLoader
    {
        private static readonly IDictionary<string, CommandProfile> CommandProfiles =
            new SortedDictionary<string, CommandProfile>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterCommandProfiles()
        {
            CommandProfile[] commandProfiles = Resources.LoadAll<CommandProfile>("Commands");
            if (commandProfiles.Length == 0)
            {
                Debug.Log("No Command Profiles found.");
                return;
            }
            foreach (CommandProfile commandProfile in commandProfiles)
            {
                CommandProfiles.Add(commandProfile.ID, commandProfile);
            }
            foreach (CommandProfile commandProfile in CommandProfiles.Values)
            {
                Debug.LogFormat("Command {0}: \"{1}\" is registered.", commandProfile.ID, commandProfile.Name);
            }
        }

        public static CommandProfile Load(string id)
        {
            if (CommandProfiles.TryGetValue(id, out CommandProfile commandProfile))
            {
                return commandProfile;
            }
            throw new CommandProfileNotFoundException(id);
        }

        public static CommandProfile Load(int id)
        {
            return Load(id.ToString());
        }
    }

    public class CommandProfileNotFoundException : Exception
    {
        public CommandProfileNotFoundException(string id) : base($"Command for ID {id} not found.")
        {
        }
    }
}
