namespace EcsFramework
{
    public interface IEcsComponent { }
    public interface IComponent : IEcsComponent { }
    public interface ITag : IEcsComponent { }
    public interface ISingletonComponent : IEcsComponent { }
    public interface IImmutableData { } // NOT inherited from IEcsComponent!
}