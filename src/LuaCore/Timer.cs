using System;

namespace LuaCore
{
	public class Timer
	{
		public delegate void OnTimeUpHandler(int timerSequence);

		private Timer.OnTimeUpHandler m_timeUpHandler;

		private int m_loop = 1;

		private int m_totalTime;

		private int m_currentTime;

		private bool m_isFinished;

		private bool m_isRunning;

		private int m_sequence;

		public int CurrentTime
		{
			get
			{
				return this.m_currentTime;
			}
		}

		public Timer(int time, int loop, Timer.OnTimeUpHandler timeUpHandler, int sequence)
		{
			if (loop == 0)
			{
				loop = -1;
			}
			this.m_totalTime = time;
			this.m_loop = loop;
			this.m_timeUpHandler = timeUpHandler;
			this.m_sequence = sequence;
			this.m_currentTime = 0;
			this.m_isRunning = true;
			this.m_isFinished = false;
		}

		public void Update(int deltaTime)
		{
			if (this.m_isFinished || !this.m_isRunning)
			{
				return;
			}
			if (this.m_loop == 0)
			{
				this.m_isFinished = true;
			}
			else
			{
				this.m_currentTime += deltaTime;
				if (this.m_currentTime >= this.m_totalTime)
				{
					if (this.m_timeUpHandler != null)
					{
						this.m_timeUpHandler(this.m_sequence);
					}
					this.m_currentTime = 0;
					this.m_loop--;
				}
			}
		}

		public bool IsFinished()
		{
			return this.m_isFinished;
		}

		public void Pause()
		{
			this.m_isRunning = false;
		}

		public void Resume()
		{
			this.m_isRunning = true;
		}

		public void Reset()
		{
			this.m_currentTime = 0;
		}

		public bool IsSequenceMatched(int sequence)
		{
			return this.m_sequence == sequence;
		}

		public bool IsDelegateMatched(Timer.OnTimeUpHandler timeUpHandler)
		{
			return this.m_timeUpHandler == timeUpHandler;
		}
	}
}
