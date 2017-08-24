using System;
using System.Threading;
using System.Threading.Tasks;

namespace G.Util
{
	public class TaskTimer
	{
		private Func<Task> function;
		private Action action;
		private TimeSpan dueTime = TimeSpan.Zero;
		private TimeSpan period = TimeSpan.MaxValue;
		private CancellationTokenSource cancellation;

		public TaskTimer(Func<Task> function, int dueTime, int period)
		{
			this.function = function;
			this.dueTime = TimeSpan.FromMilliseconds(dueTime);
			this.period = TimeSpan.FromMilliseconds(period);
		}

		public TaskTimer(Func<Task> function, TimeSpan dueTime, TimeSpan period)
		{
			this.function = function;
			this.dueTime = dueTime;
			this.period = period;
		}

		public TaskTimer(Action action, int dueTime, int period)
		{
			this.action = action;
			this.dueTime = TimeSpan.FromMilliseconds(dueTime);
			this.period = TimeSpan.FromMilliseconds(period);
		}

		public TaskTimer(Action action, TimeSpan dueTime, TimeSpan period)
		{
			this.action = action;
			this.dueTime = dueTime;
			this.period = period;
		}

		public void Start()
		{
			Stop();
			cancellation = new CancellationTokenSource();
			Run();
		}

		public void Start(int cancellationDelay)
		{
			Stop();
			cancellation = new CancellationTokenSource(cancellationDelay);
			Run();
		}

		public void Start(TimeSpan cancellationDelay)
		{
			Stop();
			cancellation = new CancellationTokenSource(cancellationDelay);
			Run();
		}

		public void Stop()
		{
			if (cancellation != null)
				cancellation.Cancel();
		}

		private void Run()
		{
			if (function != null)
				Task.Run(async () => await RunAsync(function, dueTime, period, cancellation.Token));
			else
				Task.Run(() => RunAsync(action, dueTime, period, cancellation.Token));
		}

		public static async Task RunAsync(Func<Task> func, TimeSpan dueTime, TimeSpan period, CancellationToken cancellationToken)
		{
			try
			{
				await Task.Delay(dueTime, cancellationToken);
				while (!cancellationToken.IsCancellationRequested)
				{
					await func();
					await Task.Delay(period, cancellationToken);
				}
			}
			catch (TaskCanceledException) {}
		}

		public static async Task RunAsync(Func<Task> function, int dueTime, int period, CancellationToken cancellationToken)
		{
			await RunAsync(function, TimeSpan.FromMilliseconds(dueTime), TimeSpan.FromMilliseconds(period), cancellationToken);
		}

		public static async Task RunAsync(Action action, TimeSpan dueTime, TimeSpan period, CancellationToken cancellationToken)
		{
			try
			{
				await Task.Delay(dueTime, cancellationToken);
				while (!cancellationToken.IsCancellationRequested)
				{
					await Task.Run(() => action());
					await Task.Delay(period, cancellationToken);
				}
			}
			catch (TaskCanceledException) {}
		}

		public static async Task RunAsync(Action action, int dueTime, int period, CancellationToken cancellationToken)
		{
			await RunAsync(action, TimeSpan.FromMilliseconds(dueTime), TimeSpan.FromMilliseconds(period), cancellationToken);
		}
	}
}
