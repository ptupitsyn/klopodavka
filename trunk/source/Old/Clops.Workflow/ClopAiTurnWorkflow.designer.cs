using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Clops.Workflow
{
    partial class ClopAiTurnWorkflow
	{
		#region Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
		private void InitializeComponent()
		{
            this.CanModifyActivities = true;
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition3 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition4 = new System.Workflow.Activities.CodeCondition();
            this.DoKill = new System.Workflow.Activities.CodeActivity();
            this.DoNothing1 = new System.Workflow.Activities.CodeActivity();
            this.NotFindPath = new System.Workflow.Activities.IfElseBranchActivity();
            this.PathFound = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfFindPath = new System.Workflow.Activities.IfElseActivity();
            this.DoNothing = new System.Workflow.Activities.CodeActivity();
            this.NotDefending = new System.Workflow.Activities.IfElseBranchActivity();
            this.Defending = new System.Workflow.Activities.IfElseBranchActivity();
            this.DoDefending = new System.Workflow.Activities.CodeActivity();
            this.IfDefending = new System.Workflow.Activities.IfElseActivity();
            this.FindBestPath = new System.Workflow.Activities.CodeActivity();
            this.WhileDefending = new System.Workflow.Activities.WhileActivity();
            this.FightSequence = new System.Workflow.Activities.SequenceActivity();
            this.WhileMyTurn = new System.Workflow.Activities.WhileActivity();
            this.EvaluateCells = new System.Workflow.Activities.CodeActivity();
            this.BuildBattleArray = new System.Workflow.Activities.CodeActivity();
            this.Init = new System.Workflow.Activities.CodeActivity();
            // 
            // DoKill
            // 
            this.DoKill.Name = "DoKill";
            this.DoKill.ExecuteCode += new System.EventHandler(this.DoKillEnemy);
            // 
            // DoNothing1
            // 
            this.DoNothing1.Name = "DoNothing1";
            this.DoNothing1.ExecuteCode += new System.EventHandler(this.DoNothing_ExecuteCode);
            // 
            // NotFindPath
            // 
            this.NotFindPath.Activities.Add(this.DoKill);
            this.NotFindPath.Name = "NotFindPath";
            // 
            // PathFound
            // 
            this.PathFound.Activities.Add(this.DoNothing1);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.IsFindPath);
            this.PathFound.Condition = codecondition1;
            this.PathFound.Name = "PathFound";
            // 
            // IfFindPath
            // 
            this.IfFindPath.Activities.Add(this.PathFound);
            this.IfFindPath.Activities.Add(this.NotFindPath);
            this.IfFindPath.Name = "IfFindPath";
            // 
            // DoNothing
            // 
            this.DoNothing.Name = "DoNothing";
            this.DoNothing.ExecuteCode += new System.EventHandler(this.DoNothing_ExecuteCode);
            // 
            // NotDefending
            // 
            this.NotDefending.Activities.Add(this.IfFindPath);
            this.NotDefending.Name = "NotDefending";
            // 
            // Defending
            // 
            this.Defending.Activities.Add(this.DoNothing);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.DoDefend);
            this.Defending.Condition = codecondition2;
            this.Defending.Name = "Defending";
            // 
            // DoDefending
            // 
            this.DoDefending.Name = "DoDefending";
            this.DoDefending.ExecuteCode += new System.EventHandler(this.DoDefending_ExecuteCode);
            // 
            // IfDefending
            // 
            this.IfDefending.Activities.Add(this.Defending);
            this.IfDefending.Activities.Add(this.NotDefending);
            this.IfDefending.Name = "IfDefending";
            // 
            // FindBestPath
            // 
            this.FindBestPath.Name = "FindBestPath";
            this.FindBestPath.ExecuteCode += new System.EventHandler(this.FindBestPath_ExecuteCode);
            // 
            // WhileDefending
            // 
            this.WhileDefending.Activities.Add(this.DoDefending);
            codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.DoDefend);
            this.WhileDefending.Condition = codecondition3;
            this.WhileDefending.Name = "WhileDefending";
            // 
            // FightSequence
            // 
            this.FightSequence.Activities.Add(this.WhileDefending);
            this.FightSequence.Activities.Add(this.FindBestPath);
            this.FightSequence.Activities.Add(this.IfDefending);
            this.FightSequence.Name = "FightSequence";
            // 
            // WhileMyTurn
            // 
            this.WhileMyTurn.Activities.Add(this.FightSequence);
            codecondition4.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.IsMyTurn);
            this.WhileMyTurn.Condition = codecondition4;
            this.WhileMyTurn.Name = "WhileMyTurn";
            // 
            // EvaluateCells
            // 
            this.EvaluateCells.Name = "EvaluateCells";
            this.EvaluateCells.ExecuteCode += new System.EventHandler(this.EvaluateCells_ExecuteCode);
            // 
            // BuildBattleArray
            // 
            this.BuildBattleArray.Name = "BuildBattleArray";
            this.BuildBattleArray.ExecuteCode += new System.EventHandler(this.BuildBattleArray_ExecuteCode);
            // 
            // Init
            // 
            this.Init.Name = "Init";
            this.Init.ExecuteCode += new System.EventHandler(this.Init_ExecuteCode);
            // 
            // ClopAiTurnWorkflow
            // 
            this.Activities.Add(this.Init);
            this.Activities.Add(this.BuildBattleArray);
            this.Activities.Add(this.EvaluateCells);
            this.Activities.Add(this.WhileMyTurn);
            this.Name = "ClopAiTurnWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private CodeActivity BuildBattleArray;
        private CodeActivity EvaluateCells;
        private SequenceActivity FightSequence;
        private WhileActivity WhileMyTurn;
        private WhileActivity WhileDefending;
        private CodeActivity FindBestPath;
        private CodeActivity DoDefending;
        private IfElseBranchActivity NotDefending;
        private IfElseBranchActivity Defending;
        private IfElseActivity IfDefending;
        private IfElseBranchActivity NotFindPath;
        private IfElseBranchActivity PathFound;
        private IfElseActivity IfFindPath;
        private CodeActivity DoNothing;
        private CodeActivity DoNothing1;
        private CodeActivity DoKill;
        private CodeActivity Init;



























    }
}
