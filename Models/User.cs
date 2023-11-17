namespace NetDynamicPress.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get;set; }
    public string TopImage { get;set; }
    public string Signature { get;set; }

    public IEnumerable<Presupuesto> Presupuestos { get;set; }
}