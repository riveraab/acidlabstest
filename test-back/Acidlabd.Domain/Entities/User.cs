using Amazon.DynamoDBv2.DataModel;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Acidlabs.Core
{
    [DynamoDBTable("acid-users")]
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [IgnoreDataMember]
        public string PasswordHash { get; set; }
        [IgnoreDataMember]
        public string PasswordSalt { get; set; }
    }
}
