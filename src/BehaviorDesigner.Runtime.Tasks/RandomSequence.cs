using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=31"), TaskDescription("Similar to the sequence task, the random sequence task will return success as soon as every child task returns success.  The difference is that the random sequence class will run its children in a random order. The sequence task is deterministic in that it will always run the tasks from left to right within the tree. The random sequence task shuffles the child tasks up and then begins execution in a random order. Other than that the random sequence class is the same as the sequence class. It will stop running tasks as soon as a single task ends in failure. On a task failure it will stop executing all of the child tasks and return failure. If no child returns failure then it will return success."), TaskIcon("{SkinColor}RandomSequenceIcon.png")]
	public class RandomSequence : Composite
	{
		[Tooltip("Seed the random number generator to make things easier to debug")]
		public int seed;

		[Tooltip("Do we want to use the seed?")]
		public bool useSeed;

		private List<int> childIndexList = new List<int>();

		private Stack<int> childrenExecutionOrder = new Stack<int>();

		private TaskStatus executionStatus;

		public override void OnAwake()
		{
			if (this.useSeed)
			{
				UnityEngine.Random.seed = this.seed;
			}
			this.childIndexList.Clear();
			for (int i = 0; i < this.children.Count; i++)
			{
				this.childIndexList.Add(i);
			}
		}

		public override void OnStart()
		{
			this.ShuffleChilden();
		}

		public override int CurrentChildIndex()
		{
			return this.childrenExecutionOrder.Peek();
		}

		public override bool CanExecute()
		{
			return this.childrenExecutionOrder.Count > 0 && this.executionStatus != TaskStatus.Failure;
		}

		public override void OnChildExecuted(TaskStatus childStatus)
		{
			if (this.childrenExecutionOrder.Count > 0)
			{
				this.childrenExecutionOrder.Pop();
			}
			this.executionStatus = childStatus;
		}

		public override void OnConditionalAbort(int childIndex)
		{
			this.childrenExecutionOrder.Clear();
			this.executionStatus = TaskStatus.Inactive;
			this.ShuffleChilden();
		}

		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.childrenExecutionOrder.Clear();
		}

		public override void OnReset()
		{
			this.seed = 0;
			this.useSeed = false;
		}

		private void ShuffleChilden()
		{
			for (int i = this.childIndexList.Count; i > 0; i--)
			{
				int index = UnityEngine.Random.Range(0, i);
				int num = this.childIndexList[index];
				this.childrenExecutionOrder.Push(num);
				this.childIndexList[index] = this.childIndexList[i - 1];
				this.childIndexList[i - 1] = num;
			}
		}
	}
}
