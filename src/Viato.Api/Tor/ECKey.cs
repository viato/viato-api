using Sodium;
using System;

namespace Viato.Api.Tor
{
    public class ECKey
    {
        public ECKey()
        {
            var keys = PublicKeyAuth.GenerateKeyPair();
            PublicKey = keys.PublicKey;
            PrivateKey = keys.PrivateKey;
        }

        public ECKey(byte[] pubKey, byte[] privKey)
        {
            PublicKey = pubKey ?? throw new ArgumentNullException(nameof(pubKey));
            PrivateKey = privKey ?? throw new ArgumentNullException(nameof(privKey));
        }

        public byte[] PrivateKey { get; private set; }

        public byte[] PublicKey { get; private set; }

        public static ECKey FromPrivateKey(byte[] privateKey)
        {
            var pubKey = PublicKeyAuth.ExtractEd25519PublicKeyFromEd25519SecretKey(privateKey);
            return new ECKey(pubKey, privateKey);
        }

        public static bool Verify(byte[] msg, byte[] signature, byte[] pubKey)
        {
            return PublicKeyAuth.VerifyDetached(signature, msg, pubKey);
        }

        public byte[] Sign(byte[] msg)
        {
            return PublicKeyAuth.SignDetached(msg, PrivateKey);
        }

        public bool Verify(byte[] msg, byte[] signature)
        {
            return Verify(msg, signature, PublicKey);
        }
    }
}
