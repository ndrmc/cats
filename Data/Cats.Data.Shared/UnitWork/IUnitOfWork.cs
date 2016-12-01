using System;
using System.Data.Entity;
using Cats.Models.Shared.DashBoardModels;
using Cats.Data.Shared.Repository;

namespace Cats.Data.Shared.UnitWork
{
    public interface IUnitOfWork : IDisposable
    {
        // TODO: Add properties to be implemented by UnitOfWork class for each repository

        Database Database { get; }
        IGenericRepository<DashboardDataEntry> DashboardDataEntryRepository { get; }
        void Save();

    }
}
