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

        public void RegisterModelComponent<M, C>() where M : Model where C : ComponentBase
            => RegisterModelComponent(typeof(M), typeof(C));

        public void RegisterModelComponent(Type modelType, Type componentType)
        {
            if (_componentByModelMapping.ContainsKey(modelType))
                throw new Exception($"Component already registered for model '{modelType.Name}'.");

            _componentByModelMapping.Add(modelType, componentType);
        }

        public Type? GetComponentForModel<M>(M model) where M : Model
        {
            var modelType = model.GetType();
            return _componentByModelMapping.ContainsKey(modelType) ? _componentByModelMapping[modelType] : null;
        }
    }
}
