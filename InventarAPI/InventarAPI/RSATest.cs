using System;
using System.Collections.Generic;
using System.Text;

namespace InventarAPI
{
    class RSATest
    {
        private RSA serverEncryptRSA, serverDecryptRSA;
        private RSA clientEncryptRSA, clientDecryptRSA;

        public void Test()
        {
            generateRSA();
            string serverPublicKey = serverDecryptRSA.PublicKey;
            
            clientEncryptRSA = new RSA(serverPublicKey);
            string clientPublicKey = clientDecryptRSA.PublicKey;
            serverEncryptRSA = new RSA(clientPublicKey);

            ASCIIEncoding en = new ASCIIEncoding();
            byte[] data = en.GetBytes("Fuck You!");

            byte[] encrypted = serverEncryptRSA.Encrypt(data);
            byte[] decrypted = clientDecryptRSA.Decrypt(encrypted);

            Console.WriteLine(en.GetString(decrypted));

            data = en.GetBytes("You Too!");

            encrypted = clientEncryptRSA.Encrypt(data);
            decrypted = serverDecryptRSA.Decrypt(encrypted);

            Console.WriteLine(en.GetString(decrypted));
        }

        private void generateRSA()
        {
            serverDecryptRSA = new RSA();
            clientDecryptRSA = new RSA();
        }
    }
}
