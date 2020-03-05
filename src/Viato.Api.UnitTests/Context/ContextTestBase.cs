using System;
using Microsoft.EntityFrameworkCore;

namespace Viato.Api.UnitTests.Context
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Calm down, it's just test.")]
    public class ContextTestBase : IDisposable
    {
        public ContextTestBase()
        {
            var options = new DbContextOptionsBuilder<ViatoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            Context = new ViatoContext(options);
        }

        protected ViatoContext Context { get; private set; }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
