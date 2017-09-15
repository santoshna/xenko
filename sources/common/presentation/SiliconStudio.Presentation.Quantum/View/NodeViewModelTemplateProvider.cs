// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using System;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Presentation.Quantum.ViewModels;
using SiliconStudio.Presentation.View;

namespace SiliconStudio.Presentation.Quantum.View
{
    /// <summary>
    /// A base class for implementations of <see cref="ITemplateProvider"/> that can provide templates for <see cref="SiliconStudio.Presentation.Quantum.ViewModels.NodeViewModel"/> instances.
    /// </summary>
    public abstract class NodeViewModelTemplateProvider : TemplateProviderBase
    {
        /// <inheritdoc/>
        public override bool Match(object obj)
        {
            var node = obj as NodeViewModel;
            return node != null && MatchNode(node);
        }

        /// <summary>
        /// Indicates whether this instance of <see cref="ITemplateProvider"/> can provide a template for the given <see cref="SiliconStudio.Presentation.Quantum.ViewModels.NodeViewModel"/>.
        /// </summary>
        /// <param name="node">The node to test.</param>
        /// <returns><c>true</c> if this template provider can provide a template for the given node, <c>false</c> otherwise.</returns>
        /// <remarks>This method is invoked by <see cref="Match"/>.</remarks>
        public abstract bool MatchNode(NodeViewModel node);

        /// <summary>
        /// Indicates whether the given node matches the given type, either with the <see cref="SiliconStudio.Presentation.Quantum.ViewModels.NodeViewModel.Type"/> property
        /// or the type of the <see cref="SiliconStudio.Presentation.Quantum.ViewModels.NodeViewModel.Value"/> property.
        /// </summary>
        /// <param name="node">The node to check.</param>
        /// <param name="type">The type to match.</param>
        /// <returns><c>true</c> if the node matches the given type, <c>false</c> otherwise.</returns>
        protected static bool MatchType([NotNull] NodeViewModel node, [NotNull] Type type)
        {
            return type.IsAssignableFrom(node.Type) || type.IsInstanceOfType(node.NodeValue);
        }
    }
}
