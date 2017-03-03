using System;

namespace aisilol
{
	public abstract class Scope : IDisposable
	{
		protected Scope()
		{
			mDisposed = false;
		}
		~Scope()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool _disposing)
		{
			if (mDisposed)
				return;

			if (_disposing)
				CloseScope();

			mDisposed = true;
		}

		protected abstract void CloseScope();

		private bool mDisposed;
	}
}