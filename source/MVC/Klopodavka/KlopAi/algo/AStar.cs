using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KlopAi.algo
{
   /// <summary>
   /// A-Star pathfinding algorithm implementation.
   /// </summary>
   public class AStar
   {
      #region Fields and Constants

      private static readonly int[] Dx = new[] {-1, -1, -1, 1, 1, 1, 0, 0};
      private static readonly int[] Dy = new[] {-1, 0, 1, -1, 0, 1, -1, 1};

      /// <summary>
      /// List of closed nodes - these nodes does not require processing anymore.
      /// </summary>
      private readonly Hashtable closedNodes = new Hashtable();

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

            //for each successor n' of n
            foreach (Node nextNode in GetNeighborNodes(currentNode, getNodeByXy))
            {
               if (nextNode == null) continue;

               //newg = n.g + cost(n,n')
               var newg = currentNode.Gdist + nextNode.Cost;

               //if n' is in openNodes or closedNodes, and n'.g <= newg {	skip }
               //TODO: Think about closedNodes.Contains(nextNode)
               if (((openNodes.Contains(nextNode) || closedNodes.Contains(nextNode)) && (nextNode.Gdist <= newg))) continue;

               //n'.Parent = n
               nextNode.Parent = currentNode;
               //n'.g = newg
               nextNode.Gdist = newg;
               //n'.h = GoalDistEstimate( n' )
               nextNode.Hdist = getDistance(nextNode, finishNode);
               //n'.f = n'.g + n'.h
               //OK
               //if n' is in closedNodes	remove it from closedNodes
               //nn.visited=false;
               if (closedNodes.Contains(nextNode)) closedNodes.Remove(nextNode);
               //if n' is not yet in openNodes  push n' on openNodes
               if (!(openNodes.Contains(nextNode))) openNodes.Add(nextNode);
            }

            closedNodes.Add(currentNode, null);
         }

         return null; //	return failure if no path found			
      }

      #endregion

      #region Private and protected methods

      private static IEnumerable<Node> GetNeighborNodes(Node node, Func<int, int, Node> getNodeByXy)
      {
         return Dx.Select((t, i) => getNodeByXy(node.X + t, node.Y + Dy[i]));
      }

      #endregion
   }
}