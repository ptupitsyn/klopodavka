using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
      private readonly Hashtable closedNodes = new Hashtable();  // TODO: Can we replace this with IsVisited for optimization?

      /// <summary>
      /// List of open nodes - nodes that require processing.
      /// </summary>
      private readonly PriorityQueue openNodes = new PriorityQueue();

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
         //Reset
         openNodes.Clear();
         closedNodes.Clear();

         startNode.Gdist = 0;
         startNode.Hdist = getDistance(startNode, finishNode);
         startNode.Parent = null;
         openNodes.Add(startNode);

         while (openNodes.Count > 0)
         {
            var currentNode = openNodes.Pop();

            if (currentNode.Equals(finishNode)) // if n is a goal node
            {
               return currentNode; // Return constructed path
            }

            foreach (Node nextNode in currentNode.GetNeighborNodes(getNodeByXy))
            {
               if (nextNode == null) continue;

               var newg = currentNode.Gdist + nextNode.Cost;

               //if n' is in openNodes or closedNodes, and n'.g <= newg {	skip }
               if (closedNodes.Contains(nextNode)) continue;  // TODO: think..
               if (openNodes.Contains(nextNode) && nextNode.Gdist <= newg) continue;

               nextNode.Parent = currentNode;
               nextNode.Gdist = newg;
               nextNode.Hdist = getDistance(nextNode, finishNode);
               //if (closedNodes.Contains(nextNode)) closedNodes.Remove(nextNode);
               if (!(openNodes.Contains(nextNode))) openNodes.Add(nextNode);
            }

            closedNodes.Add(currentNode, null); 
         }

         Debug.Assert(false, "Path has not been found!");
         return null; //	return failure if no path found			
      }

      #endregion

   }
}