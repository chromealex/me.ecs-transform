
namespace ME.ECS.Transform {

    public static class EntityCopyFromExtensions {

        public static void CopyFrom(in Entity from, in Entity to, bool copyHierarchy) {
            
            // Copy hierarchy data
            to.Remove<ME.ECS.Transform.Container>();
            if (from.TryRead(out ME.ECS.Transform.Container container) == true) {
                to.SetParent(container.entity);
            }

            if (copyHierarchy == true) {

                var nodes = from.Read<ME.ECS.Transform.Nodes>();
                if (nodes.items.isCreated == true) {
                    var e = nodes.items.GetEnumerator(in Worlds.current.GetState().allocator);
                    while (e.MoveNext() == true) {
                        var newChild = new Entity(EntityFlag.None);
                        newChild.CopyFrom(e.Current);
                        newChild.SetParent(to);
                    }

                    e.Dispose();
                }

            }
            
        }

    }

}