using System;
using System.Threading;
using System.Threading.Tasks;

namespace G.Util
{
	public class SemaphoreLock
	{
		private delegate _Semaphore _Lock();
		private delegate Task<_Semaphore> _LockAsync();

		private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

		private int millisecondsTimeout = 0;
		private TimeSpan timeout = TimeSpan.Zero;
		private CancellationToken cancellationToken = CancellationToken.None;
		private _Lock _lock;
		private _LockAsync _lockAsync;

		public SemaphoreLock()
		{
			_lock = _Lock1;
			_lockAsync = _Lock1Async;
		}

		public SemaphoreLock(int millisecondsTimeout)
		{
			this.millisecondsTimeout = millisecondsTimeout;
			_lock = _Lock2;
			_lockAsync = _Lock2Async;
		}

		public SemaphoreLock(TimeSpan timeout)
		{
			this.timeout = timeout;
			_lock = _Lock3;
			_lockAsync = _Lock3Async;
		}

		public SemaphoreLock(CancellationToken cancellationToken)
		{
			this.cancellationToken = cancellationToken;
			_lock = _Lock4;
			_lockAsync = _Lock4Async;
		}

		public SemaphoreLock(int millisecondsTimeout, CancellationToken cancellationToken)
		{
			this.millisecondsTimeout = millisecondsTimeout;
            this.cancellationToken = cancellationToken;
			_lock = _Lock5;
			_lockAsync = _Lock5Async;
		}

		public SemaphoreLock(TimeSpan timeout, CancellationToken cancellationToken)
		{
			this.timeout = timeout;
            this.cancellationToken = cancellationToken;
			_lock = _Lock6;
			_lockAsync = _Lock6Async;
		}

		public _Semaphore Lock()
		{
			return _lock();
		}

		public async Task<_Semaphore> LockAsync()
		{
			return await _lockAsync();
		}

		private _Semaphore _Lock1()
		{
			var semaphore = new _Semaphore(semaphoreSlim);
			semaphore.Lock();
			return semaphore;
		}

		private _Semaphore _Lock2()
		{
			var semaphore = new _Semaphore(semaphoreSlim);
			semaphore.Lock(millisecondsTimeout);
			return semaphore;
		}

		private _Semaphore _Lock3()
		{
			var semaphore = new _Semaphore(semaphoreSlim);
			semaphore.Lock(timeout);
			return semaphore;
		}

		private _Semaphore _Lock4()
		{
			var semaphore = new _Semaphore(semaphoreSlim);
			semaphore.Lock(cancellationToken);
			return semaphore;
		}

		private _Semaphore _Lock5()
		{
			var semaphore = new _Semaphore(semaphoreSlim);
			semaphore.Lock(millisecondsTimeout, cancellationToken);
			return semaphore;
		}

		private _Semaphore _Lock6()
		{
			var semaphore = new _Semaphore(semaphoreSlim);
			semaphore.Lock(timeout, cancellationToken);
			return semaphore;
		}

		private async Task<_Semaphore> _Lock1Async()
		{
			var semaphore = new _Semaphore(semaphoreSlim);
			await semaphore.LockAsync();
			return semaphore;
		}

		private async Task<_Semaphore> _Lock2Async()
		{
			var semaphore = new _Semaphore(semaphoreSlim);
			await semaphore.LockAsync(millisecondsTimeout);
			return semaphore;
		}

		private async Task<_Semaphore> _Lock3Async()
		{
			var semaphore = new _Semaphore(semaphoreSlim);
			await semaphore.LockAsync(timeout);
			return semaphore;
		}

		private async Task<_Semaphore> _Lock4Async()
		{
			var semaphore = new _Semaphore(semaphoreSlim);
			await semaphore.LockAsync(cancellationToken);
			return semaphore;
		}

		private async Task<_Semaphore> _Lock5Async()
		{
			var semaphore = new _Semaphore(semaphoreSlim);
			await semaphore.LockAsync(millisecondsTimeout, cancellationToken);
			return semaphore;
		}

