// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.

using System;
using SiliconStudio.Assets;
using SiliconStudio.Assets.Compiler;
using SiliconStudio.Core;
using SiliconStudio.Xenko.Graphics;

namespace SiliconStudio.Xenko.Assets
{
    public static class AssetCompilerContextExtensions
    {
        private static readonly PropertyKey<GameSettingsAsset> GameSettingsAssetKey = new PropertyKey<GameSettingsAsset>("GameSettingsAsset", typeof(AssetCompilerContextExtensions));

        public static GameSettingsAsset GetGameSettingsAsset(this AssetCompilerContext context)
        {
            return context.Properties.Get(GameSettingsAssetKey);
        }

        public static ColorSpace GetColorSpace(this AssetCompilerContext context)
        {
            var settings = context.GetGameSettingsAsset().GetOrCreate<RenderingSettings>(context.Platform);
            return settings.ColorSpace;
        }

        public static void SetGameSettingsAsset(this AssetCompilerContext context, GameSettingsAsset gameSettingsAsset)
        {
            context.Properties.Set(GameSettingsAssetKey, gameSettingsAsset);
        }

        public static GraphicsPlatform GetGraphicsPlatform(this AssetCompilerContext context, Package package)
        {
            // If we have a command line override, use it first
            string graphicsApi;
            if (context.OptionProperties.TryGetValue("SiliconStudioXenkoGraphicsApi", out graphicsApi))
                return (GraphicsPlatform)Enum.Parse(typeof(GraphicsPlatform), graphicsApi);

            // Ohterwise, use game settings, or default as fallback
            var settings = package.GetGameSettingsAsset();
            return settings == null ? context.Platform.GetDefaultGraphicsPlatform() : RenderingSettings.GetGraphicsPlatform(context.Platform, settings.GetOrCreate<RenderingSettings>(context.Profile).PreferredGraphicsPlatform);
        }

        public static GraphicsPlatform GetDefaultGraphicsPlatform(this PlatformType platformType)
        {
            switch (platformType)
            {
                case PlatformType.Windows:
                case PlatformType.UWP:
                    return GraphicsPlatform.Direct3D11;
                case PlatformType.Android:
                case PlatformType.iOS:
                    return GraphicsPlatform.OpenGLES;
                case PlatformType.Linux:
                case PlatformType.macOS:
                    return GraphicsPlatform.OpenGL;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // TODO: Move that as extension method?
        public static CompilationMode GetCompilationMode(this AssetCompilerContext context)
        {
            var compilationMode = CompilationMode.Debug;
            switch (context.BuildConfiguration)
            {
                case "Debug":
                    compilationMode = CompilationMode.Debug;
                    break;
                case "Release":
                    compilationMode = CompilationMode.Release;
                    break;
                case "AppStore":
                    compilationMode = CompilationMode.AppStore;
                    break;
                case "Testing":
                    compilationMode = CompilationMode.Testing;
                    break;
            }
            return compilationMode;
        }
    }
}
