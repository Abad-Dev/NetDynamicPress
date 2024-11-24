namespace NetDynamicPress.Models;
public class Recibo : Base
{
    public User User { get; set; }
    public string UserId { get; set; }
    public string Config { get; set; }
    public DateTime Creation { get; set; }

    public Recibo()
    {
        Creation = DateTime.UtcNow;
    }
}
