using System;
using Dapper;
using ModernPortfolio.Models;
using Npgsql;

namespace ModernPortfolio.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration)
    {
        
    }
    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        using var connection= new NpgsqlConnection(_connectionString);
        var sql = "SELECT * FROM Users WHERE Username = @UserName";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new {UserName= userName});
    }
}
