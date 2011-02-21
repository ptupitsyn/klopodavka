using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KlopAi.algo
{
   /// <summary>
   /// A-Star pathfinding algorithm implementation.
   /// </summary>
   public class AStar
   {
      #region Fields and Constants

      /// <summary>
      /// List of closed nodes - these nodes does not require processing anymore.
      /// </summary>
      private readonly HashSet<Node> _closedNodes = new HashSet<Node>(); // TODO: Can we replace this with IsVisited for optimization?

      /// <summary>
      /// List of open nodes - nodes that require processing.
      /// </summary>
      private readonly PriorityQueue _openNodes = new PriorityQueue();

      #endregion

      #region Public methods

      /// <summary>
      /// Finds the shortest path between specified nodes.
      /// </summary>
      /// <param name="startNode">The start node.</param>
      /// <param name="finishNode">The finish node.</param>
      /// <param name="getDistance">Distance function - should return distance between specified nodes.</param>
      /// <param name="getNodeByXy">Node function - should return nodes by specified coordinates.</param>
      /// <returns></returns>
      public Node FindPath(Node startNode, Node finishNode, Func<Node, Node, double> getDistance, Func<int, int, Node> getNodeByXy)
      {
         return FindPath(startNode, finishNode, getDistance, getNodeByXy, false);
      }


      /// <summary>
      /// Finds the shortest path between specified nodes.
      /// </summary>
      /// <param name="startNode">The start node.</param>
      /// <param name="finishNode">The finish node.</param>
      /// <param name="getDistance">Distance function - should return distance between specified nodes.</param>
      /// <param name="getNodeByXy">Node function - should return nodes by specified coordinates.</param>
      /// <param name="inverted">if set to <c>true</c> finds longest path.</param>
      /// <returns></returns>
      public Node FindPath(Node startNode, Node finishNode, Func<Node, Node, double> getDistance, Func<int, int, Node> getNodeByXy, bool inverted)
      {
         //Reset
         _openNodes.Clear();
         _closedNodes.Clear();

         startNode.Gdist = 0;
         startNode.Hdist = getDistance(startNode, finishNode);
         startNode.Parent = null;
         _openNodes.Add(startNode);

         while (_openNodes.Count > 0)
         {
            var currentNode = inverted ? _openNodes.PopHighest() : _openNodes.Pop();

            if (currentNode.Equals(finishNode)) // if n is a goal node
            {
               return currentNode; // Return constructed path
            }

            foreach (Node nextNode in currentNode.GetNeighborNodes(getNodeByXy))
            {
               if (nextNode == null) continue;

               var newg = currentNode.Gdist + nextNode.Cost;

               if (currentNode.X != nextNode.X && currentNode.Y != nextNode.Y)
                  newg *= 0.99;   // Make diagonal moves slightly preferred

               //if n' is in openNodes or closedNodes, and n'.g <= newg {	skip }
               if (_closedNodes.Contains(nextNode)) continue; // TODO: think..
               if (_openNodes.Contains(nextNode) && nextNode.Gdist <= newg) continue;

               nextNode.Parent = currentNode;
               nextNode.Gdist = newg;
               nextNode.Hdist = getDistance(nextNode, finishNode);
               //if (closedNodes.Contains(nextNode)) closedNodes.Remove(nextNode);
               _openNodes.Add(nextNode);
            }

            _closedNodes.Add(currentNode);
         }

         Debug.Assert(false, "Path has not been found!");
         return null; //	return failure if no path found			
      }

      #endregion
   }
}