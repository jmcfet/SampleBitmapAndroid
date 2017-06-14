using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Content.Res;

namespace loadBitmap
{
    [Activity(Label = "loadBitmap", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            TextView _originalDimensions = FindViewById<TextView>(Resource.Id.original_image_size);
            TextView _resizedDimensions = FindViewById<TextView>(Resource.Id.resized_image_size);

            ImageView _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            BitmapFactory.Options options = await GetBitmapOptionsOfImage();
            _originalDimensions.Text = string.Format("Original Size= {0}x{1}", options.OutHeight, options.OutWidth);
            Bitmap bitmapToDisplay = await LoadScaledDownBitmapForDisplayAsync(Resources, options, 150, 150);
            _imageView.SetImageBitmap(bitmapToDisplay);
            _resizedDimensions.Text = string.Format("Reduced the image {0}x", options.InSampleSize);
        }

        async Task<BitmapFactory.Options> GetBitmapOptionsOfImage()
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            // The result will be null because InJustDecodeBounds == true.  
            Bitmap result = await BitmapFactory.DecodeResourceAsync(Resources, Resource.Drawable.aspen, options);
            return options;

        }
        //If set to a value > 1, requests the decoder to subsample the original image, returning a smaller image to save memory.
        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;
            if (height > reqHeight || width > reqWidth)
                         
                inSampleSize *= 2;
            
            return (int)inSampleSize;
        }
        public async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(Resources res, BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);
            options.InJustDecodeBounds = false;
            return await BitmapFactory.DecodeResourceAsync(res, Resource.Drawable.aspen, options);
        }
        

    }
}

