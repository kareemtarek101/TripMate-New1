public class BookingDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int DestinationId { get; set; }
    public DateTime BookingDate { get; set; }
    public int NumberOfPeople { get; set; }

    public string Status { get; set; } = "Confirmed";


}