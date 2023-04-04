using System;
using AISystem.Civil.Iterators;
using AISystem.Civil.Objects.V2;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace AISystem.Flowchart.JobSystem
{
    public class MethodNode : Node
    {
        TaskMethod method;
        float width = 300;
        float height = 215;

        public MethodNode(Vector2 mousePosition) : base(mousePosition, "Method", out Guid entry_id)
        {
            method = new TaskMethod("New Method", "testing", null);
            Setup(mousePosition);
        }

        public MethodNode(Vector2 mousePosition, TaskMethod methodRef) : base(mousePosition, "Method", out Guid entry_id)
        {
            method = methodRef;
            Setup(mousePosition);
        }

        public void Setup(Vector2 mousePosition)
        {
            node = new Rect(mousePosition.x, mousePosition.y, width, height);
            itemId = Guid.Parse(method.id);
            orderWidgetZone = new Rect(3, 140, width - 8, height - 25);
            orderWidgetDisableScale = new Vector2(0.86f, 0.86f);

            SetupLocalWeightingState();
        }

        public override void DisplayData()
        {
            EditorGUILayout.TextField("", method.id.ToString());
            method.name = EditorGUILayout.TextField("Name", method.name);
            method.desc = EditorGUILayout.TextField("Desc", method.desc);

            DisplayWeighting(method);
        }

        public override string getName()
        {
            return method.name;
        }

        public override NODE_ITERATOR GetIterator()
        {
            return method.iterator;
        }

        public override void SetIterator(NODE_ITERATOR iterator)
        {
            method.iterator = iterator;
        }

        public override BaseNode GetNode()
        {
            return method;
        }

        public override bool CheckConnectionAllowed(string childType)
        {
            if (childType == "ActionNode")
            {
                return true;
            }

            return false;
        }

        public TaskMethod GetMethod()
        {
            return method;
        }
    }
}

#endif