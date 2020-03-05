using System;
using Sodium;

namespace Viato.Api.Tor
{
    public class ECKey
    {
        private byte[] _privateKey;
        private byte[] _publicKey;

        public ECKey()
        {
            var keys = PublicKeyAuth.GenerateKeyPair();
            SetPublicKey(keys.PublicKey);
            SetPrivateKey(keys.PrivateKey);
        }

        public ECKey(byte[] pubKey, byte[] privKey)
        {
            SetPublicKey(pubKey ?? throw new ArgumentNullException(nameof(pubKey)));
            SetPrivateKey(privKey ?? throw new ArgumentNullException(nameof(privKey)));
        }

        public static ECKey FromPrivateKey(byte[] privateKey)
        {
            var pubKey = PublicKeyAuth.ExtractEd25519PublicKeyFromEd25519SecretKey(privateKey);
            return new ECKey(pubKey, privateKey);
        }

        public static bool Verify(byte[] msg, byte[] signature, byte[] pubKey)
        {
            return PublicKeyAuth.VerifyDetached(signature, msg, pubKey);
        }

        public byte[] GetPrivateKey()
        {
            return _privateKey;
        }

        public byte[] GetPublicKey()
        {
            return _publicKey;
        }

        public byte[] Sign(byte[] msg)
        {
            return PublicKeyAuth.SignDetached(msg, GetPrivateKey());
        }

        public bool Verify(byte[] msg, byte[] signature)
        {
            return Verify(msg, signature, GetPublicKey());
        }

        private void SetPrivateKey(byte[] value)
        {
            _privateKey = value;
        }

        private void SetPublicKey(byte[] value)
        {
            _publicKey = value;
        }
    }
}
