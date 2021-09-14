using Acidlabs.Core;
using Acidlabs.Core.Repository;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<User> GetUserByIdAsync(string id)
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
            if (response != null && response.Items != null && response.Count > 0)
            {
                var item = response.Items[0];

                item.TryGetValue("Id", out var userid);
                item.TryGetValue("Email", out var email);
                item.TryGetValue("Name", out var name);

                user.Id = userid?.S;
                user.Email = email?.S;
                user.Name = name?.S;
            }
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var scanConditions = new List<ScanCondition>();
            scanConditions.Add(new ScanCondition("Email", ScanOperator.Equal, email));
            var users = await _context.ScanAsync<User>(scanConditions, null).GetRemainingAsync();

            return users.FirstOrDefault();
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

        public async Task<User> SaveUserAsync(User user)
        {
            await this._context.SaveAsync(user);
            return user;
        }

        public async Task RemoveUserAsync(string id)
        {
            await _context.DeleteAsync<User>(id);
        }

    }
}
