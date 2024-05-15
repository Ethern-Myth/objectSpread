using System.Collections.Concurrent;
using System.Reflection;

namespace objectSpread
{
    public static class ObjectExtensions
    {
        // A thread-safe dictionary to cache the properties of types for quick access
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        // Efficiently merges the properties of multiple objects into a new object using reflection and caching
        public static T Spread<T>(params object[] sources) where T : class, new()
        {
            // Create a new instance of the target type
            T result = new T();

            // Get the properties of the target type, using caching for efficiency
            PropertyInfo[] resultProperties = GetPropertiesCached(typeof(T));

            // Iterate over each source object provided
            foreach (var source in sources)
            {
                if (source == null) continue; // Skip null sources

                // Get the properties of the source object, using caching for efficiency
                PropertyInfo[] sourceProperties = GetPropertiesCached(source.GetType());

                // Iterate over each property in the source object
                foreach (var sourceProp in sourceProperties)
                {
                    if (!sourceProp.CanRead) continue; // Skip properties that can't be read

                    // Find the corresponding property in the target object
                    var targetProp = FindProperty(resultProperties, sourceProp.Name);

                    // Check if the target property exists, can be written to, and is compatible with the source property type
                    if (targetProp != null && targetProp.CanWrite && targetProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                    {
                        try
                        {
                            // Set the property's value on the target object
                            targetProp.SetValue(result, sourceProp.GetValue(source));
                        }
                        catch (Exception ex)
                        {
                            // Handle or log the error as appropriate
                            Console.WriteLine($"Error setting property {sourceProp.Name}: {ex.Message}");
                        }
                    }
                }
            }

            return result;
        }

        // Retrieves the properties of a type from the cache or loads them if not already cached
        private static PropertyInfo[] GetPropertiesCached(Type type)
        {
            return PropertyCache.GetOrAdd(type, t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance));
        }

        // Finds a property in the array by name
        private static PropertyInfo? FindProperty(PropertyInfo[] properties, string name)
        {
            foreach (var prop in properties)
            {
                if (prop.Name == name) return prop;
            }
            return null;
        }
    }
}
