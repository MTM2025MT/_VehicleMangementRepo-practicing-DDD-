using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Domain.SeedWork
{
    public abstract class Entity<T>: DomainEntity
        where T : notnull
    {
        int? _requestedHashCode;
        T _Id;
        private List<INotification> _domainEvents;
        public virtual T Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;
            }
        }
        public List<INotification> DomainEvents => _domainEvents;
        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }
        public void RemoveDomainEvent(INotification eventItem)
        {
            if (_domainEvents is null) return;
            _domainEvents.Remove(eventItem);
        }
        public void ClearDomainEvents()
        {
            if(_domainEvents is null) return; 
            _domainEvents.Clear();
        }
        public bool IsTransient()
        {
            return this.Id == null || this.Id.Equals(default(T));
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<T>))
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;
            Entity<T> item = (Entity<T>)obj;
            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return EqualityComparer<T>.Default.Equals(item.Id, this.Id); ;
        }
        public override int GetHashCode()
        {
            if (!IsTransient())
               
        {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;
                // XOR for random distribution. See:
                // https://learn.microsoft.com/archive/blogs/ericlippert/guidelines-and-rulesfor-gethashcode
                return _requestedHashCode.Value;
            }
 else
                return base.GetHashCode();
        }
        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null));
            else
                return left.Equals(right);
        }
        public static bool operator !=(Entity<T> left, Entity<T> right)
        {
            return !(left == right);
        }
    }

}
