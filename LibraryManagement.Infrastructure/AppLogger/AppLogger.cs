using LibraryManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement.Infrastructure.AppLogger;
public class LoggerMD
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string id { get; set; }
    private string user_id;
    private string message;
    private DateTime timestamp;

    public string Id { get => id; set => id = value; }
    public string User_id { get => user_id; set => user_id = value; }
    public string Message { get => message; set => message = value; }
    public DateTime Timestamp { get => timestamp; set => timestamp = value; }

}
public class LoggerDB
{
    //Start up database once
    static IMongoDatabase loggerDatabase;
    public static IMongoDatabase LoggerDatabase
    {
        get
        {
            if (loggerDatabase == null)
            {
                loggerDatabase = new MongoClient("mongodb+srv://USER_ADMIN_VIDEO_SHARING:BRleLqbEn9kcPR7Q@cluster0.48un0.mongodb.net/VideoSharing?retryWrites=true&w=majority").GetDatabase("LoggerAPI");
            }
            return loggerDatabase;
        }
    }
}
public class AppLogger<TCategory> : IAppLogger<TCategory>
{
    private readonly ILogger<TCategory> _logger;
    private readonly IMongoCollection<LoggerMD> _logCollection;


    public AppLogger(ILoggerFactory loggerFactory)
    {
        _logCollection = LoggerDB.LoggerDatabase.GetCollection<LoggerMD>("logsbookstore");
        _logger = loggerFactory.CreateLogger<TCategory>();
    }

    public void LogInformation(string? message, params object[] args)
    {
        var document = new LoggerMD
        {
            Message = message,
            Timestamp = DateTime.Now
        };
        foreach (var item in args)
        {

            if (item is string)
            {
                document.User_id = (string)item;
            }

        }
        _logCollection.InsertOne(document);


        _logger.LogInformation(message, args);
    }

    public void LogWarning(string? message, params object[] args)
    {
        var document = new LoggerMD
        {
            Message = message,
            Timestamp = DateTime.Now
        };
        foreach (var item in args)
        {

            if (item is string)
            {
                document.User_id = (string)item;
            }

        }
        _logCollection.InsertOne(document);

        _logger.LogWarning(message, args);
    }

    public void LogError(string? message, params object[] args)
    {
        var document = new LoggerMD
        {
            Message = message,
            Timestamp = DateTime.Now
        };
        foreach (var item in args)
        {

            if (item is string)
            {
                document.User_id = (string)item;
            }

        }
        _logCollection.InsertOne(document);

        _logger.LogError(message, args);
    }
}