﻿namespace ME.ECS.Transform {

    [ComponentGroup(typeof(TransformComponentConstants.GroupInfo))]
    [ComponentOrder(4)]
    public struct Container : IComponent, IVersioned, IFilterConnect {

        public Entity entity;

        Entity IFilterConnect.entity => this.entity;

    }

    [ComponentGroup(typeof(TransformComponentConstants.GroupInfo))]
    [ComponentOrder(5)]
    public struct Nodes : IComponent, IVersioned, IComponentDisposable<Nodes> {

        public ME.ECS.Collections.LowLevel.List<Entity> items;

        public void OnDispose(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator) {
            if (this.items.isCreated == true) this.items.Dispose(ref allocator);
        }

        public void ReplaceWith(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator, in Nodes other) {
            this.items.ReplaceWith(ref allocator, other.items);
        }
        
        public void CopyFrom(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator, in Nodes other) {
            this.items.CopyFrom(ref allocator, other.items);
        }

    }

}