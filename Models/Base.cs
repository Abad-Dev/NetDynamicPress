namespace NetDynamicPress.Models;

public class Base
{
    public string Id { get; private set; }
    public string Name { get; set; }

    public Base()
    {
        Id = Guid.NewGuid().ToString();
    }

    public override string ToString()
    {
        return $"{Name},{Id}";
    }
}
