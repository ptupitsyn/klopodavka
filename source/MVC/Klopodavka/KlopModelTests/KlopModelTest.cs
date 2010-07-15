using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KlopIfaces;
using KlopModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KlopModelTests
{
   /// <summary>
   /// Summary description for UnitTest1
   /// </summary>
   [TestClass]
   public class KlopModelTest
   {
      private TestContext testContextInstance;

      /// <summary>
      ///Gets or sets the test context which provides
      ///information about and functionality for the current test run.
      ///</summary>
      public TestContext TestContext
      {
         get
         {
            return testContextInstance;
         }
         set
         {
            testContextInstance = value;
         }
      }

      #region Additional test attributes
      //
      // You can use the following additional attributes as you write your tests:
      //
      // Use ClassInitialize to run code before running the first test in the class
      // [ClassInitialize()]
      // public static void MyClassInitialize(TestContext testContext) { }
      //
      // Use ClassCleanup to run code after all tests in a class have run
      // [ClassCleanup()]
      // public static void MyClassCleanup() { }
      //
      // Use TestInitialize to run code before running each test 
      // [TestInitialize()]
      // public void MyTestInitialize() { }
      //
      // Use TestCleanup to run code after each test has run
      // [TestCleanup()]
      // public void MyTestCleanup() { }
      //
      #endregion

      [TestMethod]
      public void CellAvailableTest()
      {
         const int width = 100;
         const int height = 100;
         var players = new List<IKlopPlayer>
                                {
                                   new KlopPlayer {BasePosX = width - 5, BasePosY = 4},
                                   new KlopPlayer {BasePosX = 4, BasePosY = height - 5},
                                };
         var model = new KlopModel.KlopModel(width, height, players);
         var turns = 0;
         var stopwatch = new Stopwatch();
         stopwatch.Start();

         while (model.RemainingKlops > 0)  // Simulate one turn
         {
            var availableCells = model.Cells.Where(c => c.Available).ToList();
            Assert.IsTrue(availableCells.Count() > 0, "No available cells during first turn!");

            if (availableCells.Any(c => c.Owner != null))
            {
               //Opponents meet each other => stop testing
               break;
            }

            foreach (IKlopCell cell in model.Cells.Where(c => c.Owner == model.CurrentPlayer).SelectMany(c => GetNeighborCells(c, model)))
            {
               // All neighbor cells must be available, if not belong to player already
               Assert.IsTrue(cell.Available || cell.Owner == model.CurrentPlayer, "Neighbor empty cell is not available for turn!");
            }
            model.MakeTurn(availableCells.Last());
            turns++;
         }
         stopwatch.Stop();
         var execTime = stopwatch.ElapsedMilliseconds/turns;

         Assert.IsTrue(execTime < 50);   // One function call should be less than 50 milliseconds
      }

      /// <summary>
      /// Gets the neighbor cells.
      /// </summary>
      /// <param name="cell">The cell.</param>
      /// <param name="model">The model.</param>
      /// <returns></returns>
      private static IEnumerable<IKlopCell> GetNeighborCells(IKlopCell cell, IKlopModel model)
      {
         for (int x = -1; x < 1; x++)
         {
            for (int y = -1; y < 1; y++)
            {
               var xx = cell.X + x;
               var yy = cell.Y + y;
               if ((x == y && x == 0) || xx < 0 || yy < 0 || xx >= model.FieldWidth || yy > model.FieldHeight) continue;
               yield return model[xx, yy];
            }
         }
      }
   }
}
