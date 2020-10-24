using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ZPD_Lab_1_7
{
    public class LSBAlgorithm
    {
        public LSBAlgorithm() { }
        public void Encrypt(string imageURI, string message)
        {
            Bitmap bitmap = new Bitmap(imageURI);
            int bitmapColumns = bitmap.Width;
            BitArray messageBits = GetBitsFromMessage(message);
            for (int i = 0; i < messageBits.Length / 2; i++)
            {
                int row = i / bitmapColumns;
                int column = i % bitmapColumns;

                Color pixel = bitmap.GetPixel(column, row);
                Color modifiedPixel = ApplyLSBTransformation(pixel, messageBits, i);
                bitmap.SetPixel(column, row, modifiedPixel);

            }
            bitmap.Save(imageURI.Split(".")[0] + "_encrypted.bmp");
            return;
        }

        public string Decrypt(string originalImageURI, string encryptedImageURI)
        {
            Bitmap originalImage = new Bitmap(originalImageURI);
            Bitmap encryptedImage = new Bitmap(encryptedImageURI);
            List<bool> bits = new List<bool>();

            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color originalColor = originalImage.GetPixel(x, y);
                    Color modifiedColor = encryptedImage.GetPixel(x, y);
                    int originalRGBValue = originalColor.ToArgb();
                    int modifiedRGBValue = modifiedColor.ToArgb();
                    if (originalRGBValue != modifiedRGBValue)
                    {
                        (bool firstBit, bool SecondBit) =
                            RestoreMessageFromColors(modifiedColor);
                        bits.Add(firstBit);
                        bits.Add(SecondBit);
                    }
                    else 
                    {
                        break;
                    }
                }
            }

            string decryptedMessage = GetMessageFromBitArray(bits);
            return decryptedMessage;
        }

        private Color ApplyLSBTransformation(Color color, BitArray bits, int i)
        {
            int rgbValue = color.ToArgb();
            int leastSignificatntBits = 0;
            if (bits[2 * i])
            {
                leastSignificatntBits += 1;
            }
            if (bits[2 * i + 1])
            {
                leastSignificatntBits += 2;
            }
            rgbValue = rgbValue & 0xFFFC;
            rgbValue = rgbValue | leastSignificatntBits;
            Color newColor = Color.FromArgb(rgbValue);
            return newColor;
        }

        private (bool, bool) RestoreMessageFromColors(Color modifiedColor)
        {
            int modifiedRGBValue = modifiedColor.ToArgb();
            int leastSignificantBits = modifiedRGBValue & 0x0003;
            bool firstBit = false; 
            if((leastSignificantBits & 0x0001) != 0)
            {
                firstBit = true;
            }
            bool secondBit = false;
            if ((leastSignificantBits & 0x0002) != 0)
            {
                secondBit = true;
            }
            return (firstBit, secondBit);
        }

        private BitArray GetBitsFromMessage(string message)
        {
            byte[] messageBytes = message
                .ToCharArray()
                .Select(c => (byte)c)
                .ToArray();
            BitArray bitArray = new BitArray(messageBytes);
            return bitArray;
        }

        private string GetMessageFromBitArray(IEnumerable<bool> bits)
        {

            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < bits.Count(); i += 8)
            {
                bool[] byteBits = bits.Skip(i)
                    .Take(8)
                    .ToArray();
                byte charValue = 0;
                for(int j = 0; j < 8; j++)
                {
                    if(byteBits[j])
                    {
                        charValue += (byte) (1 << j);
                    }
                }
                stringBuilder.Append((char) charValue);
            }
            return stringBuilder.ToString();
        }
    }
}

