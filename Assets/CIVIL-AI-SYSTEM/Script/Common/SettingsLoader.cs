using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISystem.Common.Objects
{
    public static class SettingsLoader
    {
        static Settings settings = Resources.Load<Settings>("System/Settings");
        

        public static INavMeshAgent LoadNavMeshAgent(GameObject npc)
        {
            INavMeshAgent navMeshAgent = null;

            switch (settings.navMode)
            {
                case NAV_MODE.UNITY:
                    navMeshAgent = new UnityNavMeshAgent();
                    break;
                case NAV_MODE.LOVE:
                    navMeshAgent = new INav();
                    break;
                default:
                    Debug.LogError("No Nav Mode is selected!");
                    break;
            }

            if (navMeshAgent != null)
            {
                navMeshAgent.Setup(npc, settings.speed, settings.angularSpeed, settings.acceleration, settings.stoppingDistance);
            }

            return navMeshAgent;
        }

        public static bool NeedSystemEnabled()
        {
            return settings.needSystemEnabled;
        }
    }
}
