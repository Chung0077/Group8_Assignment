using System.Collections;
using System.Collections.Generic;
using AISystem.Civil.CivilAISystem.V2;
using AISystem.Common.Weighting;
using UnityEngine;

namespace AISystem.Civil.Iterators.NodeSystem
{
    public class Weighted : Iterator
    {
        [SerializeField]
        private NodeConnection[] nodes;

        [SerializeField]
        int position = -1;


        public void AddCollection(NodeConnection[] nodesList)
        {
            nodes = Aggregator.GetInstance().OrderLocalWeightingGroup(nodesList);
        }

        public int Key()
        {
            return position;
        }

        public int GetLength()
        {
            return nodes.Length;
        }

        public NodeConnection Current()
        {
            if (position > nodes.Length - 1)
            {
                return null;
            }

            return nodes[position];
        }

        public NodeConnection MoveBack(AIDataBoard databoard)
        {
            Controller controller = databoard.GetActiveController();

            position--;

            if (position > 0)
            {
                return null;
            }

            if (Common.Check.CheckAgainstRequirements(nodes[position], controller, databoard))
            {
                return nodes[position];
            }

            return null;
        }

        public NodeConnection MoveNext(AIDataBoard databoard)
        {
            Controller controller = databoard.GetActiveController();

            position++;

            if (position > nodes.Length - 1)
            {
                return null;
            }

            if (Common.Check.CheckAgainstRequirements(nodes[position], controller, databoard))
            {
                return nodes[position];
            }

            return null;
        }

        public bool HasNext()
        {
            return position < nodes.Length - 1;
        }

        public void Reset()
        {
            position = 0;
        }
    }
}