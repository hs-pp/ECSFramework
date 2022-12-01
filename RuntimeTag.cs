namespace EcsFramework
{
    public struct RuntimeTag
    {
        private string m_name;
        public string Name => m_name;
        
        public RuntimeTag(string tagName)
        {
            m_name = $"RUNTIMETAG_{tagName}";
        }
    }
}