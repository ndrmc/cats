using System;
using System.Data.Entity;
using Cats.Data.Repository;
using Cats.Models.Shared.DashBoardModels;

namespace Cats.Data.UnitWork
{
    public interface IUnitOfWork : IDisposable
    {
        // TODO: Add properties to be implemented by UnitOfWork class for each repository

        Database Database { get; }
        IGenericRepository<DashboardDataEntry> DashboardDataEntryRepository { get; }
        void Save();

    }
}
