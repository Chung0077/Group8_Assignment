using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using AISystem.Flowchart;
#endif
using AISystem.Civil;

namespace AISystem
{
    [Serializable]
    public class RequirementGeneral : RequirementData
    {
        [SerializeField] public TimeWindow timeWindow; 
        [SerializeField] public ITEMS itemNeeded;
        public WorkPlaceStateRequirment workPlaceStateRequirment;
        [SerializeField] int nodeLevel;

        // Display
        List<string> subNodes = new List<string>();
        static List<string> shortHandOp = WorkPlaceStateRequirment.getShortHandOperator();
        static Vector2 defaultSize = new Vector2(350, 150);
        int currentShortHand = 0;
        int currentTask = 0;

        public RequirementGeneral(TimeWindow newTimeWindow, ITEMS newItemNeeded, WorkPlaceStateRequirment newWorkPlaceStateRequirment, string nodeType)
        {
            timeWindow = newTimeWindow;
            itemNeeded = newItemNeeded;
            workPlaceStateRequirment = newWorkPlaceStateRequirment;
            size = defaultSize;
            SetNodeLevel(nodeType);
        }

#if UNITY_EDITOR
        public override void SetToDefaultWindowSize()
        {
            size = defaultSize;
        }

        public override void Display(AIFlowchart flowchart, Node currentNode)
        {
            GUILayout.BeginHorizontal();
                timeWindow.setEnable(GUILayout.Toggle(timeWindow.isEnabled(), ""));
                GUILayout.Label("Within Time", displayStyle);
                timeWindow.SetStartTime(EditorGUILayout.IntField(timeWindow.GetStartTime(), buttonStyle));
                timeWindow.SetEndTime(EditorGUILayout.IntField(timeWindow.GetEndTime(), buttonStyle));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Item", displayStyle);
            itemNeeded = (ITEMS)EditorGUILayout.EnumPopup(itemNeeded, buttonStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            workPlaceStateRequirment.setEnable(GUILayout.Toggle(workPlaceStateRequirment.isEnabled(), ""));
            workPlaceStateRequirment.setNumberOfWorkers(EditorGUILayout.IntField(workPlaceStateRequirment.getNumberOfWorkers()));
            currentShortHand = EditorGUILayout.Popup(currentShortHand, shortHandOp.ToArray(), buttonStyle);
            workPlaceStateRequirment.setOperator(currentShortHand);
            GUILayout.Label(" of wrks doing ", displayStyle);

            subNodes = new List<string>();
            subNodes.Add("None");
            subNodes.AddRange(workPlaceStateRequirment.GetPossibleWork(flowchart, currentNode));
            currentTask = EditorGUILayout.Popup(currentTask, subNodes.ToArray(), buttonStyle);
            GUILayout.EndHorizontal();
        }

#endif

        private void SetNodeLevel(string nodeType)
        {
            switch (nodeType)
            {
                case "JobNode":
                    nodeLevel = 0;
                    break;
                case "DutyNode":
                    nodeLevel = 1;
                    break;
                case "TaskNode":
                    nodeLevel = 2;
                    break;
                case "MethodNode":
                    nodeLevel = 3;
                    break;
                case "ActionNode":
                    nodeLevel = 4;
                    break;
            }
        }

        public override bool RequirementsMet(Controller currentAI, AIDataBoard databoard)
        {
            bool requirementMet = false;

            if(timeWindow != null)
            {
                requirementMet = timeWindow.isWithinTimeWindow();
            }

            if(workPlaceStateRequirment != null && requirementMet != false)
            {
                requirementMet = workPlaceStateRequirment.WorkPlaceStateMet((WorkController)currentAI, nodeLevel);
            }

            if (itemNeeded != ITEMS.NULL && requirementMet != false)
            {
                requirementMet = databoard.CheckInventoryForItem(itemNeeded) > 0 ? true : false;
            }

            return requirementMet;
        }
    }

}
