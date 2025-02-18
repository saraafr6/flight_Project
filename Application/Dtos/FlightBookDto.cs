public class FlightBookDto
{
    public Guid Id { get; set; }
    public Guid FlightId { get; set; }
    public string FlightNumber { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public DateTimeOffset BookingDate { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsActive { get; set; }
    public virtual DateTimeOffset? CreateDateTime { get; set; }
}
