using NUnit.Framework;

namespace EcsFramework.Tests
{
    public class EcsUnitTests
    {
        protected World m_world;
        
        [SetUp]
        public void Setup()
        {
            m_world = new World(false);
        }

        [TearDown]
        public void TearDown()
        {
            m_world.Dispose();
        }
    }
}