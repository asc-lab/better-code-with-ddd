using System;

namespace LoanApplication.TacticalDdd.DomainModel.Ddd
{
    public abstract class Entity<T>
    {
        public T Id { get; protected set; }
    }
}