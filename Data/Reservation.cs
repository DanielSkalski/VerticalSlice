namespace VerticalSlice.Data;

public class Reservation
{
    public int Id { get; private set; }
    public DateTime Day { get; private set; }
    public string Name { get; private set; }
    public string ContactEmail { get; private set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Reservation(string name, string contactEmail, DateTime day, DateTimeOffset createdAt)
    {
        Name = name;
        ContactEmail = contactEmail;
        Day = day;
        CreatedAt = createdAt;
    }
}

