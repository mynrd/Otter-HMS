using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    public abstract class EntityBase : IObjectState
    {
        [Key]
        public virtual int Id { get; set; }

        public virtual int? LastUpdatedBy { get; set; }
        public virtual DateTime? LastUpdatedDate { get; set; }

        [NotMapped]
        public ObjectState ObjectState { get; set; } //TODO: Renamed since a possible coflict with State entity column
    }
}