using System;

namespace GroceryCo.Kiosk.Model
{
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
