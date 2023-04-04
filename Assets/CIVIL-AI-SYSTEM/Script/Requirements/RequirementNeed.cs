using System;
using UnityEditor;
using UnityEngine;
using AISystem.Civil;

#if UNITY_EDITOR
using AISystem.Flowchart;
using AISystem.Menu;
#endif

namespace AISystem
{
    [Serializable]
    public class RequirementNeed : RequirementData
    {
        static Vector2 defaultSize = new Vector2(340, 170);

        public float[] band = new float[2];
        public float[] maxBand = new float[2];
        public ITEMS_TYPE itemType = ITEMS_TYPE.NULL;
        public bool onlyOwned = true;

        public RequirementNeed(float min, float max)
        {
            band[0] = min;
            band[1] = max;
            maxBand[0] = min;
            maxBand[1] = max;
        }

#if UNITY_EDITOR
        public override void SetToDefaultWindowSize()
        {
            size = defaultSize;
        }

        public override void Display(AIFlowchart flowchart, Node currentNode)
        {
            float[] bandTemp = new float[2];

            EditorGUILayout.LabelField("Range");
            bandTemp[0] = EditorGUILayout.FloatField("Min", band[0]);
            bandTemp[1] = EditorGUILayout.FloatField("Max", band[1]);

            CheckAndUpdateIfWithinBand(bandTemp);

            EditorGUILayout.MinMaxSlider("Range", ref band[0], ref band[1], maxBand[0], maxBand[1]);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Item Type", displayStyle);
            itemType = (ITEMS_TYPE)EditorGUILayout.EnumPopup(itemType, buttonStyle);
            GUILayout.EndHorizontal();


        }

        void CheckAndUpdateIfWithinBand(float[] checkList)
        {
            for (int i = 0; i < checkList.Length; i++)
            {
                if (Validators.IsWithinBand(checkList[i], maxBand))
                {
                    band[i] = checkList[i];
                }
            }
        }

#endif
        public override bool RequirementsMet(Controller currentAI, AIDataBoard databoard)
        {
            return true;
        }
    }
}
