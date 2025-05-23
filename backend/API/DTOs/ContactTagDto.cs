using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs
{
    public class ContactTagDto
    {
        public Guid Id { get; set; }                //Table: ContactTags | Field: Id
        public Guid ContactId { get; set; }         //Table: ContactTags | Field: ContactId
        public string? Contact { get; set; }        //Table: Contacts | Field: Name
        public Guid TagId { get; set; }             //Table: ContactTags | Field: TagId 
        public string? Tag { get; set; }            //Table: Tags | Field: Name
        public DateTime CreatedAt { get; set; }     //Table: ContactTags | Field: CreatedAt
        public DateTime UpdatedAt { get; set; }     //Table: ContactTags | Field: UpdatedAt

    }
}
