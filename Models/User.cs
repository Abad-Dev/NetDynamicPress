namespace NetDynamicPress.Models;

public class User : Base
{
    public string TopImage { get;set; }
    public string Signature { get;set; }

    public IEnumerable<Presupuesto> Presupuestos { get;set; }
}