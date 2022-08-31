using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Blazor.Diagrams.Components.Controls;
using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Options;

namespace Blazor.Diagrams
{
    public class BlazorDiagram : Diagram
    {
        private readonly Dictionary<Type, Type> _componentByModelMapping;

        public BlazorDiagram(BlazorDiagramOptions? options = null)
        {
            _componentByModelMapping = new Dictionary<Type, Type>
            {
                [typeof(RemoveControl)] = typeof(RemoveControlWidget),
                [typeof(BoundaryControl)] = typeof(BoundaryControlWidget),
                [typeof(DragNewLinkControl)] = typeof(DragNewLinkControlWidget),
            };

            Options = options ?? new BlazorDiagramOptions();
        }

        public override BlazorDiagramOptions Options { get; }

        public void RegisterModelComponent<TModel, TComponent>(bool replace = false)
            where TModel : Model where TComponent : ComponentBase
        {
            RegisterModelComponent(typeof(TModel), typeof(TComponent), replace);
        }

        public void RegisterModelComponent(Type modelType, Type componentType, bool replace = false)
        {
            if (!replace && _componentByModelMapping.ContainsKey(modelType))
                throw new Exception($"Component already registered for model '{modelType.Name}'.");

            _componentByModelMapping[modelType] = componentType;
        }

        public Type? GetComponentForModel(Type modelType, bool checkSubclasses = true)
        {
            if (_componentByModelMapping.ContainsKey(modelType))
            {
                return _componentByModelMapping[modelType];
            }

            if (checkSubclasses)
            {
                foreach (var rmt in _componentByModelMapping.Keys)
                {
                    if (modelType.IsSubclassOf(rmt))
                        return _componentByModelMapping[rmt];
                }
            }

            return null;
        }

        public Type? GetComponentForModel<TModel>(bool checkSubclasses = true) where TModel : Model
            => GetComponentForModel(typeof(TModel), checkSubclasses);

        public Type? GetComponentForModel(Model model, bool checkSubclasses = true)
            => GetComponentForModel(model.GetType(), checkSubclasses);
    }
}