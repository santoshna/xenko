// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using System.Collections.Generic;
using System.IO;
using System.Text;
using SiliconStudio.Core.IO;
using SiliconStudio.Core.Yaml;
using SiliconStudio.Xenko.Rendering;
using SiliconStudio.Xenko.Shaders.Compiler;

namespace SiliconStudio.Xenko.Assets.Effect
{
    public class EffectLogStore : DictionaryStore<EffectCompileRequest, bool>
    {
        private byte[] documentMarker = Encoding.UTF8.GetBytes("---\r\n");

        public EffectLogStore(Stream stream)
            : base(stream)
        {
        }

        protected override void WriteEntry(Stream stream, KeyValuePair<EffectCompileRequest, bool> value)
        {
            stream.Write(documentMarker, 0, documentMarker.Length);
            AssetYamlSerializer.Default.Serialize(stream, value.Key);
        }

        protected override List<KeyValuePair<EffectCompileRequest, bool>> ReadEntries(Stream localStream)
        {
            var result = new List<KeyValuePair<EffectCompileRequest, bool>>();

            foreach (var effectCompileRequest in AssetYamlSerializer.Default.DeserializeMultiple<EffectCompileRequest>(localStream))
            {
                result.Add(new KeyValuePair<EffectCompileRequest, bool>(effectCompileRequest, true));
            }

            return result;
        }
    }
}
