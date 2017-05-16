// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.Diagnostics;
using SiliconStudio.Core.Extensions;
using SiliconStudio.Presentation.Quantum.Presenters;
using SiliconStudio.Presentation.Services;
using SiliconStudio.Presentation.ViewModel;
using SiliconStudio.Quantum;

namespace SiliconStudio.Presentation.Quantum.ViewModels
{
    /// <summary>
    /// A view model class representing one or multiple trees of <see cref="INodePresenter"/> instances.
    /// </summary>
    public class GraphViewModel : DispatcherViewModel
    {
        public const string DefaultLoggerName = "Quantum";
        public const string HasChildPrefix = "HasChild_";
        public const string HasCommandPrefix = "HasCommand_";
        public const string HasAssociatedDataPrefix = "HasAssociatedData_";
        private NodeViewModel rootNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphViewModel"/> class.
        /// </summary>
        /// <param name="serviceProvider">A service provider that can provide a <see cref="IDispatcherService"/> and an <see cref="GraphViewModelService"/> to use for this view model.</param>
        /// <param name="type"></param>
        /// <param name="rootPresenters">The root <see cref="INodePresenter"/> instances.</param>
        private GraphViewModel([NotNull] IViewModelServiceProvider serviceProvider, [NotNull] Type type, [NotNull] IEnumerable<INodePresenter> rootPresenters)
            : base(serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (rootPresenters == null) throw new ArgumentNullException(nameof(rootPresenters));
            GraphViewModelService = serviceProvider.TryGet<GraphViewModelService>();
            if (GraphViewModelService == null) throw new InvalidOperationException($"{nameof(GraphViewModel)} requires a {nameof(GraphViewModelService)} in the service provider.");
            Logger = GlobalLogger.GetLogger(DefaultLoggerName);
            if (rootPresenters == null) throw new ArgumentNullException(nameof(rootNode));
            var viewModelFactory = serviceProvider.Get<GraphViewModelService>().NodeViewModelFactory;
            viewModelFactory.CreateGraph(this, type, rootPresenters);
        }

        /// <inheritdoc/>
        public override void Destroy()
        {
            RootNode.Children.SelectDeep(x => x.Children).ForEach(x => x.Destroy());
            RootNode.Destroy();
        }

        [CanBeNull]
        public static GraphViewModel Create([NotNull] IViewModelServiceProvider serviceProvider, [NotNull] IReadOnlyCollection<IPropertyProviderViewModel> propertyProviders)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (propertyProviders == null) throw new ArgumentNullException(nameof(propertyProviders));

            var rootNodes = new List<INodePresenter>();
            Type type = null;
            var factory = serviceProvider.Get<GraphViewModelService>().NodePresenterFactory;
            foreach (var propertyProvider in propertyProviders)
            {
                if (!propertyProvider.CanProvidePropertiesViewModel)
                    return null;

                var rootNode = propertyProvider.GetRootNode();
                if (rootNode == null)
                    return null;

                if (type == null)
                    type = rootNode.Type;
                else if (type != rootNode.Type)
                    return null;

                var node = factory.CreateNodeHierarchy(rootNode, new GraphNodePath(rootNode), propertyProvider);
                rootNodes.Add(node);
            }

            if (propertyProviders.Count == 0 || type == null) throw new ArgumentException($@"{nameof(propertyProviders)} cannot be empty.", nameof(propertyProviders));
            return new GraphViewModel(serviceProvider, type, rootNodes);
        }

        /// <summary>
        /// Gets the root node of this <see cref="GraphViewModel"/>.
        /// </summary>
        public NodeViewModel RootNode { get { return rootNode; } set { SetValue(ref rootNode, value); } }
                
        /// <summary>
        /// Gets the <see cref="GraphViewModelService"/> associated to this view model.
        /// </summary>
        public GraphViewModelService GraphViewModelService { get; }

        /// <summary>
        /// Gets the <see cref="Logger"/> associated to this view model.
        /// </summary>
        public Logger Logger { get; private set; }

        /// <summary>
        /// Raised when the value of an <see cref="NodeViewModel"/> contained into this view model has changed.
        /// </summary>
        public event EventHandler<NodeViewModelValueChangedArgs> NodeValueChanged;

        [Pure]
        public NodeViewModel ResolveNode(string path)
        {
            var members = path.Split('.');
            if (members[0] != RootNode.Name)
                return null;

            var currentNode = RootNode;
            foreach (var member in members.Skip(1))
            {
                currentNode = currentNode.Children.FirstOrDefault(x => x.Name == member);
                if (currentNode == null)
                    return null;
            }
            return currentNode;
        }

        internal void NotifyNodeChanged(string nodePath)
        {
            NodeValueChanged?.Invoke(this, new NodeViewModelValueChangedArgs(this, nodePath));
        }
    }
}
