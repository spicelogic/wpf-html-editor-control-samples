using System;
using System.Drawing;

namespace CustomDialog
{
    public static class ImageUtils
    {
        /// <summary>
        /// Gets the image dimension.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        /// <returns>System.Nullable{Size}.</returns>
        public static Size? GetImageDimension(string absolutePath)
        {
            int fullSizeImageHeight;
            int fullSizeImageWidth;
            Image fullSizeImg = null;
            try
            {
                fullSizeImg = Image.FromFile(absolutePath);
                fullSizeImageHeight = fullSizeImg.Height;
                fullSizeImageWidth = fullSizeImg.Width;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (fullSizeImg != null)
                {
                    fullSizeImg.Dispose();
                    fullSizeImg = null;
                }
                GC.Collect();
            }

            return new Size(fullSizeImageWidth, fullSizeImageHeight);
        }
    }
}