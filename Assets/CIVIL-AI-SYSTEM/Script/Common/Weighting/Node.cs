using System;
using UnityEngine;

namespace AISystem.Common.Weighting
{
    [Serializable]
    public class Node
    {
        [SerializeField] float inTangent;
        [SerializeField] float inWeight;
        [SerializeField] float outTangent;
        [SerializeField] float time;
        [SerializeField] float value;
        [SerializeField] WeightedMode mode;

        public static Node Convert(Keyframe frame)
        {
            Node node = new Node();

            node.inTangent = frame.inTangent;
            node.inWeight = frame.inWeight;
            node.outTangent = frame.outTangent;
            node.time = frame.time;
            node.value = frame.value;
            node.mode = frame.weightedMode;

            return node;
        }

        public static Keyframe Convert(Node node)
        {
            Keyframe frame = new Keyframe();

            frame.inTangent = node.inTangent;
            frame.inWeight = node.inWeight;
            frame.outTangent = node.outTangent;
            frame.time = node.time;
            frame.value = node.value;
            frame.weightedMode = node.mode;

            return frame;
        }
    }
}