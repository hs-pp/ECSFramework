using System;
using Unity.Collections.LowLevel.Unsafe;

namespace EcsFramework
{
    /// <summary>
    /// https://www.flecs.dev/explorer/?remote=true
    /// </summary>
    public static unsafe class RestApi
    {
        // found in rest.h
        public struct RestApiComponent {
            UInt16 port;        /* Port of server (optional, default = 27750) */
            char *ipaddr;         /* Interface address (optional, default = 0.0.0.0) */
            void *impl;
        }
        
        public static void EnableRest(World world)
        {
            flecs.ecs_add_id(world, world.WorldEntity, (ulong)BuiltInTag.EcsRest);

            var structSize = (ulong)UnsafeUtility.SizeOf<RestApiComponent>();
            RestApiComponent rest = new RestApiComponent();
            var pointer = UnsafeUtility.AddressOf(ref rest);
            flecs.ecs_set_id(world, world.WorldEntity, (ulong)BuiltInTag.EcsRest, structSize, pointer);
        }
    }
}