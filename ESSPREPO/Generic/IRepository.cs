using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ESSPREPO.Generic
{
    public interface IRepository<T> where T : class
    {

        List<T> GetAll();
        List<T> GetSpecificWithPaging(int skip,int take);

        List<T> FindBy(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        T Delete(T entity);
        void Edit(T entity);
        T GetSingle(int id);
        T GetSingle(string id);
        void Save();
        int TotalClientRows(Expression<Func<T, bool>> predicate);
        int GetMaxID(Expression<Func<T, bool>> predicate);
        void ToggleEFValidations(bool val);
    }
}
