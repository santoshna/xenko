// Copyright (c) 2011-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using SiliconStudio.Core;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;

namespace FirstPersonShooter.Trigger
{
    [DataContract("TriggerEvent")]
    public class TriggerEvent
    {
        [DataMember(10)]
        [Display("Name")]
        [InlineProperty]
        public string Name { get; set; }

        [DataMember(20)]
        [Display("Source")]
        public Prefab SourcePrefab { get; set; }

        [DataMember(30)]
        public bool FollowEntity { get; set; }

        [DataMember(40)]
        [Display("Duration")]
        public float Duration { get; set; } = 3f;

        [DataMemberIgnore]
        public Matrix LocalMatrix = Matrix.Identity;

        [DataMember(110)]
        [Display("Local translation")]
        public Vector3 Position { get { return translation; } set { translation = value; UpdateMatrix(); } }

        [DataMember(120)]
        [Display("Local rotation")]
        public Quaternion Rotation { get { return rotation; } set { rotation = value; UpdateMatrix(); } }

        [DataMember(130)]
        [Display("Local scale")]
        public Vector3 Scale { get { return scaling; } set { scaling = value; UpdateMatrix(); } }

        private Vector3 translation = Vector3.Zero;
        private Vector3 scaling = Vector3.One;
        private Quaternion rotation = Quaternion.Identity;

        private void UpdateMatrix()
        {
            Matrix.Transformation(ref scaling, ref rotation, ref translation, out LocalMatrix);
        }
    }
}
