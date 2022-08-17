
namespace ME.ECS.Transform {

    public static class FilterExtensions {
        
        public static FilterBuilder Parent(this FilterBuilder filter, FilterBuilder.InnerFilterBuilderDelegate parentFilter) {

            filter.Connect<ME.ECS.Transform.Container>(parentFilter);
            return filter;

        }

    }

}