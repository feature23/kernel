namespace F23.Kernel;

/// <summary>
/// Represents an object that can create a deep clone of itself.
/// </summary>
/// <typeparam name="T">The type of the object to clone.</typeparam>
public interface IDeepCloneable<out T>
{
    /// <summary>
    /// Creates a deep clone of the current object.
    /// </summary>
    /// <returns>A deep clone of the current object.</returns>
    T DeepClone();
}
