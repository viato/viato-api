using Microsoft.EntityFrameworkCore;
using System;

namespace Viato.Api.UnitTests.Context
{
    public class ContextTestBase : IDisposable
    {
        protected readonly ViatoContext Context;

        public ContextTestBase()
        {
            var options = new DbContextOptionsBuilder<ViatoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            Context = new ViatoContext(options);
        }

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Context.Database.EnsureDeleted();
                    Context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
