using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Seed.Images
{
    public class PhotoUploader
    {
        private readonly StorageCredentials _credentials;
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudBlobClient _blobClient;

        private CloudBlobContainer _roomsContainer;
        private CloudBlobContainer _hotelsContainer;
        private CloudBlobContainer _confRoomsContainer;

        private readonly List<string> _hotelSmallFiles;
        private readonly List<string> _hotelLargeFiles;
        private readonly List<string> _roomSmallFiles;
        private readonly List<string> _roomLargeFiles;
        private readonly List<string> _confSmallFiles;

        private int _hotelSmallCount = 0;
        private int _hotelLargeCount = 0;
        private int _roomSmallCount = 0;
        private int _roomLargeCount = 0;
        private int _confSmallCount = 0;

        public PhotoUploader(string name, string constr)
        {
            _credentials = new StorageCredentials(name, constr);
            _storageAccount = new CloudStorageAccount(_credentials, useHttps: true);
            _blobClient = _storageAccount.CreateCloudBlobClient();

            _hotelSmallFiles = new List<string>();
            _hotelLargeFiles = new List<string>();
            _roomSmallFiles = new List<string>();
            _roomLargeFiles = new List<string>();
            _confSmallFiles = new List<string>();
        }

        public async Task Init()
        {
            _roomsContainer = _blobClient.GetContainerReference("picrooms");
            await _roomsContainer.CreateIfNotExistsAsync();

            _hotelsContainer = _blobClient.GetContainerReference("pichotels");
            await _hotelsContainer.CreateIfNotExistsAsync();

            _confRoomsContainer = _blobClient.GetContainerReference("picconf");
            await _confRoomsContainer.CreateIfNotExistsAsync();

            var separator = Path.DirectorySeparatorChar;

            _roomLargeFiles.AddRange(Directory.EnumerateFiles($"rooms{separator}large").OrderBy(x => x));
            _hotelLargeFiles.AddRange(Directory.EnumerateFiles($"hotels{separator}large").OrderBy(x => x));
            _roomSmallFiles.AddRange(Directory.EnumerateFiles($"rooms{separator}small").OrderBy(x => x));
            _hotelSmallFiles.AddRange(Directory.EnumerateFiles($"hotels{separator}small").OrderBy(x => x));
            _confSmallFiles.AddRange(Directory.EnumerateFiles($"conf{separator}small").OrderBy(x => x));
        }

        public async Task UploadSmallRoomPhoto(int roomid, int idx)
        {
            var newBlob = _roomsContainer.GetBlockBlobReference($"{roomid}_{idx}.png");
            await newBlob.UploadFromFileAsync(_roomSmallFiles[_roomSmallCount % _roomSmallFiles.Count]);
            _roomSmallCount++;
        }

        public async Task UploadSmallHotelPhoto(int hotelid, int idx)
        {
            var newBlob = _hotelsContainer.GetBlockBlobReference($"{hotelid}_{idx}.png");
            await newBlob.UploadFromFileAsync(_hotelSmallFiles[_hotelSmallCount % _hotelSmallFiles.Count]);
            _hotelSmallCount++;
        }

        public async Task UploadSmallConferencePhoto(int confid, int idx)
        {
            var newBlob = _confRoomsContainer.GetBlockBlobReference($"{confid}_{idx}.png");
            await newBlob.UploadFromFileAsync(_confSmallFiles[_confSmallCount % _confSmallFiles.Count]);
            _confSmallCount++;
        }

        public async Task UploadFeaturedHotelPhoto(int id)
        {
            var newBlob = _hotelsContainer.GetBlockBlobReference(id.ToString() + "_featured.png");
            await newBlob.UploadFromFileAsync(_hotelSmallFiles[_hotelSmallCount % _hotelSmallFiles.Count]);
            _hotelSmallCount++;         // Featured photos are fetched from small photos collection
        }

        public async Task UploadFeaturedConferencePhoto(int id)
        {
            var newBlob = _confRoomsContainer.GetBlockBlobReference(id.ToString() + "_featured.png");
            await newBlob.UploadFromFileAsync(_confSmallFiles[_confSmallCount % _confSmallFiles.Count]);
            _confSmallCount++;         // Featured photos are fetched from small photos collection
        }

        public async Task UploadDefaultRoomPhoto(int id)
        {
            var newBlob = _roomsContainer.GetBlockBlobReference(id.ToString() + "_default.png");
            await newBlob.UploadFromFileAsync(_roomLargeFiles[_roomLargeCount % _roomLargeFiles.Count]);
            _roomLargeCount++;
        }

        public async Task UploadSuggestionsFrom(string path)
        {
            var suggestionsContainer = _blobClient.GetContainerReference("suggestions");
            await suggestionsContainer.CreateIfNotExistsAsync();

            var files = Directory.EnumerateFiles($"suggestions").OrderBy(x => x);
            foreach (var file in files) {
                var newBlob = suggestionsContainer.GetBlockBlobReference(Path.GetFileName(file));
                await newBlob.UploadFromFileAsync(file);
            }
        }

        public async Task UploadBotFrom(string path)
        {
            var suggestionsContainer = _blobClient.GetContainerReference("bot");
            await suggestionsContainer.CreateIfNotExistsAsync();

            var files = Directory.EnumerateFiles(path).OrderBy(x => x);
            foreach (var file in files)
            {
                var newBlob = suggestionsContainer.GetBlockBlobReference(Path.GetFileName(file));
                await newBlob.UploadFromFileAsync(file);
            }
        }

        public async Task UploadDefaultHotelPhoto(int id)
        {
            var newBlob = _hotelsContainer.GetBlockBlobReference(id.ToString() + "_default.png");
            await newBlob.UploadFromFileAsync(_hotelLargeFiles[_hotelLargeCount % _hotelLargeFiles.Count]);
            _hotelLargeCount++;
        }
    }
}