using System;

public class ServiceDescriptor
{
    public Type ServiceType { get; }
    public object Implementation { get; }

    public ServiceDescriptor(object implementation)
    {
        ServiceType = implementation.GetType();
        Implementation = implementation;
    }
}