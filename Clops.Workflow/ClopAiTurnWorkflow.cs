using System;
using System.Workflow.Activities;

namespace Clops.Workflow
{
    public sealed partial class ClopAiTurnWorkflow : SequentialWorkflowActivity
	{
        public ClopAiTurnWorkflow()
		{
			InitializeComponent();
		}

        private void Init_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void BuildBattleArray_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void EvaluateCells_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void IsMyTurn(object sender, ConditionalEventArgs e)
        {
            e.Result = false; //TODO
        }

        private void DoDefend(object sender, ConditionalEventArgs e)
        {
            e.Result = false; //TODO: return DOoDefence
        }

        private void DoDefending_ExecuteCode(object sender, EventArgs e)
        {
            //Do nothing. DoDefend does all in while loop
        }

        private void FindBestPath_ExecuteCode(object sender, EventArgs e)
        {
            //CPU.FindPath
        }

        private void DoNothing_ExecuteCode(object sender, EventArgs e)
        {
            //Do nothing
        }

        private void IsFindPath(object sender, ConditionalEventArgs e)
        {
            e.Result = false; //CPU.FindPath
        }

        private void DoKillEnemy(object sender, EventArgs e)
        {

        }
	}

}
