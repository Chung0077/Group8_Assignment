using System;
using UnityEditor;
using UnityEngine;
using AISystem.Civil;

#if UNITY_EDITOR
using AISystem.Flowchart;
#endif

namespace AISystem
{
    [Serializable]
    public class RequirementAction : RequirementData
    {
        static Vector2 defaultSize = new Vector2(150, 100);

        public AnimationClip customAnimation;
        public float waitTime;

        public RequirementAction(AnimationClip newCustomAnimation, float newWaitTime)
        {
            customAnimation = newCustomAnimation;
            waitTime = newWaitTime;
            size = defaultSize;
        }

#if UNITY_EDITOR
        public override void SetToDefaultWindowSize()
        {
            size = defaultSize;
        }


        public override void Display(AIFlowchart flowchart, Node currentNode)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Animation", displayStyle);
            customAnimation = (AnimationClip)EditorGUILayout.ObjectField(customAnimation, typeof(AnimationClip), true);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Wait Time", displayStyle);
            waitTime = EditorGUILayout.FloatField(waitTime);
            GUILayout.EndHorizontal();
        }
#endif
        public override bool RequirementsMet(Controller currentAI, AIDataBoard databoard)
        {
            return true;
        }
    }
}
