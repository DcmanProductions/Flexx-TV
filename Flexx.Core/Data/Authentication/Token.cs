using System;

namespace Flexx.Core.Data.Authentication
{
    public class Token
    {
        public string GetToken { get; private set; }
        private Token(string _token)
        {
            GetToken = _token;
        }
        public static Token GenerateToken()
        {
            string token = "";
            char[] alphanumaric = ("qwertyuiopasdfghjklzxcvbnm" + "qwertyuiopasdfghjklzxcvbnm".ToUpper() + "0123456789").ToCharArray();
            Random ran = new Random();
            int blockSize = 6, blockCount = 4;
            for (int i = 0; i < blockCount; i++)
            {
                string block = "";
                for (int j = 0; j < blockSize; j++)
                {
                    block += alphanumaric[ran.Next(alphanumaric.Length)];
                }
                token += i == (blockCount - 1) ? block : block + "-";
            }
            return new Token(token);
        }
    }
}
