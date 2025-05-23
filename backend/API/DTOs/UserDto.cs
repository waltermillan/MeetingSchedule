namespace API.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }             // Table: Users | Field: Id
        public string? Name { get; set; }        // Table: Users | Field: Name
        public string? UserName { get; set; }    // Table: Users | Field: UserName
        public DateTime CreatedAt { get; set; }     //Table: Users | Field: CreatedAt
        public DateTime UpdatedAt { get; set; }     //Table: Users | Field: UpdatedAt
    }
}
