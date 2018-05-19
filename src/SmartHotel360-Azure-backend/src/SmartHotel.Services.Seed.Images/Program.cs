using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SmartHotel.Services.Seed.Images
{
    class Program
    {
        private static PhotoUploader _photoUploader;
        static void Main(string[] args)
        {
            var cfgBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true);
            var parameters = ParseArgs(args);
            if (!string.IsNullOrEmpty(parameters.Environment))
            {
                cfgBuilder.AddJsonFile($"appsettings.{parameters.Environment}.json", optional: false);
            }

            var cfg = cfgBuilder.AddEnvironmentVariables().Build();


            var constr = cfg["HotelsConnectionString"];
            var blobconstr = cfg["StorageConnectionString"];
            var blobname = cfg["StorageAccountName"];
            Console.WriteLine($"+++ using db constr {constr}");
            Console.WriteLine($"+++ using storage {blobname} with constr {blobconstr}");

            _photoUploader = new PhotoUploader(blobname, blobconstr);
            Task.Run(() => _photoUploader.Init()).GetAwaiter().GetResult();





            if (parameters.AddHotels)
            {
                Task.Run(() => AddRoomsPhotos(constr)).GetAwaiter().GetResult();
                Task.Run(() => AddHotelsPhotos(constr)).GetAwaiter().GetResult();
                Task.Run(() => AddConferencePhotos(constr)).GetAwaiter().GetResult();
            }
            if (parameters.AddSuggestions)
            {
                Task.Run(() => AddSuggestionsPhotos(constr)).GetAwaiter().GetResult();
            }
            if (parameters.AddBotImages)
            {
                Task.Run(() => AddBotImages()).GetAwaiter().GetResult();
            }
        }

        private async static Task AddBotImages()
        {
            Console.WriteLine($"+++ Adding photos for bot");
            await _photoUploader.UploadBotFrom("bot");
        }

        private static CliParameters ParseArgs(string[] args)
        {
            var parameters = new CliParameters();

            foreach (var arg in args)
            {
                switch (arg)
                {
                    case "--suggestions":
                        parameters.AddSuggestions = true;
                        break;
                    case "--hotels":
                        parameters.AddHotels = true;
                        break;
                    case "--bot":
                        parameters.AddBotImages = true;
                        break;
                    default:
                        if (arg.StartsWith("--env:"))
                        {
                            var env = arg.Substring("--env:".Length);
                            parameters.Environment = env;
                        }
                        break;
                }
            }


            return parameters;
        }

        private async static Task AddRoomsPhotos(string constr)
        {
            using (var con = new SqlConnection(constr))
            {
                var sql = "Select Id, NumPhotos from dbo.RoomType";
                var cmd = new SqlCommand(sql, con);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var roomid = reader.GetInt32(0);
                        var numphotos = reader.GetInt32(1);
                        Console.WriteLine($"+++ Adding {numphotos} photos for room {roomid}");
                        await _photoUploader.UploadDefaultRoomPhoto(roomid);
                        for (var idx = 1; idx <= numphotos; idx++)
                        {
                            await _photoUploader.UploadSmallRoomPhoto(roomid, idx);
                        }
                    }
                }
            }
        }

        private static async Task AddHotelsPhotos(string constr)
        {
            using (var con = new SqlConnection(constr))
            {
                var sql = "Select Id, NumPhotos from dbo.Hotels";
                using (var cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var hotelid = reader.GetInt32(0);
                            var numphotos = reader.GetInt32(1);
                            Console.WriteLine($"+++ Adding {numphotos} photos for hotel {hotelid}");
                            await _photoUploader.UploadDefaultHotelPhoto(hotelid);
                            await _photoUploader.UploadFeaturedHotelPhoto(hotelid);
                            for (var idx = 1; idx <= numphotos; idx++)
                            {
                                await _photoUploader.UploadSmallHotelPhoto(hotelid, idx);
                            }
                        }
                    }
                }
            }
        }

        private static async Task AddConferencePhotos(string constr)
        {
            using (var con = new SqlConnection(constr))
            {
                var sql = "Select Id, NumPhotos from dbo.ConferenceRoom";
                using (var cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var confid = reader.GetInt32(0);
                            var numphotos = reader.GetInt32(1);
                            Console.WriteLine($"+++ Adding {numphotos} photos for conference room {confid}");
                            await _photoUploader.UploadFeaturedConferencePhoto(confid);
                            for (var idx = 1; idx <= numphotos; idx++)
                            {
                                await _photoUploader.UploadSmallConferencePhoto(confid, idx);
                            }
                        }
                    }
                }
            }
        }

        private async static Task AddSuggestionsPhotos(string constr)
        {
            Console.WriteLine($"+++ Adding photos from suggestions");
            await _photoUploader.UploadSuggestionsFrom("suggestions");
        }

    }
}
