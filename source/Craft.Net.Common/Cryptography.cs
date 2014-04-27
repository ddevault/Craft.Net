using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using Org.BouncyCastle.Math;

namespace Craft.Net.Common
{
    public static class Cryptography
    {
        /// <summary>
        /// Creates a Java-style SHA-1 hash.
        /// </summary>
        public static string JavaHexDigest(byte[] data)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(data);
            bool negative = (hash[0] & 0x80) == 0x80;
            if (negative) // check for negative hashes
                hash = TwosCompliment(hash);
            // Create the string and trim away the zeroes
            string digest = GetHexString(hash).TrimStart('0');
            if (negative)
                digest = "-" + digest;
            return digest;
        }

        private static string GetHexString(byte[] p)
        {
            string result = "";
            for (int i = 0; i < p.Length; i++)
                result += p[i].ToString("X2");
            return result;
        }

        private static byte[] TwosCompliment(byte[] p) // little endian
        {
            int i;
            bool carry = true;
            for (i = p.Length - 1; i >= 0; i--)
            {
                p[i] = unchecked((byte)~p[i]);
                if (carry)
                {
                    carry = p[i] == 0xFF;
                    p[i]++;
                }
            }
            return p;
        }
    }

    public class AsnKeyBuilder
    {
        private static readonly byte[] Zero = new byte[] {0};
        private static readonly byte[] Empty = new byte[] {};

        // PublicKeyInfo (X.509 compatible) message
        /// <summary>
        /// Returns the AsnMessage representing the X.509 PublicKeyInfo.
        /// </summary>
        /// <param name="publicKey">The DSA key to be encoded.</param>
        /// <returns>Returns the AsnType representing the
        /// X.509 PublicKeyInfo.</returns>
        /// <seealso cref="PrivateKeyToPKCS8(DSAParameters)"/>
        /// <seealso cref="PrivateKeyToPKCS8(RSAParameters)"/>
        /// <seealso cref="PublicKeyToX509(RSAParameters)"/>
        public static AsnMessage PublicKeyToX509(DSAParameters publicKey)
        {
            // Value Type cannot be null
            // Debug.Assert(null != publicKey);

            /* *
            * SEQUENCE              // PrivateKeyInfo
            * +- SEQUENCE           // AlgorithmIdentifier
            * |  +- OID             // 1.2.840.10040.4.1
            * |  +- SEQUENCE        // DSS-Params (Optional Parameters)
            * |    +- INTEGER (P)
            * |    +- INTEGER (Q)
            * |    +- INTEGER (G)
            * +- BITSTRING          // PublicKey
            *    +- INTEGER(Y)      // DSAPublicKey Y
            * */

            // DSA Parameters
            AsnType p = CreateIntegerPos(publicKey.P);
            AsnType q = CreateIntegerPos(publicKey.Q);
            AsnType g = CreateIntegerPos(publicKey.G);

            // Sequence - DSA-Params
            AsnType dssParams = CreateSequence(new[] {p, q, g});

            // OID - packed 1.2.840.10040.4.1
            //   { 0x2A, 0x86, 0x48, 0xCE, 0x38, 0x04, 0x01 }
            AsnType oid = CreateOid("1.2.840.10040.4.1");

            // Sequence
            AsnType algorithmID = CreateSequence(new[] {oid, dssParams});

            // Public Key Y
            AsnType y = CreateIntegerPos(publicKey.Y);
            AsnType key = CreateBitString(y);

            // Sequence 'A'
            AsnType publicKeyInfo =
                CreateSequence(new[] {algorithmID, key});

            return new AsnMessage(publicKeyInfo.GetBytes(), "X.509");
        }

        // PublicKeyInfo (X.509 compatible) message
        /// <summary>
        /// Returns the AsnMessage representing the X.509 PublicKeyInfo.
        /// </summary>
        /// <param name="publicKey">The RSA key to be encoded.</param>
        /// <returns>Returns the AsnType representing the
        /// X.509 PublicKeyInfo.</returns>
        /// <seealso cref="PrivateKeyToPKCS8(DSAParameters)"/>
        /// <seealso cref="PrivateKeyToPKCS8(RSAParameters)"/>
        /// <seealso cref="PublicKeyToX509(DSAParameters)"/>
        public static AsnMessage PublicKeyToX509(RSAParameters publicKey)
        {
            // Value Type cannot be null
            // Debug.Assert(null != publicKey);

            /* *
            * SEQUENCE              // PrivateKeyInfo
            * +- SEQUENCE           // AlgorithmIdentifier
            *    +- OID             // 1.2.840.113549.1.1.1
            *    +- Null            // Optional Parameters
            * +- BITSTRING          // PrivateKey
            *    +- SEQUENCE        // RSAPrivateKey
            *       +- INTEGER(N)   // N
            *       +- INTEGER(E)   // E
            * */

            // OID - packed 1.2.840.113549.1.1.1
            //   { 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 }
            AsnType oid = CreateOid("1.2.840.113549.1.1.1");
            AsnType algorithmID =
                CreateSequence(new[] {oid, CreateNull()});

            AsnType n = CreateIntegerPos(publicKey.Modulus);
            AsnType e = CreateIntegerPos(publicKey.Exponent);
            AsnType key = CreateBitString(
                CreateSequence(new[] {n, e})
                );

            AsnType publicKeyInfo =
                CreateSequence(new[] {algorithmID, key});

            return new AsnMessage(publicKeyInfo.GetBytes(), "X.509");
        }

        // PKCS #8, Section 6 (PrivateKeyInfo) message
        // !!!!!!!!!!!!!!! Unencrypted !!!!!!!!!!!!!!!
        /// <summary>
        /// Returns AsnMessage representing the unencrypted
        /// PKCS #8 PrivateKeyInfo.
        /// </summary>
        /// <param name="privateKey">The DSA key to be encoded.</param>
        /// <returns>Returns the AsnType representing the unencrypted
        /// PKCS #8 PrivateKeyInfo.</returns>
        /// <seealso cref="PrivateKeyToPKCS8(RSAParameters)"/>
        /// <seealso cref="PublicKeyToX509(DSAParameters)"/>
        /// <seealso cref="PublicKeyToX509(RSAParameters)"/>
        public static AsnMessage PrivateKeyToPKCS8(DSAParameters privateKey)
        {
            // Value Type cannot be null
            // Debug.Assert(null != privateKey);

            /* *
            * SEQUENCE              // PrivateKeyInfo
            * +- INTEGER(0)         // Version (v1998)
            * +- SEQUENCE           // AlgorithmIdentifier
            * |  +- OID             // 1.2.840.10040.4.1
            * |  +- SEQUENCE        // DSS-Params (Optional Parameters)
            * |    +- INTEGER (P)
            * |    +- INTEGER (Q)
            * |    +- INTEGER (G)
            * +- OCTETSTRING        // PrivateKey
            *    +- INTEGER(X)   // DSAPrivateKey X
            * */

            // Version - 0 (v1998)
            AsnType version = CreateInteger(Zero);

            // Domain Parameters
            AsnType p = CreateIntegerPos(privateKey.P);
            AsnType q = CreateIntegerPos(privateKey.Q);
            AsnType g = CreateIntegerPos(privateKey.G);

            AsnType dssParams = CreateSequence(new[] {p, q, g});

            // OID - packed 1.2.840.10040.4.1
            //   { 0x2A, 0x86, 0x48, 0xCE, 0x38, 0x04, 0x01 }
            AsnType oid = CreateOid("1.2.840.10040.4.1");

            // AlgorithmIdentifier
            AsnType algorithmID = CreateSequence(new[] {oid, dssParams});

            // Private Key X
            AsnType x = CreateIntegerPos(privateKey.X);
            AsnType key = CreateOctetString(x);

            // Sequence
            AsnType privateKeyInfo =
                CreateSequence(new[] {version, algorithmID, key});

            return new AsnMessage(privateKeyInfo.GetBytes(), "PKCS#8");
        }

        // PKCS #8, Section 6 (PrivateKeyInfo) message
        // !!!!!!!!!!!!!!! Unencrypted !!!!!!!!!!!!!!!
        /// <summary>
        /// Returns AsnMessage representing the unencrypted
        /// PKCS #8 PrivateKeyInfo.
        /// </summary>
        /// <param name="privateKey">The RSA key to be encoded.</param>
        /// <returns>Returns the AsnType representing the unencrypted
        /// PKCS #8 PrivateKeyInfo.</returns>
        /// <seealso cref="PrivateKeyToPKCS8(DSAParameters)"/>
        /// <seealso cref="PublicKeyToX509(DSAParameters)"/>
        /// <seealso cref="PublicKeyToX509(RSAParameters)"/>
        public static AsnMessage PrivateKeyToPKCS8(RSAParameters privateKey)
        {
            // Value Type cannot be null
            // Debug.Assert(null != privateKey);

            /* *
            * SEQUENCE                  // PublicKeyInfo
            * +- INTEGER(0)             // Version - 0 (v1998)
            * +- SEQUENCE               // AlgorithmIdentifier
            *    +- OID                 // 1.2.840.113549.1.1.1
            *    +- NULL                // Optional Parameters
            * +- OCTETSTRING            // PrivateKey
            *    +- SEQUENCE            // RSAPrivateKey
            *       +- INTEGER(0)       // Version - 0 (v1998)
            *       +- INTEGER(N)
            *       +- INTEGER(E)
            *       +- INTEGER(D)
            *       +- INTEGER(P)
            *       +- INTEGER(Q)
            *       +- INTEGER(DP)
            *       +- INTEGER(DQ)
            *       +- INTEGER(Inv Q)
            * */

            AsnType n = CreateIntegerPos(privateKey.Modulus);
            AsnType e = CreateIntegerPos(privateKey.Exponent);
            AsnType d = CreateIntegerPos(privateKey.D);
            AsnType p = CreateIntegerPos(privateKey.P);
            AsnType q = CreateIntegerPos(privateKey.Q);
            AsnType dp = CreateIntegerPos(privateKey.DP);
            AsnType dq = CreateIntegerPos(privateKey.DQ);
            AsnType iq = CreateIntegerPos(privateKey.InverseQ);

            // Version - 0 (v1998)
            AsnType version = CreateInteger(new byte[] {0});

            // octstring = OCTETSTRING(SEQUENCE(INTEGER(0)INTEGER(N)...))
            AsnType key = CreateOctetString(
                CreateSequence(new[] {version, n, e, d, p, q, dp, dq, iq})
                );

            // OID - packed 1.2.840.113549.1.1.1
            //   { 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 }
            AsnType algorithmID = CreateSequence(new[] {CreateOid("1.2.840.113549.1.1.1"), CreateNull()}
                );

            // PrivateKeyInfo
            AsnType privateKeyInfo =
                CreateSequence(new[] {version, algorithmID, key});

            return new AsnMessage(privateKeyInfo.GetBytes(), "PKCS#8");
        }

        /// <summary>
        /// <para>An ordered collection of one or more types.
        /// Returns the AsnType representing an ASN.1 encoded sequence.</para>
        /// <para>If the AsnType is null, an empty sequence (length 0)
        /// is returned.</para>
        /// </summary>
        /// <param name="value">An AsnType consisting of
        /// a single value to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded sequence.</returns>
        /// <seealso cref="CreateSet(AsnType)"/>
        /// <seealso cref="CreateSet(AsnType[])"/> 
        /// <seealso cref="CreateSetOf(AsnType)"/>
        /// <seealso cref="CreateSetOf(AsnType[])"/>
        /// <seealso cref="CreateSequence(AsnType)"/>
        /// <seealso cref="CreateSequence(AsnType[])"/>
        /// <seealso cref="CreateSequenceOf(AsnType)"/>
        /// <seealso cref="CreateSequenceOf(AsnType[])"/>
        public static AsnType CreateSequence(AsnType value)
        {
            // Should be at least 1...
            Debug.Assert(!IsEmpty(value));

            // One or more required
            if (IsEmpty(value))
            {
                throw new ArgumentException("A sequence requires at least one value.");
            }

            // Sequence: Tag 0x30 (16, Universal, Constructed)
            return new AsnType(0x30, value.GetBytes());
        }

        /// <summary>
        /// <para>An ordered collection of one or more types.
        /// Returns the AsnType representing an ASN.1 encoded sequence.</para>
        /// <para>If the AsnType is null, an
        /// empty sequence (length 0) is returned.</para>
        /// </summary>
        /// <param name="values">An array of AsnType consisting of
        /// the values in the collection to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded Set.</returns>
        /// <seealso cref="CreateSet(AsnType)"/>
        /// <seealso cref="CreateSet(AsnType[])"/> 
        /// <seealso cref="CreateSetOf(AsnType)"/>
        /// <seealso cref="CreateSetOf(AsnType[])"/>
        /// <seealso cref="CreateSequence(AsnType)"/>
        /// <seealso cref="CreateSequence(AsnType[])"/>
        /// <seealso cref="CreateSequenceOf(AsnType)"/>
        /// <seealso cref="CreateSequenceOf(AsnType[])"/>
        public static AsnType CreateSequence(AsnType[] values)
        {
            // Should be at least 1...
            Debug.Assert(!IsEmpty(values));

            // One or more required
            if (IsEmpty(values))
            {
                throw new ArgumentException("A sequence requires at least one value.");
            }

            // Sequence: Tag 0x30 (16, Universal, Constructed)
            return new AsnType((0x10 | 0x20), Concatenate(values));
        }

        /// <summary>
        /// <para>An ordered collection zero, one or more types.
        /// Returns the AsnType representing an ASN.1 encoded sequence.</para>
        /// <para>If the AsnType value is null,an
        /// empty sequence (length 0) is returned.</para>
        /// </summary>
        /// <param name="value">An AsnType consisting of
        /// a single value to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded sequence.</returns>
        /// <seealso cref="CreateSet(AsnType)"/>
        /// <seealso cref="CreateSet(AsnType[])"/> 
        /// <seealso cref="CreateSetOf(AsnType)"/>
        /// <seealso cref="CreateSetOf(AsnType[])"/>
        /// <seealso cref="CreateSequence(AsnType)"/>
        /// <seealso cref="CreateSequence(AsnType[])"/>
        /// <seealso cref="CreateSequenceOf(AsnType)"/>
        /// <seealso cref="CreateSequenceOf(AsnType[])"/>
        public static AsnType CreateSequenceOf(AsnType value)
        {
            // From the ASN.1 Mailing List
            if (IsEmpty(value))
            {
                return new AsnType(0x30, Empty);
            }

            // Sequence: Tag 0x30 (16, Universal, Constructed)
            return new AsnType(0x30, value.GetBytes());
        }

        /// <summary>
        /// <para>An ordered collection zero, one or more types.
        /// Returns the AsnType representing an ASN.1 encoded sequence.</para>
        /// <para>If the AsnType array is null or the array is 0 length,
        /// an empty sequence (length 0) is returned.</para>
        /// </summary>
        /// <param name="values">An AsnType consisting of
        /// the values in the collection to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded sequence.</returns>
        /// <seealso cref="CreateSet(AsnType)"/>
        /// <seealso cref="CreateSet(AsnType[])"/> 
        /// <seealso cref="CreateSetOf(AsnType)"/>
        /// <seealso cref="CreateSetOf(AsnType[])"/>
        /// <seealso cref="CreateSequence(AsnType)"/>
        /// <seealso cref="CreateSequence(AsnType[])"/>
        /// <seealso cref="CreateSequenceOf(AsnType)"/>
        /// <seealso cref="CreateSequenceOf(AsnType[])"/>
        public static AsnType CreateSequenceOf(AsnType[] values)
        {
            // From the ASN.1 Mailing List
            if (IsEmpty(values))
            {
                return new AsnType(0x30, Empty);
            }

            // Sequence: Tag 0x30 (16, Universal, Constructed)
            return new AsnType(0x30, Concatenate(values));
        }

        /// <summary>
        /// <para>An ordered sequence of zero, one or more bits. Returns
        /// the AsnType representing an ASN.1 encoded bit string.</para>
        /// <para>If octets is null or length is 0, an empty (0 length)
        /// bit string is returned.</para>
        /// </summary>
        /// <param name="octets">A MSB (big endian) byte[] representing the
        /// bit string to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded bit string.</returns>
        /// <seealso cref="CreateBitString(byte[], uint)"/>
        /// <seealso cref="CreateBitString(AsnType)"/>
        /// <seealso cref="CreateBitString(AsnType[])"/>
        /// <seealso cref="CreateBitString(String)"/>
        /// <seealso cref="CreateOctetString(byte[])"/>
        /// <seealso cref="CreateOctetString(AsnType)"/>
        /// <seealso cref="CreateOctetString(AsnType[])"/>
        /// <seealso cref="CreateOctetString(String)"/>
        public static AsnType CreateBitString(byte[] octets)
        {
            // BitString: Tag 0x03 (3, Universal, Primitive)
            return CreateBitString(octets, 0);
        }

        /// <summary>
        /// <para>An ordered sequence of zero, one or more bits. Returns
        /// the AsnType representing an ASN.1 encoded bit string.</para>
        /// <para>unusedBits is applied to the end of the bit string,
        /// not the start of the bit string. unusedBits must be less than 8
        /// (the size of an octet). Refer to ITU X.680, Section 32.</para>
        /// <para>If octets is null or length is 0, an empty (0 length)
        /// bit string is returned.</para>
        /// </summary>
        /// <param name="octets">A MSB (big endian) byte[] representing the
        /// bit string to be encoded.</param>
        /// <param name="unusedBits">The number of unused trailing binary
        /// digits in the bit string to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded bit string.</returns>
        /// <seealso cref="CreateBitString(byte[])"/>
        /// <seealso cref="CreateBitString(AsnType)"/>
        /// <seealso cref="CreateBitString(AsnType[])"/>
        /// <seealso cref="CreateBitString(String)"/>
        /// <seealso cref="CreateOctetString(byte[])"/>
        /// <seealso cref="CreateOctetString(AsnType)"/>
        /// <seealso cref="CreateOctetString(AsnType[])"/>
        /// <seealso cref="CreateOctetString(String)"/>
        public static AsnType CreateBitString(byte[] octets, uint unusedBits)
        {
            if (IsEmpty(octets))
            {
                // Empty octet string
                return new AsnType(0x03, Empty);
            }

            if (!(unusedBits < 8))
            {
                throw new ArgumentException("Unused bits must be less than 8.");
            }

            byte[] b = Concatenate(new[] {(byte)unusedBits}, octets);
            // BitString: Tag 0x03 (3, Universal, Primitive)
            return new AsnType(0x03, b);
        }

        /// <summary>
        /// An ordered sequence of zero, one or more bits. Returns
        /// the AsnType representing an ASN.1 encoded bit string.
        /// If value is null, an empty (0 length) bit string is
        /// returned.
        /// </summary>
        /// <param name="value">An AsnType object to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded bit string.</returns>
        /// <seealso cref="CreateBitString(byte[])"/>
        /// <seealso cref="CreateBitString(byte[], uint)"/>
        /// <seealso cref="CreateBitString(AsnType[])"/>
        /// <seealso cref="CreateBitString(String)"/>
        /// <seealso cref="CreateOctetString(byte[])"/>
        /// <seealso cref="CreateOctetString(AsnType)"/>
        /// <seealso cref="CreateOctetString(AsnType[])"/>
        /// <seealso cref="CreateOctetString(String)"/>
        public static AsnType CreateBitString(AsnType value)
        {
            if (IsEmpty(value))
            {
                return new AsnType(0x03, Empty);
            }

            // BitString: Tag 0x03 (3, Universal, Primitive)
            return CreateBitString(value.GetBytes(), 0x00);
        }

        /// <summary>
        /// An ordered sequence of zero, one or more bits. Returns
        /// the AsnType representing an ASN.1 encoded bit string.
        /// If value is null, an empty (0 length) bit string is
        /// returned.
        /// </summary>
        /// <param name="values">An AsnType object to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded bit string.</returns>
        /// <seealso cref="CreateBitString(byte[])"/>
        /// <seealso cref="CreateBitString(byte[], uint)"/>
        /// <seealso cref="CreateBitString(AsnType)"/>
        /// <seealso cref="CreateBitString(String)"/>
        /// <seealso cref="CreateOctetString(byte[])"/>
        /// <seealso cref="CreateOctetString(AsnType)"/>
        /// <seealso cref="CreateOctetString(AsnType[])"/>
        /// <seealso cref="CreateOctetString(String)"/>
        public static AsnType CreateBitString(AsnType[] values)
        {
            if (IsEmpty(values))
            {
                return new AsnType(0x03, Empty);
            }

            // BitString: Tag 0x03 (3, Universal, Primitive)
            return CreateBitString(Concatenate(values), 0x00);
        }

        /// <summary>
        /// <para>An ordered sequence of zero, one or more bits. Returns
        /// the AsnType representing an ASN.1 encoded bit string.</para>
        /// <para>If octets is null or length is 0, an empty (0 length)
        /// bit string is returned.</para>
        /// <para>If conversion fails, the bit string returned is a partial
        /// bit string. The partial bit string ends at the octet before the
        /// point of failure (it does not include the octet which could
        /// not be parsed, or subsequent octets).</para>
        /// </summary>
        /// <param name="value">A MSB (big endian) byte[] representing the
        /// bit string to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded bit string.</returns>
        /// <seealso cref="CreateBitString(byte[])"/>
        /// <seealso cref="CreateBitString(byte[], uint)"/>
        /// <seealso cref="CreateBitString(AsnType)"/>
        /// <seealso cref="CreateOctetString(byte[])"/>
        /// <seealso cref="CreateOctetString(AsnType)"/>
        /// <seealso cref="CreateOctetString(AsnType[])"/>
        /// <seealso cref="CreateOctetString(String)"/>
        public static AsnType CreateBitString(String value)
        {
            if (IsEmpty(value))
            {
                return CreateBitString(Empty);
            }

            // Any unused bits?
            int lstrlen = value.Length;
            int unusedBits = 8 - (lstrlen%8);
            if (8 == unusedBits)
            {
                unusedBits = 0;
            }

            for (int i = 0; i < unusedBits; i++)
            {
                value += "0";
            }

            // Determine number of octets
            int loctlen = (lstrlen + 7)/8;

            var octets = new List<byte>();
            for (int i = 0; i < loctlen; i++)
            {
                String s = value.Substring(i*8, 8);
                byte b = 0x00;

                try
                {
                    b = Convert.ToByte(s, 2);
                }

                catch (FormatException /*e*/)
                {
                    unusedBits = 0;
                    break;
                }
                catch (OverflowException /*e*/)
                {
                    unusedBits = 0;
                    break;
                }

                octets.Add(b);
            }

            // BitString: Tag 0x03 (3, Universal, Primitive)
            return CreateBitString(octets.ToArray(), (uint)unusedBits);
        }

        /// <summary>
        /// An ordered sequence of zero, one or more octets. Returns
        /// the ASN.1 encoded octet string. If octets is null or length
        /// is 0, an empty (0 length) octet string is returned.
        /// </summary>
        /// <param name="value">A MSB (big endian) byte[] representing the
        /// octet string to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded octet string.</returns>
        /// <seealso cref="CreateBitString(byte[])"/>
        /// <seealso cref="CreateBitString(byte[], uint)"/>
        /// <seealso cref="CreateBitString(AsnType)"/>
        /// <seealso cref="CreateBitString(String)"/>
        /// <seealso cref="CreateOctetString(AsnType)"/>
        /// <seealso cref="CreateOctetString(AsnType[])"/>
        /// <seealso cref="CreateOctetString(String)"/>
        public static AsnType CreateOctetString(byte[] value)
        {
            if (IsEmpty(value))
            {
                // Empty octet string
                return new AsnType(0x04, Empty);
            }

            // OctetString: Tag 0x04 (4, Universal, Primitive)
            return new AsnType(0x04, value);
        }

        /// <summary>
        /// An ordered sequence of zero, one or more octets. Returns
        /// the byte[] representing an ASN.1 encoded octet string.
        /// If octets is null or length is 0, an empty (0 length)
        /// o ctet string is returned.
        /// </summary>
        /// <param name="value">An AsnType object to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded octet string.</returns>
        /// <seealso cref="CreateBitString(byte[])"/>
        /// <seealso cref="CreateBitString(byte[], uint)"/>
        /// <seealso cref="CreateBitString(AsnType)"/>
        /// <seealso cref="CreateBitString(String)"/>
        /// <seealso cref="CreateOctetString(byte[])"/>
        /// <seealso cref="CreateOctetString(String)"/>
        public static AsnType CreateOctetString(AsnType value)
        {
            if (IsEmpty(value))
            {
                // Empty octet string
                return new AsnType(0x04, 0x00);
            }

            // OctetString: Tag 0x04 (4, Universal, Primitive)
            return new AsnType(0x04, value.GetBytes());
        }

        /// <summary>
        /// An ordered sequence of zero, one or more octets. Returns
        /// the byte[] representing an ASN.1 encoded octet string.
        /// If octets is null or length is 0, an empty (0 length)
        /// o ctet string is returned.
        /// </summary>
        /// <param name="values">An AsnType object to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded octet string.</returns>
        /// <seealso cref="CreateBitString(byte[])"/>
        /// <seealso cref="CreateBitString(byte[], uint)"/>
        /// <seealso cref="CreateBitString(AsnType)"/>
        /// <seealso cref="CreateBitString(String)"/>
        /// <seealso cref="CreateOctetString(byte[])"/>
        /// <seealso cref="CreateOctetString(AsnType)"/>
        /// <seealso cref="CreateOctetString(String)"/>
        public static AsnType CreateOctetString(AsnType[] values)
        {
            if (IsEmpty(values))
            {
                // Empty octet string
                return new AsnType(0x04, 0x00);
            }

            // OctetString: Tag 0x04 (4, Universal, Primitive)
            return new AsnType(0x04, Concatenate(values));
        }

        /// <summary>
        /// <para>An ordered sequence of zero, one or more bits. Returns
        /// the AsnType representing an ASN.1 encoded octet string.</para>
        /// <para>If octets is null or length is 0, an empty (0 length)
        /// octet string is returned.</para>
        /// <para>If conversion fails, the bit string returned is a partial
        /// bit string. The partial octet string ends at the octet before the
        /// point of failure (it does not include the octet which could
        /// not be parsed, or subsequent octets).</para>
        /// </summary>
        /// <param name="value">A string representing the
        /// octet string to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded octet string.</returns>
        /// <seealso cref="CreateBitString(byte[])"/>
        /// <seealso cref="CreateBitString(byte[], uint)"/>
        /// <seealso cref="CreateBitString(String)"/>
        /// <seealso cref="CreateBitString(AsnType)"/>
        /// <seealso cref="CreateOctetString(byte[])"/>
        /// <seealso cref="CreateOctetString(AsnType)"/>
        /// <seealso cref="CreateOctetString(AsnType[])"/>
        public static AsnType CreateOctetString(String value)
        {
            if (IsEmpty(value))
            {
                return CreateOctetString(Empty);
            }

            // Determine number of octets
            int len = (value.Length + 255)/256;

            var octets = new List<byte>();
            for (int i = 0; i < len; i++)
            {
                String s = value.Substring(i*2, 2);
                byte b = 0x00;

                try
                {
                    b = Convert.ToByte(s, 16);
                }
                catch (FormatException /*e*/)
                {
                    break;
                }
                catch (OverflowException /*e*/)
                {
                    break;
                }

                octets.Add(b);
            }

            // OctetString: Tag 0x04 (4, Universal, Primitive)
            return CreateOctetString(octets.ToArray());
        }

        /// <summary>
        /// <para>Returns the AsnType representing a ASN.1 encoded
        /// integer. The octets pass through this method are not modified.</para>
        /// <para>If octets is null or zero length, the method returns an
        /// AsnType equivalent to CreateInteger(byte[]{0})..</para>
        /// </summary>
        /// <param name="value">A MSB (big endian) byte[] representing the
        /// integer to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded integer.</returns>
        /// <example>
        /// ASN.1 encoded 0:
        /// <code>CreateInteger(null)</code>
        /// <code>CreateInteger(new byte[]{0x00})</code>
        /// <code>CreateInteger(new byte[]{0x00, 0x00})</code>
        /// </example>
        /// <example>
        /// ASN.1 encoded 1:
        /// <code>CreateInteger(new byte[]{0x01})</code>
        /// </example>
        /// <seealso cref="CreateIntegerPos"/>
        /// <seealso cref="CreateIntegerNeg"/>
        public static AsnType CreateInteger(byte[] value)
        {
            // Is it better to add a '0', or silently
            //   drop the Integer? Dropping integers
            //   is probably not te best choice...
            if (IsEmpty(value))
            {
                return CreateInteger(Zero);
            }

            return new AsnType(0x02, value);
        }

        /// <summary>
        /// <para>Returns the AsnType representing a positive ASN.1 encoded
        /// integer. If the high bit of most significant byte is set,
        /// the method prepends a 0x00 to octets before assigning the
        /// value to ensure the resulting integer is interpreted as
        /// positive in the application.</para>
        /// <para>If octets is null or zero length, the method returns an
        /// AsnType equivalent to CreateInteger(byte[]{0})..</para>
        /// </summary>
        /// <param name="value">A MSB (big endian) byte[] representing the
        /// integer to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded positive integer.</returns>
        /// <example>
        /// ASN.1 encoded 0:
        /// <code>CreateIntegerPos(null)</code>
        /// <code>CreateIntegerPos(new byte[]{0x00})</code>
        /// <code>CreateIntegerPos(new byte[]{0x00, 0x00})</code>
        /// </example>
        /// <example>
        /// ASN.1 encoded 1:
        /// <code>CreateInteger(new byte[]{0x01})</code>
        /// </example>
        /// <seealso cref="CreateInteger"/>
        /// <seealso cref="CreateIntegerNeg"/>
        public static AsnType CreateIntegerPos(byte[] value)
        {
            byte[] i = null, d = Duplicate(value);

            if (IsEmpty(d))
            {
                d = Zero;
            }

            // Mediate the 2's compliment representation.
            // If the first byte has its high bit set, we will
            // add the additional byte of 0x00
            if (d.Length > 0 && d[0] > 0x7F)
            {
                i = new byte[d.Length + 1];
                i[0] = 0x00;
                Array.Copy(d, 0, i, 1, value.Length);
            }
            else
            {
                i = d;
            }

            // Integer: Tag 0x02 (2, Universal, Primitive)
            return CreateInteger(i);
        }

        /// <summary>
        /// <para>Returns the negative ASN.1 encoded integer. If the high
        /// bit of most significant byte is set, the integer is already
        /// considered negative.</para>
        /// <para>If the high bit of most significant byte
        /// is <bold>not</bold> set, the integer will be 2's complimented
        /// to form a negative integer.</para>
        /// <para>If octets is null or zero length, the method returns an
        /// AsnType equivalent to CreateInteger(byte[]{0})..</para>
        /// </summary>
        /// <param name="value">A MSB (big endian) byte[] representing the
        /// integer to be encoded.</param>
        /// <returns>Returns the negative ASN.1 encoded integer.</returns>
        /// <example>
        /// ASN.1 encoded 0:
        /// <code>CreateIntegerNeg(null)</code>
        /// <code>CreateIntegerNeg(new byte[]{0x00})</code>
        /// <code>CreateIntegerNeg(new byte[]{0x00, 0x00})</code>
        /// </example>
        /// <example>
        /// ASN.1 encoded -1 (2's compliment 0xFF):
        /// <code>CreateIntegerNeg(new byte[]{0x01})</code>
        /// </example>
        /// <example>
        /// ASN.1 encoded -2 (2's compliment 0xFE):
        /// <code>CreateIntegerNeg(new byte[]{0x02})</code>
        /// </example>
        /// <example>
        /// ASN.1 encoded -1:
        /// <code>CreateIntegerNeg(new byte[]{0xFF})</code>
        /// <code>CreateIntegerNeg(new byte[]{0xFF,0xFF})</code>
        /// Note: already negative since the high bit is set.</example>
        /// <example>
        /// ASN.1 encoded -255 (2's compliment 0xFF, 0x01):
        /// <code>CreateIntegerNeg(new byte[]{0x00,0xFF})</code>
        /// </example>
        /// <example>
        /// ASN.1 encoded -255 (2's compliment 0xFF, 0xFF, 0x01):
        /// <code>CreateIntegerNeg(new byte[]{0x00,0x00,0xFF})</code>
        /// </example>
        /// <seealso cref="CreateInteger"/>
        /// <seealso cref="CreateIntegerPos"/>
        public static AsnType CreateIntegerNeg(byte[] value)
        {
            // Is it better to add a '0', or silently
            //   drop the Integer? Dropping integers
            //   is probably not te best choice...
            if (IsEmpty(value))
            {
                return CreateInteger(Zero);
            }

            // No Trimming
            // The byte[] may be that way for a reason
            if (IsZero(value))
            {
                return CreateInteger(value);
            }

            //
            // At this point, we know we have at least 1 octet
            //

            // Is this integer already negative?
            if (value[0] >= 0x80)
                // Pass through with no modifications
            {
                return CreateInteger(value);
            }

            // No need to Duplicate - Compliment2s
            // performs the action
            byte[] c = Compliment2S(value);

            return CreateInteger(c);
        }

        /// <summary>
        /// Returns the AsnType representing an ASN.1 encoded null.
        /// </summary>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded null.</returns>
        public static AsnType CreateNull()
        {
            return new AsnType(0x05, new byte[] {0x00});
        }

        /// <summary>
        /// Removes leading 0x00 octets from the byte[] octets. This
        /// method may return an empty byte array (0 length).
        /// </summary>
        /// <param name="octets">An array of octets to trim.</param>
        /// <returns>A byte[] with leading 0x00 octets removed.</returns>
        public static byte[] TrimStart(byte[] octets)
        {
            if (IsEmpty(octets) || IsZero(octets))
            {
                return new byte[] {};
            }

            byte[] d = Duplicate(octets);

            // Position of the first non-zero value
            int pos = 0;
            foreach (byte b in d)
            {
                if (0 != b)
                {
                    break;
                }
                pos++;
            }

            // Nothing to trim
            if (pos == d.Length)
            {
                return octets;
            }

            // Allocate trimmed array
            var t = new byte[d.Length - pos];

            // Copy
            Array.Copy(d, pos, t, 0, t.Length);

            return t;
        }

        /// <summary>
        /// Removes trailing 0x00 octets from the byte[] octets. This
        /// method may return an empty byte array (0 length).
        /// </summary>
        /// <param name="octets">An array of octets to trim.</param>
        /// <returns>A byte[] with trailing 0x00 octets removed.</returns>
        public static byte[] TrimEnd(byte[] octets)
        {
            if (IsEmpty(octets) || IsZero(octets))
            {
                return Empty;
            }

            byte[] d = Duplicate(octets);

            Array.Reverse(d);

            d = TrimStart(d);

            Array.Reverse(d);

            return d;
        }

        /// <summary>
        /// Returns the AsnType representing an ASN.1 encoded OID.
        /// If conversion fails, the result is a partial conversion
        /// up to the point of failure. If the oid string is null or
        /// not well formed, an empty byte[] is returned.
        /// </summary>
        /// <param name="value">The string representing the object
        /// identifier to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded object identifier.</returns>
        /// <example>The following assigns the encoded AsnType
        /// for a RSA key to oid:
        /// <code>AsnType oid = CreateOid("1.2.840.113549.1.1.1")</code>
        /// </example>
        /// <seealso cref="CreateOid(byte[])"/>
        public static AsnType CreateOid(String value)
        {
            // Punt?
            if (IsEmpty(value))
                return null;

            String[] tokens = value.Split(new[] {' ', '.'});

            // Punt?
            if (IsEmpty(tokens))
                return null;

            // Parsing/Manipulation of the arc value
            UInt64 a = 0;

            // One or more strings are available
            var arcs = new List<UInt64>();

            foreach (String t in tokens)
            {
                // No empty or ill-formed strings...
                if (t.Length == 0)
                {
                    break;
                }

                try
                {
                    a = Convert.ToUInt64(t, CultureInfo.InvariantCulture);
                }
                catch (FormatException /*e*/)
                {
                    break;
                }
                catch (OverflowException /*e*/)
                {
                    break;
                }

                arcs.Add(a);
            }

            // Punt?
            if (0 == arcs.Count)
                return null;

            // Octets to be returned to caller
            var octets = new List<byte>();

            // Guard the case of a small list
            // The list has at least 1 item...    
            if (arcs.Count >= 1)
            {
                a = arcs[0]*40;
            }
            if (arcs.Count >= 2)
            {
                a += arcs[1];
            }
            octets.Add((byte)(a));

            // Add remaining arcs (subidentifiers)
            for (int i = 2; i < arcs.Count; i++)
            {
                // Scratch list builder for this arc
                var temp = new List<byte>();

                // The current arc (subidentifier)
                UInt64 arc = arcs[i];

                // Build the arc (subidentifier) byte array
                // The array is built in reverse (LSB to MSB).
                do
                {
                    // Each entry is formed from the low 7 bits (0x7F).
                    // Set high bit of all entries (0x80) per X.680. We
                    // will unset the high bit of the final byte later.
                    temp.Add((byte)(0x80 | (arc & 0x7F)));
                    arc >>= 7;
                } while (0 != arc);

                // Grab resulting array. Because of the do/while,
                // there is at least one value in the array.
                byte[] t = temp.ToArray();

                // Unset high bit of byte t[0]
                // t[0] will be LSB after the array is reversed.
                t[0] = (byte)(0x7F & t[0]);

                // MSB first...
                Array.Reverse(t);

                // Add to the resulting array
                foreach (byte b in t)
                {
                    octets.Add(b);
                }
            }

            return CreateOid(octets.ToArray());
        }

        /// <summary>
        /// Returns the AsnType representing an ASN.1 encoded OID.
        /// If conversion fails, the result is a partial conversion
        /// (up to the point of failure). If octets is null, an
        /// empty byte[] is returned.
        /// </summary>
        /// <param name="value">The packed byte[] representing the object
        /// identifier to be encoded.</param>
        /// <returns>Returns the AsnType representing an ASN.1
        /// encoded object identifier.</returns>
        /// <example>The following assigns the encoded AsnType for a RSA
        /// key to oid:
        /// <code>// Packed 1.2.840.113549.1.1.1
        /// byte[] rsa = new byte[] { 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 };
        /// AsnType = CreateOid(rsa)</code>
        /// </example>
        /// <seealso cref="CreateOid(String)"/>
        public static AsnType CreateOid(byte[] value)
        {
            // Punt...
            if (IsEmpty(value))
            {
                return null;
            }

            // OID: Tag 0x06 (6, Universal, Primitive)
            return new AsnType(0x06, value);
        }

        private static byte[] Compliment2S(byte[] value)
        {
            if (IsEmpty(value))
            {
                return Empty;
            }

            // 2s Compliment of 0 is 0
            if (IsZero(value))
            {
                return Duplicate(value);
            }

            // Make a copy of octet array
            byte[] d = Duplicate(value);

            int carry = 1;
            for (int i = d.Length - 1; i >= 0; i--)
            {
                // Compliment
                d[i] = (byte)~d[i];

                // Add
                int j = d[i] + carry;

                // Write Back
                d[i] = (byte)(j & 0xFF);

                // Determine Next Carry
                if (0x100 == (j & 0x100))
                {
                    carry = 1;
                }
                else
                {
                    carry = 0;
                }
            }

            // Carry Array (we may need to carry out of 'd'
            byte[] c = null;
            if (1 == carry)
            {
                c = new byte[d.Length + 1];

                // Sign Extend....
                c[0] = 0xFF;

                Array.Copy(d, 0, c, 1, d.Length);
            }
            else
            {
                c = d;
            }

            return c;
        }

        private static byte[] Concatenate(AsnType[] values)
        {
            // Nothing in, nothing out
            if (IsEmpty(values))
                return new byte[] {};

            int length = 0;
            foreach (AsnType t in values)
            {
                if (null != t)
                {
                    length += t.GetBytes().Length;
                }
            }

            var cated = new byte[length];

            int current = 0;
            foreach (AsnType t in values)
            {
                if (null != t)
                {
                    byte[] b = t.GetBytes();

                    Array.Copy(b, 0, cated, current, b.Length);
                    current += b.Length;
                }
            }

            return cated;
        }

        private static byte[] Concatenate(byte[] first, byte[] second)
        {
            return Concatenate(new[] {first, second});
        }

        private static byte[] Concatenate(byte[][] values)
        {
            // Nothing in, nothing out
            if (IsEmpty(values))
                return new byte[] {};

            int length = 0;
            foreach (var b in values)
            {
                if (null != b)
                {
                    length += b.Length;
                }
            }

            var cated = new byte[length];

            int current = 0;
            foreach (var b in values)
            {
                if (null != b)
                {
                    Array.Copy(b, 0, cated, current, b.Length);
                    current += b.Length;
                }
            }

            return cated;
        }

        private static byte[] Duplicate(byte[] b)
        {
            if (IsEmpty(b))
            {
                return Empty;
            }

            var d = new byte[b.Length];
            Array.Copy(b, d, b.Length);

            return d;
        }

        private static bool IsZero(byte[] octets)
        {
            if (IsEmpty(octets))
            {
                return false;
            }

            bool allZeros = true;
            for (int i = 0; i < octets.Length; i++)
            {
                if (0 != octets[i])
                {
                    allZeros = false;
                    break;
                }
            }
            return allZeros;
        }

        private static bool IsEmpty(byte[] octets)
        {
            if (null == octets || 0 == octets.Length)
            {
                return true;
            }

            return false;
        }

        private static bool IsEmpty(String s)
        {
            if (null == s || 0 == s.Length)
            {
                return true;
            }

            return false;
        }

        private static bool IsEmpty(String[] strings)
        {
            if (null == strings || 0 == strings.Length)
                return true;

            return false;
        }

        private static bool IsEmpty(AsnType value)
        {
            if (null == value)
            {
                return true;
            }

            return false;
        }

        private static bool IsEmpty(AsnType[] values)
        {
            if (null == values || 0 == values.Length)
                return true;

            return false;
        }

        private static bool IsEmpty(byte[][] arrays)
        {
            if (null == arrays || 0 == arrays.Length)
                return true;

            return false;
        }

        #region Nested type: AsnMessage

        public class AsnMessage
        {
            private readonly String mFormat;
            private readonly byte[] mOctets;

            public AsnMessage(byte[] octets, String format)
            {
                mOctets = octets;
                mFormat = format;
            }

            public int Length
            {
                get
                {
                    if (null == mOctets)
                    {
                        return 0;
                    }
                    return mOctets.Length;
                }
                // set { m_length = value; }
            }

            public byte[] GetBytes()
            {
                if (null == mOctets)
                {
                    return new byte[] {};
                }

                return mOctets;
            }

            public String GetFormat()
            {
                return mFormat;
            }
        }

        #endregion

        #region Nested type: AsnType

        public class AsnType
        {
            // Constructors
            // No default - must specify tag and data

            private readonly byte[] mTag;
            private byte[] mLength;
            private byte[] mOctets;
            private bool mRaw;

            public AsnType(byte tag, byte octet)
            {
                mRaw = false;
                mTag = new[] {tag};
                mOctets = new[] {octet};
            }

            public AsnType(byte tag, byte[] octets)
            {
                mRaw = false;
                mTag = new[] {tag};
                mOctets = octets;
            }

            public AsnType(byte tag, byte[] length, byte[] octets)
            {
                mRaw = true;
                mTag = new[] {tag};
                mLength = length;
                mOctets = octets;
            }

            private bool Raw
            {
                get { return mRaw; }
                set { mRaw = value; }
            }

            // Setters and Getters

            public byte[] Tag
            {
                get
                {
                    if (null == mTag)
                        return Empty;
                    return mTag;
                }
                // set { m_tag = value; }
            }

            public byte[] Length
            {
                get
                {
                    if (null == mLength)
                        return Empty;
                    return mLength;
                }
                // set { m_length = value; }
            }

            public byte[] Octets
            {
                get
                {
                    if (null == mOctets)
                    {
                        return Empty;
                    }
                    return mOctets;
                }
                set { mOctets = value; }
            }

            // Methods
            internal byte[] GetBytes()
            {
                // Created raw by user
                // return the bytes....
                if (mRaw)
                {
                    return Concatenate(
                        new[] {mTag, mLength, mOctets}
                        );
                }

                SetLength();

                // Special case
                // Null does not use length
                if (0x05 == mTag[0])
                {
                    return Concatenate(
                        new[] {mTag, mOctets}
                        );
                }

                return Concatenate(
                    new[] {mTag, mLength, mOctets}
                    );
            }

            private void SetLength()
            {
                if (null == mOctets)
                {
                    mLength = Zero;
                    return;
                }

                // Special case
                // Null does not use length
                if (0x05 == mTag[0])
                {
                    mLength = Empty;
                    return;
                }

                byte[] length = null;

                // Length: 0 <= l < 0x80
                if (mOctets.Length < 0x80)
                {
                    length = new byte[1];
                    length[0] = (byte)mOctets.Length;
                }
                    // 0x80 < length <= 0xFF
                else if (mOctets.Length <= 0xFF)
                {
                    length = new byte[2];
                    length[0] = 0x81;
                    length[1] = (byte)((mOctets.Length & 0xFF));
                }

                    //
                    // We should almost never see these...
                    //

                    // 0xFF < length <= 0xFFFF
                else if (mOctets.Length <= 0xFFFF)
                {
                    length = new byte[3];
                    length[0] = 0x82;
                    length[1] = (byte)((mOctets.Length & 0xFF00) >> 8);
                    length[2] = (byte)((mOctets.Length & 0xFF));
                }

                    // 0xFFFF < length <= 0xFFFFFF
                else if (mOctets.Length <= 0xFFFFFF)
                {
                    length = new byte[4];
                    length[0] = 0x83;
                    length[1] = (byte)((mOctets.Length & 0xFF0000) >> 16);
                    length[2] = (byte)((mOctets.Length & 0xFF00) >> 8);
                    length[3] = (byte)((mOctets.Length & 0xFF));
                }
                    // 0xFFFFFF < length <= 0xFFFFFFFF
                else
                {
                    length = new byte[5];
                    length[0] = 0x84;
                    length[1] = (byte)((mOctets.Length & 0xFF000000) >> 24);
                    length[2] = (byte)((mOctets.Length & 0xFF0000) >> 16);
                    length[3] = (byte)((mOctets.Length & 0xFF00) >> 8);
                    length[4] = (byte)((mOctets.Length & 0xFF));
                }

                mLength = length;
            }

            private byte[] Concatenate(byte[][] values)
            {
                // Nothing in, nothing out
                if (IsEmpty(values))
                    return new byte[] {};

                int length = 0;
                foreach (var b in values)
                {
                    if (null != b) length += b.Length;
                }

                var cated = new byte[length];

                int current = 0;
                foreach (var b in values)
                {
                    if (null != b)
                    {
                        Array.Copy(b, 0, cated, current, b.Length);
                        current += b.Length;
                    }
                }

                return cated;
            }
        };

        #endregion
    }

    public class AsnKeyParser
    {
        private AsnParser parser;

        public AsnKeyParser(String pathname)
        {
            using (BinaryReader reader = new BinaryReader(
                new FileStream(pathname, FileMode.Open, FileAccess.Read)))
            {
                FileInfo info = new FileInfo(pathname);

                parser = new AsnParser(reader.ReadBytes((int)info.Length));
            }
        }

        public AsnKeyParser(byte[] data)
        {
            parser = new AsnParser(data);
        }

        internal static byte[] TrimLeadingZero(byte[] values)
        {
            byte[] r = null;
            if ((0x00 == values[0]) && (values.Length > 1))
            {
                r = new byte[values.Length - 1];
                Array.Copy(values, 1, r, 0, values.Length - 1);
            }
            else
            {
                r = new byte[values.Length];
                Array.Copy(values, r, values.Length);
            }

            return r;
        }

        internal static bool EqualOid(byte[] first, byte[] second)
        {
            if (first.Length != second.Length)
            { return false; }

            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                { return false; }
            }

            return true;
        }

        public RSAParameters ParseRSAPublicKey()
        {
            RSAParameters parameters = new RSAParameters();

            // Current value
            byte[] value = null;

            // Sanity Check
            int length = 0;

            // Checkpoint
            int position = parser.CurrentPosition();

            // Ignore Sequence - PublicKeyInfo
            length = parser.NextSequence();
            if (length != parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect Sequence Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture),
                  parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = parser.CurrentPosition();

            // Ignore Sequence - AlgorithmIdentifier
            length = parser.NextSequence();
            if (length > parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect AlgorithmIdentifier Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture),
                  parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = parser.CurrentPosition();
            // Grab the OID
            value = parser.NextOID();
            byte[] oid = { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
            if (!EqualOid(value, oid))
            { throw new BerDecodeException("Expected OID 1.2.840.113549.1.1.1", position); }

            // Optional Parameters
            if (parser.IsNextNull())
            {
                parser.NextNull();
                // Also OK: value = parser.Next();
            }
            else
            {
                // Gracefully skip the optional data
                value = parser.Next();
            }

            // Checkpoint
            position = parser.CurrentPosition();

            // Ignore BitString - PublicKey
            length = parser.NextBitString();
            if (length > parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect PublicKey Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture),
                  (parser.RemainingBytes()).ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = parser.CurrentPosition();

            // Ignore Sequence - RSAPublicKey
            length = parser.NextSequence();
            if (length < parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect RSAPublicKey Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture),
                  parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            parameters.Modulus = TrimLeadingZero(parser.NextInteger());
            parameters.Exponent = TrimLeadingZero(parser.NextInteger());

            Debug.Assert(0 == parser.RemainingBytes());

            return parameters;
        }

        internal RSAParameters ParseRSAPrivateKey()
        {
            RSAParameters parameters = new RSAParameters();

            // Current value
            byte[] value = null;

            // Checkpoint
            int position = parser.CurrentPosition();

            // Sanity Check
            int length = 0;

            // Ignore Sequence - PrivateKeyInfo
            length = parser.NextSequence();
            if (length != parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect Sequence Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture), parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = parser.CurrentPosition();
            // Version
            value = parser.NextInteger();
            if (0x00 != value[0])
            {
                StringBuilder sb = new StringBuilder("Incorrect PrivateKeyInfo Version. ");
                BigInteger v = new BigInteger(value);
                sb.AppendFormat("Expected: 0, Specified: {0}", v.ToString(10));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = parser.CurrentPosition();

            // Ignore Sequence - AlgorithmIdentifier
            length = parser.NextSequence();
            if (length > parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect AlgorithmIdentifier Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture),
                  parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = parser.CurrentPosition();

            // Grab the OID
            value = parser.NextOID();
            byte[] oid = { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
            if (!EqualOid(value, oid))
            { throw new BerDecodeException("Expected OID 1.2.840.113549.1.1.1", position); }

            // Optional Parameters
            if (parser.IsNextNull())
            {
                parser.NextNull();
                // Also OK: value = parser.Next();
            }
            else
            {
                // Gracefully skip the optional data
                value = parser.Next();
            }

            // Checkpoint
            position = parser.CurrentPosition();

            // Ignore OctetString - PrivateKey
            length = parser.NextOctetString();
            if (length > parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect PrivateKey Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture),
                  parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = parser.CurrentPosition();

            // Ignore Sequence - RSAPrivateKey
            length = parser.NextSequence();
            if (length < parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect RSAPrivateKey Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture),
                  parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = parser.CurrentPosition();
            // Version
            value = parser.NextInteger();
            if (0x00 != value[0])
            {
                StringBuilder sb = new StringBuilder("Incorrect RSAPrivateKey Version. ");
                BigInteger v = new BigInteger(value);
                sb.AppendFormat("Expected: 0, Specified: {0}", v.ToString(10));
                throw new BerDecodeException(sb.ToString(), position);
            }

            parameters.Modulus = TrimLeadingZero(parser.NextInteger());
            parameters.Exponent = TrimLeadingZero(parser.NextInteger());
            parameters.D = TrimLeadingZero(parser.NextInteger());
            parameters.P = TrimLeadingZero(parser.NextInteger());
            parameters.Q = TrimLeadingZero(parser.NextInteger());
            parameters.DP = TrimLeadingZero(parser.NextInteger());
            parameters.DQ = TrimLeadingZero(parser.NextInteger());
            parameters.InverseQ = TrimLeadingZero(parser.NextInteger());

            Debug.Assert(0 == parser.RemainingBytes());

            return parameters;
        }

        internal DSAParameters ParseDSAPublicKey()
        {
            DSAParameters parameters = new DSAParameters();

            // Current value
            byte[] value = null;

            // Current Position
            int position = parser.CurrentPosition();
            // Sanity Checks
            int length = 0;

            // Ignore Sequence - PublicKeyInfo
            length = parser.NextSequence();
            if (length != parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect Sequence Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture), parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = parser.CurrentPosition();

            // Ignore Sequence - AlgorithmIdentifier
            length = parser.NextSequence();
            if (length > parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect AlgorithmIdentifier Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture), parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = parser.CurrentPosition();

            // Grab the OID
            value = parser.NextOID();
            byte[] oid = { 0x2a, 0x86, 0x48, 0xce, 0x38, 0x04, 0x01 };
            if (!EqualOid(value, oid))
            { throw new BerDecodeException("Expected OID 1.2.840.10040.4.1", position); }


            // Checkpoint
            position = parser.CurrentPosition();

            // Ignore Sequence - DSS-Params
            length = parser.NextSequence();
            if (length > parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect DSS-Params Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture), parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Next three are curve parameters
            parameters.P = TrimLeadingZero(parser.NextInteger());
            parameters.Q = TrimLeadingZero(parser.NextInteger());
            parameters.G = TrimLeadingZero(parser.NextInteger());

            // Ignore BitString - PrivateKey
            parser.NextBitString();

            // Public Key
            parameters.Y = TrimLeadingZero(parser.NextInteger());

            Debug.Assert(0 == parser.RemainingBytes());

            return parameters;
        }

        internal DSAParameters ParseDSAPrivateKey()
        {
            DSAParameters parameters = new DSAParameters();

            // Current value
            byte[] value = null;

            // Current Position
            int position = parser.CurrentPosition();
            // Sanity Checks
            int length = 0;

            // Ignore Sequence - PrivateKeyInfo
            length = parser.NextSequence();
            if (length != parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect Sequence Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture), parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = parser.CurrentPosition();
            // Version
            value = parser.NextInteger();
            if (0x00 != value[0])
            {
                throw new BerDecodeException("Incorrect PrivateKeyInfo Version", position);
            }

            // Checkpoint
            position = parser.CurrentPosition();

            // Ignore Sequence - AlgorithmIdentifier
            length = parser.NextSequence();
            if (length > parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect AlgorithmIdentifier Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture), parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Checkpoint
            position = parser.CurrentPosition();
            // Grab the OID
            value = parser.NextOID();
            byte[] oid = { 0x2a, 0x86, 0x48, 0xce, 0x38, 0x04, 0x01 };
            if (!EqualOid(value, oid))
            { throw new BerDecodeException("Expected OID 1.2.840.10040.4.1", position); }

            // Checkpoint
            position = parser.CurrentPosition();

            // Ignore Sequence - DSS-Params
            length = parser.NextSequence();
            if (length > parser.RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect DSS-Params Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  length.ToString(CultureInfo.InvariantCulture), parser.RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            // Next three are curve parameters
            parameters.P = TrimLeadingZero(parser.NextInteger());
            parameters.Q = TrimLeadingZero(parser.NextInteger());
            parameters.G = TrimLeadingZero(parser.NextInteger());

            // Ignore OctetString - PrivateKey
            parser.NextOctetString();

            // Private Key
            parameters.X = TrimLeadingZero(parser.NextInteger());

            Debug.Assert(0 == parser.RemainingBytes());

            return parameters;
        }
    }

    class AsnParser
    {
        private List<byte> octets;
        private int initialCount;

        public AsnParser(byte[] values)
        {
            octets = new List<byte>(values.Length);
            octets.AddRange(values);

            initialCount = octets.Count;
        }

        internal int CurrentPosition()
        {
            return initialCount - octets.Count;
        }

        internal int RemainingBytes()
        {
            return octets.Count;
        }

        private int GetLength()
        {
            int length = 0;

            // Checkpoint
            int position = CurrentPosition();

            try
            {
                byte b = GetNextOctet();

                if (b == (b & 0x7f)) { return b; }
                int i = b & 0x7f;

                if (i > 4)
                {
                    StringBuilder sb = new StringBuilder("Invalid Length Encoding. ");
                    sb.AppendFormat("Length uses {0} octets",
                      i.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                while (0 != i--)
                {
                    // shift left
                    length <<= 8;

                    length |= GetNextOctet();
                }
            }
            catch (ArgumentOutOfRangeException ex)
            { throw new BerDecodeException("Error Parsing Key", position, ex); }

            return length;
        }

        internal byte[] Next()
        {
            int position = CurrentPosition();

            try
            {
                /*byte b = */GetNextOctet();

                int length = GetLength();
                if (length > RemainingBytes())
                {
                    StringBuilder sb = new StringBuilder("Incorrect Size. ");
                    sb.AppendFormat("Specified: {0}, Remaining: {1}",
                      length.ToString(CultureInfo.InvariantCulture),
                      RemainingBytes().ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                return GetOctets(length);
            }

            catch (ArgumentOutOfRangeException ex)
            { throw new BerDecodeException("Error Parsing Key", position, ex); }
        }

        internal byte GetNextOctet()
        {
            int position = CurrentPosition();

            if (0 == RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  1.ToString(CultureInfo.InvariantCulture),
                  RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            byte b = GetOctets(1)[0];

            return b;
        }

        internal byte[] GetOctets(int octetCount)
        {
            int position = CurrentPosition();

            if (octetCount > RemainingBytes())
            {
                StringBuilder sb = new StringBuilder("Incorrect Size. ");
                sb.AppendFormat("Specified: {0}, Remaining: {1}",
                  octetCount.ToString(CultureInfo.InvariantCulture),
                  RemainingBytes().ToString(CultureInfo.InvariantCulture));
                throw new BerDecodeException(sb.ToString(), position);
            }

            byte[] values = new byte[octetCount];

            try
            {
                octets.CopyTo(0, values, 0, octetCount);
                octets.RemoveRange(0, octetCount);
            }

            catch (ArgumentOutOfRangeException ex)
            { throw new BerDecodeException("Error Parsing Key", position, ex); }

            return values;
        }

        internal bool IsNextNull()
        {
            return 0x05 == octets[0];
        }

        internal int NextNull()
        {
            int position = CurrentPosition();

            try
            {
                byte b = GetNextOctet();
                if (0x05 != b)
                {
                    StringBuilder sb = new StringBuilder("Expected Null. ");
                    sb.AppendFormat("Specified Identifier: {0}", b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                // Next octet must be 0
                b = GetNextOctet();
                if (0x00 != b)
                {
                    StringBuilder sb = new StringBuilder("Null has non-zero size. ");
                    sb.AppendFormat("Size: {0}", b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                return 0;
            }

            catch (ArgumentOutOfRangeException ex)
            { throw new BerDecodeException("Error Parsing Key", position, ex); }
        }

        internal bool IsNextSequence()
        {
            return 0x30 == octets[0];
        }

        internal int NextSequence()
        {
            int position = CurrentPosition();

            try
            {
                byte b = GetNextOctet();
                if (0x30 != b)
                {
                    StringBuilder sb = new StringBuilder("Expected Sequence. ");
                    sb.AppendFormat("Specified Identifier: {0}",
                      b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                int length = GetLength();
                if (length > RemainingBytes())
                {
                    StringBuilder sb = new StringBuilder("Incorrect Sequence Size. ");
                    sb.AppendFormat("Specified: {0}, Remaining: {1}",
                      length.ToString(CultureInfo.InvariantCulture),
                      RemainingBytes().ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                return length;
            }

            catch (ArgumentOutOfRangeException ex)
            { throw new BerDecodeException("Error Parsing Key", position, ex); }
        }

        internal bool IsNextOctetString()
        {
            return 0x04 == octets[0];
        }

        internal int NextOctetString()
        {
            int position = CurrentPosition();

            try
            {
                byte b = GetNextOctet();
                if (0x04 != b)
                {
                    StringBuilder sb = new StringBuilder("Expected Octet String. ");
                    sb.AppendFormat("Specified Identifier: {0}", b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                int length = GetLength();
                if (length > RemainingBytes())
                {
                    StringBuilder sb = new StringBuilder("Incorrect Octet String Size. ");
                    sb.AppendFormat("Specified: {0}, Remaining: {1}",
                      length.ToString(CultureInfo.InvariantCulture),
                      RemainingBytes().ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                return length;
            }

            catch (ArgumentOutOfRangeException ex)
            { throw new BerDecodeException("Error Parsing Key", position, ex); }
        }

        internal bool IsNextBitString()
        {
            return 0x03 == octets[0];
        }

        internal int NextBitString()
        {
            int position = CurrentPosition();

            try
            {
                byte b = GetNextOctet();
                if (0x03 != b)
                {
                    StringBuilder sb = new StringBuilder("Expected Bit String. ");
                    sb.AppendFormat("Specified Identifier: {0}", b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                int length = GetLength();

                // We need to consume unused bits, which is the first
                //   octet of the remaing values
                b = octets[0];
                octets.RemoveAt(0);
                length--;

                if (0x00 != b)
                { throw new BerDecodeException("The first octet of BitString must be 0", position); }

                return length;
            }

            catch (ArgumentOutOfRangeException ex)
            { throw new BerDecodeException("Error Parsing Key", position, ex); }
        }

        internal bool IsNextInteger()
        {
            return 0x02 == octets[0];
        }

        internal byte[] NextInteger()
        {
            int position = CurrentPosition();

            try
            {
                byte b = GetNextOctet();
                if (0x02 != b)
                {
                    StringBuilder sb = new StringBuilder("Expected Integer. ");
                    sb.AppendFormat("Specified Identifier: {0}", b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                int length = GetLength();
                if (length > RemainingBytes())
                {
                    StringBuilder sb = new StringBuilder("Incorrect Integer Size. ");
                    sb.AppendFormat("Specified: {0}, Remaining: {1}",
                      length.ToString(CultureInfo.InvariantCulture),
                      RemainingBytes().ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                return GetOctets(length);
            }

            catch (ArgumentOutOfRangeException ex)
            { throw new BerDecodeException("Error Parsing Key", position, ex); }
        }

        internal byte[] NextOID()
        {
            int position = CurrentPosition();

            try
            {
                byte b = GetNextOctet();
                if (0x06 != b)
                {
                    StringBuilder sb = new StringBuilder("Expected Object Identifier. ");
                    sb.AppendFormat("Specified Identifier: {0}",
                      b.ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                int length = GetLength();
                if (length > RemainingBytes())
                {
                    StringBuilder sb = new StringBuilder("Incorrect Object Identifier Size. ");
                    sb.AppendFormat("Specified: {0}, Remaining: {1}",
                      length.ToString(CultureInfo.InvariantCulture),
                      RemainingBytes().ToString(CultureInfo.InvariantCulture));
                    throw new BerDecodeException(sb.ToString(), position);
                }

                byte[] values = new byte[length];

                for (int i = 0; i < length; i++)
                {
                    values[i] = octets[0];
                    octets.RemoveAt(0);
                }

                return values;
            }

            catch (ArgumentOutOfRangeException ex)
            { throw new BerDecodeException("Error Parsing Key", position, ex); }
        }
    }

    [Serializable]
    public sealed class BerDecodeException : Exception, ISerializable
    {
        private int m_position;
        public int Position
        { get { return m_position; } }

        public override string Message
        {
            get
            {
                StringBuilder sb = new StringBuilder(base.Message);

                sb.AppendFormat(" (Position {0}){1}",
                  m_position, Environment.NewLine);

                return sb.ToString();
            }
        }

        public BerDecodeException()
            : base() { }

        public BerDecodeException(String message)
            : base(message) { }

        public BerDecodeException(String message, Exception ex)
            : base(message, ex) { }

        public BerDecodeException(String message, int position)
            : base(message) { m_position = position; }

        public BerDecodeException(String message, int position, Exception ex)
            : base(message, ex) { m_position = position; }

        private BerDecodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { m_position = info.GetInt32("Position"); }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Position", m_position);
        }
    }
}