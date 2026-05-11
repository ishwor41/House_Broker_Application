using System.Collections.Generic;
using System.Linq;
using HouseBroker.Domain.Entities;
using HouseBroker.Application.Interfaces;
using System.Data.SqlClient;
using HouseBroker.Infrastructure.Data;
using System.Data;
using HouseBroker.Application.DTOs.Property;

namespace HouseBroker.Infrastructure.Repositories
{
    /// <summary>
    /// Repository layer responsible for all database operations related to Property.
    /// Uses ADO.NET with stored procedure (sp_Property_Manage).
    /// This layer communicates directly with SQL Server.
    /// </summary>
    public class PropertyRepository : IPropertyRepository
    {
        private static List<Property> _db = new();

        private readonly ISqlConnectionFactory _connectionFactory;

        public PropertyRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Inserts a new Property record into the database.
        /// </summary>
        public async Task AddAsync(Property property)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();

            await connection.OpenAsync();

            using SqlCommand command = new SqlCommand("sp_Property_Manage", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Title", property.Title);
            command.Parameters.AddWithValue("@PropertyType", property.PropertyType);
            command.Parameters.AddWithValue("@Location", property.Location);
            command.Parameters.AddWithValue("@Price", property.Price);
            command.Parameters.AddWithValue("@Features", property.Features);
            command.Parameters.AddWithValue("@CommissionAmount", property.CommissionAmount);
            command.Parameters.AddWithValue("@ImageUrl", property.PropertyImageUrl);
            command.Parameters.AddWithValue("@CreatedDate", property.CreatedDate);
            command.Parameters.AddWithValue("@CreatedBy", property.CreatedBy);
            command.Parameters.AddWithValue("@RecordStatus", property.RecordStatus);
            command.Parameters.AddWithValue("@flag", "I");
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Retrieves all properties with optional filtering and role-based user access.
        /// </summary>
        public async Task<List<Property>> GetAllAsync(PropertyFilterDto filter, Guid? UserId)
        {
            List<Property> properties = new();

            using SqlConnection connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using SqlCommand command = new SqlCommand("sp_Property_Manage", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@flag", "L");
            command.Parameters.AddWithValue("@Title", filter.Title);
            command.Parameters.AddWithValue("@Location", filter.Location);
            command.Parameters.AddWithValue("@MaxPrice", filter.MaxPrice);
            command.Parameters.AddWithValue("@Features", filter.Features);
            command.Parameters.AddWithValue("@MinPrice", filter.MinPrice);
            command.Parameters.AddWithValue("@UserId", UserId);



            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                properties.Add(new Property
                {
                    Id = Guid.Parse(reader["Id"].ToString()),
                    Title = reader["Title"].ToString(),
                    PropertyType = reader["PropertyType"].ToString(),
                    Location = reader["Location"].ToString(),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Features = reader["Features"].ToString(),
                    CommissionAmount = reader["CommissionAmount"] != DBNull.Value? Convert.ToDecimal(reader["CommissionAmount"]): null
                });
            }

            return properties;
        }

        /// <summary>
        /// Retrieves a single property by Id.
        /// </summary>
        public async Task<Property?> GetByIdAsync(Guid id, Guid? UserId)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using SqlCommand command = new SqlCommand("sp_Property_Manage", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@flag", "G");
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@UserId", UserId);



            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Property
                {
                    Id = Guid.Parse(reader["Id"].ToString()),
                    Title = reader["Title"].ToString(),
                    PropertyType = reader["PropertyType"].ToString(),
                    Location = reader["Location"].ToString(),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Features = reader["Features"].ToString(),
                    PropertyImageUrl = reader["PropertyImageUrl"].ToString(),
                    CommissionAmount = reader["CommissionAmount"] != DBNull.Value ? Convert.ToDecimal(reader["CommissionAmount"]) : null
                };
            }

            return null;
        }

        /// <summary>
        /// Updates existing property details in database.
        /// </summary>
        public async Task UpdateAsync(Property property)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using SqlCommand command = new SqlCommand("sp_Property_Manage", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", property.Id);

            command.Parameters.AddWithValue("@Title", property.Title);
            command.Parameters.AddWithValue("@PropertyType", property.PropertyType);
            command.Parameters.AddWithValue("@Location", property.Location);
            command.Parameters.AddWithValue("@Price", property.Price);
            command.Parameters.AddWithValue("@Features", property.Features);
            command.Parameters.AddWithValue("@CommissionAmount", property.CommissionAmount);
            command.Parameters.AddWithValue("@ImageUrl", property.PropertyImageUrl);
            command.Parameters.AddWithValue("@UpdatedDate", property.UpdatedDate);
            command.Parameters.AddWithValue("@UpdatedBy", property.UpdatedBy);
            command.Parameters.AddWithValue("@RecordStatus", property.RecordStatus);
            command.Parameters.AddWithValue("@flag", "U");

            await command.ExecuteNonQueryAsync();
        }

         /// <summary>
        /// Soft deletes a property based on stored procedure logic.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using SqlCommand command = new SqlCommand("sp_Property_Manage", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@flag", "D");


            await command.ExecuteNonQueryAsync();
        }


    }
}