using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReleaseMusicServerlessApi.Models
{
    public class User
    {
        [BsonId()]
        public ObjectId Id { get; private set; }

        [BsonElement("email")]
        [BsonRequired()]
        public string Email { get; private set; }

        [BsonElement("password")]
        [BsonRequired()]
        public string Password { get; private set; }

        public User(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }

        public User(ObjectId id, string email)
        {
            this.Id = id;
            this.Email = email;
        }
    }
}
