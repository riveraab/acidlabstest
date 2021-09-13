using Acidlabs.Core;
using Acidlabs.Core.Repository;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acidlabs.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly IAmazonDynamoDB _client;
        private readonly IDynamoDBContext _context;
        public UserRepository(IAmazonDynamoDB client, IDynamoDBContext context)
        {
            _client = client;
            _context = context;
        }

        public async Task<User> getUserByIdAsync(string id)
        {
            var user = new User();
            var request = new QueryRequest
            {
                TableName = "acid-users",
                KeyConditionExpression = "Id = :v_Id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
        {":v_Id", new AttributeValue { S =  id }}}
            };

            var response = await _client.QueryAsync(request);

            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                // Process the result.
                item.TryGetValue("Id", out var userid);
                item.TryGetValue("Email", out var email);
                item.TryGetValue("Name", out var name);

                user.Id = userid?.S;
                user.Email = email?.S;
                user.Name = name?.S;
            }
            return user;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            List<User> users = new List<User>();

            var result = await _client.ScanAsync(new ScanRequest { TableName = "acid-users" });
            if (result != null && result.Items != null)
            {
                foreach (var item in result.Items)
                {
                    item.TryGetValue("Id", out var id);
                    item.TryGetValue("Email", out var email);
                    item.TryGetValue("Name", out var name);
                    users.Add(new User { Id = id?.S, Email = email?.S, Name = name?.S });
                }

            }
            return users;
        }

        public async Task SaveUserAsync(User user)
        {
            user.Id = Guid.NewGuid().ToString("N");
            await this._context.SaveAsync(user);

        }
    }
}
