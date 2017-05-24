// Copyright (c) 2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.

using System;
using System.Collections.Generic;
using SiliconStudio.Core;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.Reflection;
using SiliconStudio.Xenko.Data;
using SiliconStudio.Xenko.Physics;

namespace SiliconStudio.Xenko.Navigation
{
    /// <summary>
    /// Settings for the dynamic navigation mesh builder (<see cref="DynamicNavigationMeshSystem"/>)
    /// </summary>
    [DataContract]
    [Display("Navigation")]
    [ObjectFactory(typeof(NavigationSettingsFactory))]
    [CategoryOrder(0, "Dynamic navigation mesh", Expand = ExpandRule.Never)]
    public class NavigationSettings : Configuration
    {
        /// <summary>
        /// If set to <c>true</c>, navigation meshes will be built at runtime. This allows for scene streaming and dynamic obstacles
        /// </summary>
        /// <userdoc>
        /// Enable dynamic navigation on navigation components that have no assigned navigation mesh
        /// </userdoc>
        [DataMember(0)]
        [Display("Enabled", "Dynamic navigation mesh")]
        public bool EnableDynamicNavigationMesh { get; set; }

        /// <summary>
        /// Collision filter that indicates which colliders are used in navmesh generation
        /// </summary>
        /// <userdoc>
        /// Set which collision groups dynamically-generated navigation meshes use
        /// </userdoc>
        [DataMember(10)]
        [Display(category: "Dynamic navigation mesh")]
        public CollisionFilterGroupFlags IncludedCollisionGroups { get; set; } = CollisionFilterGroupFlags.AllFilter;

        /// <summary>
        /// Build settings used by Recast
        /// </summary>
        /// <userdoc>
        /// Advanced settings for dynamically-generated navigation meshes
        /// </userdoc>
        [DataMember(20)]
        [Display(category: "Dynamic navigation mesh")]
        public NavigationMeshBuildSettings BuildSettings { get; set; }

        /// <summary>
        /// Settings for agents used with the dynamic navigation mesh
        /// </summary>
        /// <userdoc>
        /// The groups that use the dynamic navigation mesh
        /// </userdoc>
        [DataMember(30)]
        public List<NavigationMeshGroup> Groups = new List<NavigationMeshGroup>();
    }

    public class NavigationSettingsFactory : IObjectFactory
    {
        public object New(Type type)
        {
            // Initialize build settings
            return new NavigationSettings
            {
                EnableDynamicNavigationMesh = false,
                BuildSettings = ObjectFactoryRegistry.NewInstance<NavigationMeshBuildSettings>(),
                IncludedCollisionGroups = CollisionFilterGroupFlags.AllFilter,
                Groups = new List<NavigationMeshGroup>
                {
                    ObjectFactoryRegistry.NewInstance<NavigationMeshGroup>()
                }
            };
        }
    }
}
