using System.Runtime.Serialization;
using Figures.Domain;

namespace Figures.Infrastructure.Utilities;

public class CustomSerializationBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        if (typeName.EndsWith(nameof(Circle)))
        {
            return typeof(Circle);
        }
        else if (typeName.EndsWith(nameof(Rectangle)))
        {
            return typeof(Rectangle);
        }

        return Type.GetType($"{typeName}, {assemblyName}")!;
    }
}