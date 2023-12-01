namespace NetDynamicPress.Models;

public class User : Base
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string TopImage { get;set; }
    public string Signature { get;set; }

    public virtual IEnumerable<Presupuesto> Presupuestos { get;set; }
}