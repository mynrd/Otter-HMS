//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;

//namespace Repository
//{
//    public static class RepositoryExtension
//    {
//        public static IQueryable<TEntity> ExcludeDeleted<TEntity>(this ICollection<TEntity> query) where TEntity : EntityBase
//        {
//            return query.Where(x => x.deleted == false).AsQueryable();
//        }

//        public static IQueryable<TEntity> Select<TEntity>(this ICollection<TEntity> query) where TEntity : EntityBase
//        {
//            return query.Where(x => x.deleted == false).AsQueryable();
//        }

//        public static IQueryable<TEntity> Select<TEntity>(this ICollection<TEntity> query, bool includeDeleted) where TEntity : EntityBase
//        {
//            if (includeDeleted)
//                return query.AsQueryable();
//            return query.Where(x => x.deleted == false).AsQueryable();
//        }

//        public static IQueryable<TResult> Select<TEntity, TResult>(this ICollection<TEntity> query, Expression<Func<TEntity, TResult>> selector)
//            where TEntity : EntityBase
//        {
//            return query.Where(x => x.deleted == false).AsQueryable().Select(selector);
//        }

//        public static IQueryable<TResult> Select<TEntity, TResult>(this ICollection<TEntity> query, Expression<Func<TEntity, TResult>> selector, bool includeDeleted)
//          where TEntity : EntityBase
//        {
//            if (includeDeleted)
//                return query.Select(selector);
//            return query.Where(x => x.deleted == false).AsQueryable().Select(selector);
//        }
//    }
//}