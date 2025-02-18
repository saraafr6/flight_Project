public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }
    public DateTimeOffset? BirthDate { get; set; }
    public bool? IsActive { get; set; }
}
