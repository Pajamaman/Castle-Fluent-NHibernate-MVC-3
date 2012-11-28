using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using NHibernate;
using NHibernate.Linq;

namespace CastleFluentNHibernateMvc3.Repositories
{
    public class Repository<T> : IRepository<T>
    {
        private readonly ISession Session;

        public Repository( ISession session )
        {
            Session = session;
        }

        public void BeginTransaction()
        {
            Session.BeginTransaction();
        }

        public void Commit()
        {
            Session.Transaction.Commit();
        }

        public void Rollback()
        {
            Session.Transaction.Rollback();
        }

        public IQueryable<T> GetAll()
        {
            return Session.Query<T>();
        }

        public IQueryable<T> Get( Expression<Func<T, bool>> predicate )
        {
            return GetAll().Where( predicate );
        }

        public IEnumerable<T> SaveOrUpdateAll( params T[] entities )
        {
            foreach (var entity in entities)
            {
                Session.SaveOrUpdate( entity );
            }

            return entities;
        }

        public T SaveOrUpdate( T entity )
        {
            Session.SaveOrUpdate( entity );

            return entity;
        }
    }
}