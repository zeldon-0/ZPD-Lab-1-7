using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ZPD_Lab_1_7
{
    public class AdditiveAlgorithm
    {
        public AdditiveAlgorithm() { }
        private int[] _lCoefficients = { 1, 20, 3, 2, 10, 4, 1, 2, 100 };
        public void Encrypt(string imageURI, string message)
        {
            Bitmap bitmap = new Bitmap(imageURI);
            int gap = GetGapBetweenPixels(bitmap, message);
            int bitmapColumns = bitmap.Width;
            int bitmapRows = bitmap.Height;
            for (int i = 0; i < message.Length; i++)
            {
                int index = i * (gap + 1);
                int row = index / bitmapColumns;
                int column;
                if(row % 2 ==0)
                {
                    column = index % bitmapColumns;
                }
                else
                {
                    column = bitmapColumns - 1 - index % bitmapColumns;
                }
                Color pixel = bitmap.GetPixel(column, row);
                Color modifiedPixel = ApplyAdditiveFunction(pixel, message[i], i);
                bitmap.SetPixel(column, row, modifiedPixel);
                Color newPixel = bitmap.GetPixel(column, row);
                int a = 10;
            }
            bitmap.Save(imageURI.Split(".")[0] + "_encrypted.bmp");
            return;
        }

        public string Decrypt(string originalImageURI, string encryptedImageURI)
        {
            Bitmap originalImage = new Bitmap(originalImageURI);
            Bitmap encryptedImage = new Bitmap(encryptedImageURI);
            StringBuilder stringBuilder = new StringBuilder();
            int charCounter = 0;

            for (int y = 0; y < originalImage.Height; y++)
            {
                if (y % 2 == 0)
                {
                    for (int x = 0; x < originalImage.Width; x++)
                    {
                        Color originalColor = originalImage.GetPixel(x, y);
                        Color modifiedColor = encryptedImage.GetPixel(x, y);
                        int originalRGBValue = originalColor.ToArgb();
                        int modifiedRGBValue = modifiedColor.ToArgb();
                        if (originalRGBValue != modifiedRGBValue)
                        {
                            stringBuilder.Append(
                                RestoreMessageFromColors(originalColor, modifiedColor, charCounter));
                            charCounter++;
                        }
                    }
                }
                else 
                {
                    for (int x = originalImage.Width - 1; x >= 0; x--)
                    {
                        Color originalColor = originalImage.GetPixel(x, y);
                        Color modifiedColor = encryptedImage.GetPixel(x, y);
                        int originalRGBValue = originalColor.ToArgb();
                        int modifiedRGBValue = modifiedColor.ToArgb();
                        if (originalRGBValue != modifiedRGBValue)
                        {
                            stringBuilder.Append(
                                RestoreMessageFromColors(originalColor, modifiedColor, charCounter));
                            charCounter++;
                        }
                    }
                }
            }
            string decryptedMessage = stringBuilder.ToString();
            return decryptedMessage;
        }

        private Color ApplyAdditiveFunction(Color color, char word, int i)
        {
            int rgbValue = color.ToArgb();
            rgbValue = rgbValue + _lCoefficients[i % _lCoefficients.Length] * word;
            Color newColor = Color.FromArgb(rgbValue);
            return newColor;
        }

        private char RestoreMessageFromColors( Color originalColor, Color modifiedColor, int i)
        {
            int originalRGBValue = originalColor.ToArgb();
            int modifiedRGBValue = modifiedColor.ToArgb();
            char word = (char) 
                ((modifiedRGBValue - originalRGBValue) / _lCoefficients[i % _lCoefficients.Length]);
            return word;
        }

        private int GetGapBetweenPixels(Bitmap bitmap, string message)
        {
            int messageLength = message.Length;
            int bitmapSize = bitmap.Width * bitmap.Height;
            int gapSize = (bitmapSize - messageLength) / (messageLength - 1);
            return gapSize;
        }
    }
}
