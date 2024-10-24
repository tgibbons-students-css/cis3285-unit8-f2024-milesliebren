using Microsoft.Data.SqlClient;
using SingleResponsibilityPrinciple;
using System.IO;
using Xunit;

namespace Unit8_SRP_F24
{
    public class TradeProcessorTests
    {
        [Fact]
        public void ProcessTrades_ShouldAddRecordsToDatabase()
        {
            // Arrange
            var processor = new TradeProcessor();
            var sampleData = "GBPUSD,1000,1.51\nEURUSD,2000,1.21\n";  // Sample trade data
            var sampleStream = new MemoryStream();
            var writer = new StreamWriter(sampleStream);
            writer.Write(sampleData);
            writer.Flush();
            sampleStream.Position = 0;  // Reset stream to the beginning

            // Act
            processor.ProcessTrades(sampleStream);

            // Assert
            int recordCount = CountDbRecords(); // This is a method that counts the number of records in the DB.
            Assert.Equal(2, recordCount); // We expect 2 records to be inserted
        }

        // This method should be implemented to connect to the database and count records.
        private int CountDbRecords()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\tradedatabase.mdf;Integrated Security=True;Connect Timeout=30;";
            int count = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT COUNT(*) FROM TradeRecords", connection);
                count = (int)command.ExecuteScalar();
                connection.Close();
            }
            return count;
        }
    }
}