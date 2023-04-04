using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace AISystem.Flowchart
{
    public class RequirementWidget : EditorWindow
    {
        static int id = 10000;
        int actualID;

        // Style
        static GUIStyle displayStyle;
        static GUIStyle buttonStyle;
        float topX;
        float topY;
        bool visible = false;

        // Data Ref
        Node node;
        AIFlowchart flowchartRef;

        // Values
        RequirementData requirementData;

        public static void ResetComponent()
        {
            id = 10000;
        }

        public static void UpdateDisplayStyle(GUIStyle newDisplayStyle, GUIStyle newButtonStyle)
        {
            displayStyle = newDisplayStyle;
            buttonStyle = newButtonStyle;
        }

        public void initLoad(AIFlowchart flowchart, Node nodeRef, RequirementData newRequirementData)
        {
            flowchartRef = flowchart;
            node = nodeRef;
            actualID = id;
            id++;
            visible = false;

            if(newRequirementData != null)
            {
                newRequirementData.SetToDefaultWindowSize();
                requirementData = newRequirementData;
            }
        }

        public void init(AIFlowchart flowchart, Node nodeRef, string connectedNodeIDRef, Vector3 centreOfBezier, string nodeType)
        {
            flowchartRef = flowchart;
            node = nodeRef;
            actualID = id;
            id++;

            requirementData = flowchart.GetModeControl().GetRequirementData(nodeType, node);

            requirementData.setConnectedNodeID(connectedNodeIDRef);
            requirementData.SetToDefaultWindowSize();

            recalculatePosition(centreOfBezier);

            visible = true;
        }

        public void toggleVisable()
        {
            visible = !visible;
        }

        public void recalculatePosition(Vector3 centreOfBezier)
        {
            if (requirementData != null)
            {
                float canvasScale = node.getCanvasScale();

                topX = centreOfBezier.x - ((requirementData.GetWindowSize().x * canvasScale) / 2);
                topY = centreOfBezier.y + (20f * canvasScale);
            }
        }

        public int GetNodeID()
        {
            return node.GetID();
        }

        public string GetConnectedNodeID()
        {
            if(requirementData == null)
            {
                return null;
            }

            return requirementData.getConnectedNodeID();
        }

        public void Display()
        {
            if (requirementData != null && visible)
            {
                float canvasScale = node.getCanvasScale();

                Rect scaledRect = new Rect(topX, topY, requirementData.GetWindowSize().x * canvasScale, requirementData.GetWindowSize().y * canvasScale);

                GUI.Window(actualID, scaledRect, CommonDisplay, "Requirement");

                node.UpdateRequirementByUID(requirementData.getConnectedNodeID(), requirementData);
            }
        }

        public void CommonDisplay(int id)
        {
            if (GUILayout.Button("Delete"))
            {
                DeleteRequirement();
            }

            requirementData.Display(flowchartRef, node);
        }

        public void DeleteRequirement()
        {
            node.UpdateRequirementByUID(requirementData.getConnectedNodeID(), null);
            node.RemoveRequirementWindow(this);
        }

        public RequirementData GetRequirementData()
        {
            return requirementData;
        }
    }
}
#endif