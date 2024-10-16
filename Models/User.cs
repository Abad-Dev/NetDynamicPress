namespace NetDynamicPress.Models;

public class User : Base
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] TopImage { get;set; }
    public byte[] Signature { get;set; }

    public virtual IEnumerable<Presupuesto> Presupuestos { get;set; }
}
