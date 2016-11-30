using System;
using System.Data.Entity;
using Cats.Data.Repository;
using Cats.Models.Shared.DashBoardModels;
using log4net;
using System.Text;
using System.Data.Entity.Validation;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace Cats.Data.UnitWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatsContext _context;

        // TODO: Add private properties for each repository
     
        private readonly ILog _log;

        public Database Database { get { return _context.Database; } }

        private IGenericRepository<DashboardDataEntry> dashboardDataEntry;
        public IGenericRepository<DashboardDataEntry> DashboardDataEntryRepository
        {
            get
            {
                return dashboardDataEntry ??
                       (dashboardDataEntry = new GenericRepository<DashboardDataEntry>(_context));
            }
        }

        public UnitOfWork()
        {
            this._context = new CatsContext();
        }

        public UnitOfWork(ILog log)
        {
            _context = new CatsContext();
            _log = log;
        }

        public void Save()
        {
            try
            {
                var newEntities = new List<DbEntityEntry>(); 

                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {                
                for (DbEntityValidationException eCurrent = e; eCurrent != null; eCurrent = (DbEntityValidationException)eCurrent.InnerException)
                {
                    foreach (var eve in eCurrent.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);

                        StringBuilder errorMsg = new StringBuilder(string.Empty);
                        string s = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);

                        errorMsg.Append(s);

                        foreach (var ve in eve.ValidationErrors)
                        {
                            errorMsg.Append(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                            _log.Error(errorMsg, eCurrent.GetBaseException());
                        }
                    }
                }

                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
