using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TrustFrontend
{
    public static class Photo
    {
        public static async Task<MediaFile> TakeFaceIDAsync()
        {
            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                return await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions()
                    {
                        SaveToAlbum = false
                    });
            }
            return null; 
        }
        public static byte[] GetByteRepresentationOfFaceID(MediaFile faceID)
        {
            byte[] faceIDData;
            using (FileStream fileStream = new FileStream(faceID.Path,
                FileMode.Open))
            {
                faceIDData = new byte[fileStream.Length];
                fileStream.Read(faceIDData, 0, (int)fileStream.Length);
            }
            return faceIDData;
        }
    }
}
