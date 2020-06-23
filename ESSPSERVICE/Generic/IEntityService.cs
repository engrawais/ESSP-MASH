using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ESSPCORE.EF;

namespace ESSPSERVICE.Generic
{
    public class ServiceMessage
    {
        public List<string> ValidationMessage{ get; set; }
    }
    public interface IEntityService<T> where T : class
    {
        List<T> GetIndex();
        List<T> GetIndexSpecific(Expression<Func<T, bool>> predicate);
        T GetEdit(int id);
        T GetDelete(int id);
        ServiceMessage PostEdit(T obj);
        ServiceMessage PostCreate(T obj);
        ServiceMessage PostDelete(T obj);
        int GetMaxID(Expression<Func<T, bool>> predicate);
    }

}
