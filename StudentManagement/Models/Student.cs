using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentManagement.Models
{
    [BsonIgnoreExtraElements]
    public class Student
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }=string.Empty;
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("graduated")]
        public Boolean IsGraduated { get; set; }
        [BsonElement("courses")]
        public string[]? Course { get; set; }
        [BsonElement("gender")]
        public string Gender { get; set; }=string.Empty ;

        [BsonElement("age")]
        public int Age { get; set; }
    }
}
