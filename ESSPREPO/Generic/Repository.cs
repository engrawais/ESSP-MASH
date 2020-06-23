using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace ESSPREPO.Generic
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _entities;
        protected readonly IDbSet<T> _dbset;

        public Repository(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<T>();
        }

        public virtual List<T> GetAll()
        {
            return _dbset.AsNoTracking().AsEnumerable<T>().ToList();
        }

        public List<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            try
            {
                IEnumerable<T> query = _dbset.Where(predicate).AsNoTracking().AsEnumerable();
                return query.ToList();
            }
            catch (Exception ex)
            {

                return new List<T>();
            }
        }
        public virtual T Add(T entity)
        {
            try
            {
                return _dbset.Add(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw new Exception(errorMessage, dbEx);
            }
        }
        public virtual T Delete(T entity)
        {
            try
            {

                _entities.Entry(entity).State = EntityState.Deleted;
                return _dbset.Remove(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw new Exception(errorMessage, dbEx);
            }
        }
        private void Detached(T t)
        {
            _entities.Entry(t).State = EntityState.Detached;
        }
        public virtual void Edit(T entity)
        {
            try
            {
                //_entities.Entry(entity).State = EntityState.Detached;
                //_entities.Set<T>().Attach(entity);
                //_entities.Entry(entity).State = EntityState.Modified;
                foreach (DbEntityEntry dbEntityEntry in _entities.ChangeTracker.Entries())
                {

                    if (dbEntityEntry.Entity != null)
                    {
                        dbEntityEntry.State = EntityState.Detached;
                    }
                }
                _entities.Entry(entity).State = EntityState.Modified;
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw new Exception(errorMessage, dbEx);
            }
        }

        public virtual void Save()
        {
            try
            {
                _entities.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw new Exception(errorMessage, dbEx);
            }
        }

        public T GetSingle(int id)
        {
            T entity = _dbset.Find(id);
            //Detached(entity);
            return entity;
        }
        public T GetSingle(string id)
        {
            return _dbset.Find(id);
        }
        public int TotalClientRows(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate).Count();
        }

        public List<T> GetSpecificWithPaging(int skip, int take)
        {

            return _dbset.AsEnumerable<T>().Skip(skip).Take(take).ToList();
        }

        public int GetMaxID(Expression<Func<T, bool>> predicate)
        {
            return Convert.ToInt32(_dbset.Max(predicate));
        }

        public void ToggleEFValidations(bool val)
        {
            _entities.Configuration.AutoDetectChangesEnabled = val;
            _entities.Configuration.ValidateOnSaveEnabled = val;
        }
    }

}
