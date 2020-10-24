using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.IO;
namespace ZPD_Lab_1_7.Tests
{
    public class LSBlgorithmTest
    {
        [Theory]
        [InlineData("Rojyou Sutangan no Dengeki ga Utsu Gunsyuu no Kage Yai Yai to Hitowa Yuki Himitsuri ni Koto wa Naru",
            "coffee_zone.bmp")]
        [InlineData("Yai Yai to Huminarashi Itsudatsu no Tami wo Uteto Kikeyo Madobede Yokikoto no Tame to Renko Suru",
            "coffee_zone.bmp")]
        [InlineData("Yai Yai to Hito Hito Hito no Mega Kimiwo Ou Yai Yai to Hito Hito Hito no Mega Kimiwo Miru",
            "coffee_zone.bmp")]
        public void EncryptAndDecrypt_FileURIAndMessage_ShouldReturnOriginalMessage(string message, string imageURI)
        {
            LSBAlgorithm algorithm = new LSBAlgorithm();
            string path = Path.Combine(@"E:\ZPD\ZPD-Lab-1-7\", imageURI);
            algorithm.Encrypt(path, message);
            string newPath = Path.Combine(@"E:\ZPD\ZPD-Lab-1-7\",
                imageURI.Split(".")[0] + "_encrypted.bmp");
            string decryptedMessage = algorithm.Decrypt(
                path,
                newPath);
            Assert.Equal(message, decryptedMessage);
        }
    }
}
