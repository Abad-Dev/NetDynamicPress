namespace NetDynamicPress.Models;

public class Presupuesto : Base
{
    public User User { get;set; }
    public string UserId { get;set; }
    public string Config { get;set; }
    public DateTime Creation { get;set; }

    public Presupuesto()
    {
        Creation = DateTime.UtcNow;
    }
}