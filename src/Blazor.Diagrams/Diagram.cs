using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Blazor.Diagrams
{
    public class Diagram : DiagramBase
    {
        private readonly Dictionary<Type, Type> _componentByModelMapping;

        public Diagram(DiagramOptions? options = null) : base(options)
        {
            _componentByModelMapping = new Dictionary<Type, Type>();
        }

        public void RegisterModelComponent<M, C>(bool replace = false) where M : Model where C : ComponentBase
            => RegisterModelComponent(typeof(M), typeof(C), replace);

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

        public Type? GetComponentForModel<M>(bool checkSubclasses = true) where M : Model
            => GetComponentForModel(typeof(M), checkSubclasses);

        public Type? GetComponentForModel(Model model, bool checkSubclasses = true)
            => GetComponentForModel(model.GetType(), checkSubclasses);
    }
}
