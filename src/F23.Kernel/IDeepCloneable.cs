namespace F23.Kernel;

public interface IDeepCloneable<out T>
{
    T DeepClone();
}
