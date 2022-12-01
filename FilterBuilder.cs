namespace EcsFramework
{
    public class FilterBuilder : BaseQueryBuilder<FilterBuilder>
    {
        public FilterBuilder(World world) : base(world) { }
        
        public Filter Build()
        {
            Filter filter = new Filter(m_filterDesc, m_world);
            ResetBuilder();
            return filter;
        }
    }
}