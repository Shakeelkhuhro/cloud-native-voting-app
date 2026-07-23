using System;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using Npgsql;
using StackExchange.Redis;

namespace Worker
{
    class Program
    {
        static int Main(string[] args)
        {
            string pgHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "postgres";
            string pgPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
            string pgDb = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "postgres";
            string pgUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
            string pgPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "postgres";

            string redisHost = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "redis";
            string redisPassword = Environment.GetEnvironmentVariable("REDIS_PASSWORD");

            Console.WriteLine($"Postgres: {pgHost}:{pgPort}");
            Console.WriteLine($"Redis: {redisHost}");

            string conn =
                $"Server={pgHost};Port={pgPort};Database={pgDb};Username={pgUser};Password={pgPassword};";

            var pgsql = OpenDbConnection(conn);

            var redisConn = OpenRedisConnection(redisHost, redisPassword);

            var redis = redisConn.GetDatabase();

            var keepAlive = pgsql.CreateCommand();
            keepAlive.CommandText = "SELECT 1";

            var definition = new { vote = "", voter_id = "" };

            while (true)
            {
                Thread.Sleep(100);

                if (redisConn == null || !redisConn.IsConnected)
                {
                    redisConn = OpenRedisConnection(redisHost, redisPassword);
                    redis = redisConn.GetDatabase();
                }

                string json = redis.ListLeftPop("votes");

                if (json != null)
                {
                    var vote = JsonConvert.DeserializeAnonymousType(json, definition);

                    if (pgsql.State != System.Data.ConnectionState.Open)
                        pgsql = OpenDbConnection(conn);

                    UpdateVote(pgsql, vote.voter_id, vote.vote);
                }
                else
                {
                    keepAlive.ExecuteNonQuery();
                }
            }
        }

        static NpgsqlConnection OpenDbConnection(string connectionString)
        {
            while (true)
            {
                try
                {
                    var conn = new NpgsqlConnection(connectionString);
                    conn.Open();

                    Console.WriteLine("Connected to Postgres");

                    var cmd = conn.CreateCommand();

                    cmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS votes(
                        id VARCHAR(255) PRIMARY KEY,
                        vote VARCHAR(255)
                    );";

                    cmd.ExecuteNonQuery();

                    return conn;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Waiting for db");
                    Thread.Sleep(2000);
                }
            }
        }

        static ConnectionMultiplexer OpenRedisConnection(string host,string password)
        {
            while(true)
            {
                try
                {
                    var ip=GetIp(host);

                    var options=new ConfigurationOptions();

                    options.EndPoints.Add(ip,6379);

                    if(!string.IsNullOrEmpty(password))
                        options.Password=password;

                    options.AbortOnConnectFail=false;

                    Console.WriteLine("Connected to Redis");

                    return ConnectionMultiplexer.Connect(options);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Waiting for redis");
                    Thread.Sleep(2000);
                }
            }
        }

        static string GetIp(string hostname)
        {
            return Dns.GetHostEntry(hostname)
                .AddressList
                .First(x=>x.AddressFamily==AddressFamily.InterNetwork)
                .ToString();
        }

        static void UpdateVote(NpgsqlConnection connection,string id,string vote)
        {
            try
            {
                var cmd=connection.CreateCommand();

                cmd.CommandText=
                @"INSERT INTO votes(id,vote)
                  VALUES(@id,@vote)
                  ON CONFLICT(id)
                  DO UPDATE SET vote=@vote;";

                cmd.Parameters.AddWithValue("@id",id);
                cmd.Parameters.AddWithValue("@vote",vote);

                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
