using Firebase.Storage;
using G3_TrainBookingSytem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3_TrainBookingSystem
{
    public class FirebaseStorageHelper
    {
        FirebaseStorage firebaseStorage;

        public FirebaseStorageHelper()
        {
            // Use the Firebase Cloud Storage URL from GlobalData
            firebaseStorage = new FirebaseStorage(GlobalData.firebaseStorage);
        }

        public async Task<string> UploadFile(Stream fileStream, string folderName, string fileName)
        {
            var fileUrl = await firebaseStorage
                .Child(folderName)
                .Child(fileName)
                .PutAsync(fileStream);
            return fileUrl;
        }

        public async Task<string> GetFileUrl(string folderName, string fileName)
        {
            return await firebaseStorage
                .Child(folderName)
                .Child(fileName)
                .GetDownloadUrlAsync();
        }

        public async Task DeleteFile(string folderName, string fileName)
        {
            await firebaseStorage
                 .Child(folderName)
                 .Child(fileName)
                 .DeleteAsync();
        }
    }
}
