/**
* Smoke Tester Tool : Post deployment smoke testing tool.
* 
* http://www.stephenhaunts.com
* 
* This file is part of Smoke Tester Tool.
* 
* Smoke Tester Tool is free software: you can redistribute it and/or modify it under the terms of the
* GNU General Public License as published by the Free Software Foundation, either version 2 of the
* License, or (at your option) any later version.
* 
* Smoke Tester Tool is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* 
* See the GNU General Public License for more details <http://www.gnu.org/licenses/>.
* 
* Curator: Stephen Haunts
*/
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using Common.Security;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.OS)]
    public class CertificateExistsTest : Test
    {
        [Description("The store to get the certificate from")]
        [Category("Certificate Store Properties")]
        public StoreName CertificateStoreName { get; set; }

        [Description("The location of the store")]
        [Category("Certificate Store Properties")]
        public StoreLocation CertificateStoreLocation { get; set; }

        [Description("The certificate thumbprint")]
        [Category("Certificate Store Properties")]
        public string ThumbPrint { get; set; }

        [Description("The certificate subject name (CN)")]
        [Category("Certificate Store Properties")]
        public string SubjectName { get; set; }

        [Description("The certificate key usage")]
        [Category("Certificate Store Properties")]
        public string KeyUsage { get; set; }

        [Description("The certificate serial number")]
        [Category("Certificate Store Properties")]
        public string SerialNumber { get; set; }

        [Description("The certificate issuer name")]
        [Category("Certificate Store Properties")]
        public string IssuerName { get; set; }

        public override void Run()
        {
            var certificateInfo = new CertificateInfo {IssuerName = IssuerName,SerialNumber = SerialNumber,KeyUsage = KeyUsage,SubjectName = SubjectName,ThumbPrint = ThumbPrint};
            Certificates.GetCertificateFromStore(CertificateStoreName, CertificateStoreLocation, certificateInfo);
        }

        public override List<Test> CreateExamples()
        {
            var examples = new List<Test>
            {
                new CertificateExistsTest()
                {
                    CertificateStoreName = StoreName.Root,
                    CertificateStoreLocation = StoreLocation.LocalMachine,
                    IssuerName = "Microsoft Root Certificate Authority"
                },
                new CertificateExistsTest()
                {
                    CertificateStoreName = StoreName.Root,
                    CertificateStoreLocation = StoreLocation.CurrentUser,
                    IssuerName = "Microsoft Root Certificate Authority",
                    SubjectName = "Microsoft Code Signing PCA"
                },
                new CertificateExistsTest()
                {
                    CertificateStoreName = StoreName.AddressBook,
                    CertificateStoreLocation = StoreLocation.CurrentUser,
                    IssuerName = "Mr. X's Certificates",
                    SerialNumber = "ABCD1234"
                },
                new CertificateExistsTest()
                {
                    CertificateStoreName = StoreName.TrustedPublisher,
                    CertificateStoreLocation = StoreLocation.CurrentUser,
                    IssuerName = "GeoTrust Global CA",
                    KeyUsage = "Certificate Signing"
                }
            };

            return examples;
        }
    }
}