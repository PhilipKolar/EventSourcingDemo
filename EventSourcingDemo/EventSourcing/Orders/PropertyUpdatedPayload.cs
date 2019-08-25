using System;

namespace EventSourcingDemo.EventSourcing.Orders
{
    public class PropertyUpdatedPayload
    {
        public PropertyUpdatedPayload()
        {
        }

        public PropertyUpdatedPayload(OrderProperty orderProperty, object value)
        {
            Name = Enum.GetName(typeof(OrderProperty), orderProperty);
            var valueType = value?.GetType();
            var validator = new PropertyTypeValidator();
            if (!validator.IsValid(orderProperty, valueType))
            {
                throw new NotSupportedException($"The property {Name} does not support a value with type {valueType.Name}");
            }
            Type = valueType?.FullName;
            Value = value;
        }

        public string Name { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }
    }
}
