using Microsoft.EntityFrameworkCore;
using System;


namespace Repository
{
    public class StateHelper
    {
        public static EntityState ConvertState(ObjectState state)
        {
            return state switch
            {
                ObjectState.Added => EntityState.Added,
                ObjectState.Modified => EntityState.Modified,
                ObjectState.Deleted => EntityState.Deleted,
                ObjectState.Unchanged => EntityState.Unchanged,
                _ => EntityState.Unchanged,
            };
        }

        public static ObjectState ConvertState(EntityState state)
        {
            return state switch
            {
                EntityState.Detached => ObjectState.Unchanged,
                EntityState.Unchanged => ObjectState.Unchanged,
                EntityState.Added => ObjectState.Added,
                EntityState.Deleted => ObjectState.Deleted,
                EntityState.Modified => ObjectState.Modified,
                _ => throw new ArgumentOutOfRangeException("state"),
            };
        }
    }
}