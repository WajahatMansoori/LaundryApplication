using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LaundryApplication.Models
{
    public class Password
    {
        public string Encrypt(string Password)
        {
            try
            {
                byte[] Pass = new byte[Password.Length];
                Pass = Encoding.UTF8.GetBytes(Password);
                string EncryptedData = Convert.ToBase64String(Pass);
                return EncryptedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Encode" + ex.Message);
            }
        }
        public string Decode(string EncryptedData)
        {
            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Decoder UTFDecoder = encoder.GetDecoder();
                byte[] DecodeByte = Convert.FromBase64String(EncryptedData);
                int CharCount = UTFDecoder.GetCharCount(DecodeByte, 0, DecodeByte.Length);
                char[] DecodeChar = new char[CharCount];
                
                //This line of code is not optional 
                UTFDecoder.GetChars(DecodeByte, 0, DecodeByte.Length, DecodeChar, 0);

                string DecryptedData = new string(DecodeChar);
                
                return DecryptedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Encode" + ex.Message);
            }
        }
    }
}