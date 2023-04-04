using System;
using UnityEngine;
#if UNITY_EDITOR
using AISystem.Flowchart;
#endif
using AISystem.Civil;

namespace AISystem
{
    [Serializable]
    public class RequirementData 
    {
        static protected GUIStyle displayStyle;
        static protected GUIStyle buttonStyle;
        protected Vector2 size = new Vector2(150, 100);
        [SerializeField] protected string connectedNodeID = null;

        public RequirementData()
        {

        }

        public RequirementData GetRequirments()
        {
            return this;
        }

#if UNITY_EDITOR
        public static void SetupStyle(GUIStyle newDisplayStyle, GUIStyle newButtonStyle)
        {
            displayStyle = newDisplayStyle;
            buttonStyle = newButtonStyle;
        }

        public string getConnectedNodeID()
        {
            if(connectedNodeID == null)
            {
                return "";
            }

            return connectedNodeID;
        }

        public void setConnectedNodeID(string id)
        {
            connectedNodeID = id;
        }

        public virtual void Display(AIFlowchart flowchart, Node currentNode) { }

        public virtual void SetToDefaultWindowSize() { }

        public Vector2 GetWindowSize()
        {
            return size;
        }
#endif
        public virtual bool RequirementsMet(Controller currentAI, AIDataBoard dataBoard) { return false; }


    }
}
