using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Transform {

    public static class WorldInitializer {

        public static DisposeStatic disposeStatic = new DisposeStatic();

        private static bool initialized = false;
        
        #if UNITY_EDITOR
        [UnityEditor.InitializeOnLoad]
        private static class EditorInitializer {
            static EditorInitializer() => WorldInitializer.Initialize();
        }
        #endif

        [UnityEngine.RuntimeInitializeOnLoadMethodAttribute(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize() {
            
            if (WorldInitializer.initialized == false) {
                
                CoreComponentsInitializer.RegisterInitCallback(WorldInitializer.InitTypeId, WorldInitializer.Init, WorldInitializer.Init);
                WorldStaticCallbacks.RegisterCallbacks(WorldInitializer.EntityCopyFrom);
                WorldStaticCallbacks.RegisterCallbacks(WorldInitializer.OnEntityDestroy);
                WorldStaticCallbacks.RegisterViewCreatedCallback(WorldInitializer.OnViewCreated);
                WorldStaticCallbacks.RegisterViewDestroyCallback(WorldInitializer.OnViewDestroy);
                WorldStaticCallbacks.SetGetVersionCallback(WorldInitializer.GetEntityVersion);
                WorldInitializer.initialized = true;
                
            }
            
        }

        public class DisposeStatic {
            ~DisposeStatic() {
                CoreComponentsInitializer.UnRegisterInitCallback(WorldInitializer.InitTypeId, WorldInitializer.Init, WorldInitializer.Init);
                WorldStaticCallbacks.UnRegisterCallbacks(WorldInitializer.EntityCopyFrom);
                WorldStaticCallbacks.UnRegisterCallbacks(WorldInitializer.OnEntityDestroy);
                WorldStaticCallbacks.UnRegisterViewCreatedCallback(WorldInitializer.OnViewCreated);
                WorldStaticCallbacks.UnRegisterViewDestroyCallback(WorldInitializer.OnViewDestroy);
                WorldStaticCallbacks.UnSetGetVersionCallback(WorldInitializer.GetEntityVersion);
                WorldInitializer.initialized = false;
            }
        }

        public static uint GetEntityVersion(in Entity entity) {

            return entity.GetVersionInHierarchy();

        }

        public static void OnViewCreated(in Entity entity, ME.ECS.Views.IView view, int parentId) {

            var viewMono = view as ME.ECS.Views.Providers.MonoBehaviourView;
            if (viewMono == null) return;
            
            var parent = entity.GetParent();
            if (parent.IsEmpty() == false) {

                var viewModule = Worlds.current.GetModule<ME.ECS.Views.IViewModule>();
                var parentView = viewModule.GetViewByEntity(parent) as ME.ECS.Views.Providers.MonoBehaviourView;
                if (parentView != null) {
                    viewMono.transform.SetParent(parentView.GetParentTransform(parentId));
                }
                
            }

        }

        public static void OnViewDestroy(in Entity entity, ME.ECS.Views.IView view) {
            
            var viewMono = view as ME.ECS.Views.Providers.MonoBehaviourView;
            if (viewMono == null) return;

            viewMono.transform.SetParent(null);

        }

        public static void OnEntityDestroy(State state, in Entity entity, bool cleanUpHierarchy) {
            
            if (cleanUpHierarchy == true) ECSTransformHierarchy.OnEntityDestroy(ref state.allocator, in entity);

        }

        public static void EntityCopyFrom(World world, in Entity from, in Entity to, bool copyHierarchy) {
            
            EntityCopyFromExtensions.CopyFrom(in from, in to, copyHierarchy);
            
        }
        
        public static void InitTypeId() {
            
            TransformComponentsInitializer.InitTypeId();
            
        }

        public static void Init(State state, ref World.NoState noState) {
    
            TransformComponentsInitializer.Init(state);

        }
    
        public static void Init(in Entity entity) {

            TransformComponentsInitializer.Init(in entity);
            
        }

    }

}