		private async Task<_Semaphore> _Lock6Async()
		{
			var semaphore = new _Semaphore(semaphoreSlim);
			await semaphore.LockAsync(timeout, cancellationToken);
			return semaphore;
		}
	}

	public class _Semaphore : IDisposable
	{
		private bool disposed = false;

		private SemaphoreSlim semaphoreSlim;

		internal _Semaphore(SemaphoreSlim semaphoreSlim)
		{
            this.semaphoreSlim = semaphoreSlim;
		}

		~_Semaphore()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed) return;

			if (disposing)
			{
				if (semaphoreSlim != null) semaphoreSlim.Release();
			}

			disposed = true;
		}

		internal void Lock()
		{
			semaphoreSlim.Wait();
		}

		internal void Lock(int millisecondsTimeout)
		{
			if (false == semaphoreSlim.Wait(millisecondsTimeout))
			{
				semaphoreSlim = null;
				throw new TimeoutException("SemaphoreLock");
			}
		}

		internal void Lock(TimeSpan timeout)
		{
			if (false == semaphoreSlim.Wait(timeout))
			{
				semaphoreSlim = null;
				throw new TimeoutException("SemaphoreLock");
			}
		}

		internal void Lock(CancellationToken cancellationToken)
		{
			try
			{
				semaphoreSlim.Wait(cancellationToken);
			}
			catch (OperationCanceledException)
			{
				semaphoreSlim = null;
				throw;
			}
		}

		internal void Lock(int millisecondsTimeout, CancellationToken cancellationToken)
		{
			try
			{
				if (false == semaphoreSlim.Wait(millisecondsTimeout, cancellationToken))
				{
					semaphoreSlim = null;
					throw new TimeoutException("SemaphoreLock");
				}
			}
			catch (OperationCanceledException)
			{
				semaphoreSlim = null;
				throw;
			}
		}

		internal void Lock(TimeSpan timeout, CancellationToken cancellationToken)
		{
			try
			{
				if (false == semaphoreSlim.Wait(timeout, cancellationToken))
				{
					semaphoreSlim = null;
					throw new TimeoutException("SemaphoreLock");
				}
			}
			catch (OperationCanceledException)
			{
				semaphoreSlim = null;
				throw;
			}
		}

		internal async Task LockAsync()
		{
			await semaphoreSlim.WaitAsync();
		}

		internal async Task LockAsync(int millisecondsTimeout)
		{
			if (false == await semaphoreSlim.WaitAsync(millisecondsTimeout))
			{
				semaphoreSlim = null;
				throw new TimeoutException("SemaphoreLock");
			}
		}

		internal async Task LockAsync(TimeSpan timeout)
		{
			if (false == await semaphoreSlim.WaitAsync(timeout))
			{
				semaphoreSlim = null;
				throw new TimeoutException("SemaphoreLock");
			}
		}

		internal async Task LockAsync(CancellationToken cancellationToken)
		{
			try
			{
				await semaphoreSlim.WaitAsync(cancellationToken);
			}
			catch (OperationCanceledException)
			{
				semaphoreSlim = null;
				throw;
			}
		}

		internal async Task LockAsync(int millisecondsTimeout, CancellationToken cancellationToken)
		{
			try
			{
				if (false == await semaphoreSlim.WaitAsync(millisecondsTimeout, cancellationToken))
				{
					semaphoreSlim = null;
					throw new TimeoutException("SemaphoreLock");
				}
			}
			catch (OperationCanceledException)
			{
				semaphoreSlim = null;
				throw;
			}
		}

		internal async Task LockAsync(TimeSpan timeout, CancellationToken cancellationToken)
		{
			try
			{
				if (false == await semaphoreSlim.WaitAsync(timeout, cancellationToken))
				{
					semaphoreSlim = null;
					throw new TimeoutException("SemaphoreLock");
				}
			}
			catch (OperationCanceledException)
			{
				semaphoreSlim = null;
				throw;
			}
		}
	}
}
