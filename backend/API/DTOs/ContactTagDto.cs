namespace API.DTOs
{
    public class ContactTagDto
    {
        public Guid Id { get; set; }                //Table: ContactTag | Field: Id
        public Guid ContactId { get; set; }         //Table: ContactTag | Field: ContactId
        public string Contact { get; set; }         //Table: Contact | Field: Name
        public Guid TagId { get; set; }             //Table: ContactTag | Field: TagId 
        public string Tag { get; set; }             //Table: Tag | Field: Name

    }
}
