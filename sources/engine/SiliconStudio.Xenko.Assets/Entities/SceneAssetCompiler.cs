﻿// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.

using System.Threading.Tasks;
using SiliconStudio.Assets;
using SiliconStudio.Assets.Compiler;
using SiliconStudio.BuildEngine;
using SiliconStudio.Core.Serialization.Contents;
using SiliconStudio.Xenko.Engine;

namespace SiliconStudio.Xenko.Assets.Entities
{
    [AssetCompiler(typeof(SceneAsset), typeof(AssetCompilationContext))]
    public class SceneAssetCompiler : EntityHierarchyCompilerBase<SceneAsset>
    {
        protected override AssetCommand<SceneAsset> Create(string url, SceneAsset assetParameters, Package package)
        {
            return new SceneCommand(url, assetParameters, package);
        }

        private class SceneCommand : AssetCommand<SceneAsset>
        {
            public SceneCommand(string url, SceneAsset parameters, IAssetFinder assetFinder) : base(url, parameters, assetFinder)
            {
            }

            protected override Task<ResultStatus> DoCommandOverride(ICommandContext commandContext)
            {
                var assetManager = new ContentManager();

                var scene = new Scene
                {
                    Parent = Parameters.Parent,
                    Offset = Parameters.Offset
                };

                foreach (var rootEntity in Parameters.Hierarchy.RootParts)
                {
                    scene.Entities.Add(rootEntity);
                }
                assetManager.Save(Url, scene);

                return Task.FromResult(ResultStatus.Successful);
            }

            public override string ToString()
            {
                return $"Scene command for asset '{Url}'.";
            }
        }
    }
}
