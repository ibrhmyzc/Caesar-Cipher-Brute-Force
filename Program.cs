using System;

namespace Cipher_Decoder
{
    class Program
    {

        static void Main(string[] args)
        {
            var decoder = new Decoder(args[0], args[1]);
            decoder.WriteDecodedDocument(args[2]);
        }
    }
}
