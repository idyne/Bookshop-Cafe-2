using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    public static class GlobalEventDispatcher
    {
        private static Dictionary<string, List<int>> records = new Dictionary<string, List<int>>();
        private static int count = 0;
        private static Dictionary<int, Action> actions = new Dictionary<int, Action>();
        private static Dictionary<int, string> actionSignalRelations = new Dictionary<int, string>();

        public static int Register(string signal, Action action)
        {
            int id = count++;
            if (!records.ContainsKey(signal)) records.Add(signal, new List<int>());
            records[signal].Add(id);
            actions.Add(id, action);
            actionSignalRelations.Add(id, signal);
            return id;
        }

        public static void Emit(string signal)
        {
            if (!records.ContainsKey(signal)) return;
            List<int> actionIDs = records[signal];
            for (int i = 0; i < actionIDs.Count; i++)
                actions[actionIDs[i]]();
        }

        public static void Remove(int id)
        {
            if (actions.ContainsKey(id)) actions.Remove(id);
            if (actionSignalRelations.ContainsKey(id))
            {
                string signal = actionSignalRelations[id];
                actionSignalRelations.Remove(id);
                records[signal].Remove(id);
            }
        }

        public static void Clear()
        {
            records.Clear();
            actions.Clear();
            actionSignalRelations.Clear();
        }

    }

}
