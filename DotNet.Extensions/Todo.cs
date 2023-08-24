namespace DotNet.Extensions;

public sealed class Todo : SystemException
{
    public Todo(string message) : base(message) { }
}
