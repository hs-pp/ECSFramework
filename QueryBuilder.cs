namespace EcsFramework
{
    public class QueryBuilder : BaseQueryBuilder<QueryBuilder>
    {
        public QueryBuilder(World world) : base(world) { }
        
        public Query Build()
        {
            Query query = new Query(m_filterDesc, m_world);
            ResetBuilder();
            return query;
        }
    }
}