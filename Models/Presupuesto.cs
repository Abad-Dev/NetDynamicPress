namespace NetDynamicPress.Models;

public class Presupuesto
{
    public Guid Id { get;set; }
    public Guid UserId { get;set; }
    public string Name { get;set; }
    public string Config { get;set; }
    public DateTime Creation { get;set; }
}