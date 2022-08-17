namespace ME.ECSEditor.Transform {

    using ME.ECS;

    [UnityEditor.InitializeOnLoadAttribute]
    public static class DefinesGenerator {

        static DefinesGenerator() {
            
            InitializerEditor.buildConfiguration += (configuration) => {
                
                configuration.defines.Add(new ME.ECS.InitializerBase.Configuration.Define() {
                    enabled = true,
                    name = "TRANSFORM_HIERARCHY_SUPPORT",
                });
                return configuration;

            };

        }
    
    }

}