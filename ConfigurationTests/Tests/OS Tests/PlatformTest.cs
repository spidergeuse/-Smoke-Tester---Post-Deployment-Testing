﻿/**
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
using ConfigurationTests.Attributes;

using System;
using System.IO;

namespace ConfigurationTests.Tests
{
    public enum CpuArchitectures
    {
        Unknown,
        x86,
        x64,
        AnyCpu
    }

    [TestCategory(Enums.TestCategory.OS)]
    public class PlatformTest : Test
    {
        [MandatoryField]
        [Category("Platform Properties")]
        public string AssemblyFilePath { get; set; }

        [MandatoryField]
        [Category("Platform Properties")]
        public CpuArchitectures ExpectedCpuArchitecture { get; set; }

        public override void Run()
        {
            try
            {
                var actualCpuArchitecture = GetPeArchitecture(AssemblyFilePath);

                if (ExpectedCpuArchitecture.ToString() != actualCpuArchitecture.ToString())
                {
                    throw new AssertionException(
                        string.Format("{0} was expected to be compiled for {1} but was actually compiled for {2}",
                            AssemblyFilePath,
                            ExpectedCpuArchitecture,
                            actualCpuArchitecture));
                }
            }
            catch (BadImageFormatException)
            {
                throw new AssertionException(string.Format("Unable to load {0} because it is not a .Net assembly", AssemblyFilePath));
            }
        }

        public static CpuArchitectures GetPeArchitecture(string pFilePath)
        {
            ushort architecture = 0;

            var coffHeader = new ushort[10];

            using (var fStream = new FileStream(pFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var bReader = new BinaryReader(fStream))
                {
                    if (bReader.ReadUInt16() == 23117) //check the MZ signature
                    {
                        fStream.Seek(0x3A, SeekOrigin.Current); // Go to the location of the location of the NT header
                        fStream.Seek(bReader.ReadUInt32(), SeekOrigin.Begin); // seek to the start of the NT header.

                        if (bReader.ReadUInt32() == 17744) //check the PE\0\0 signature.
                        {
                            for (int i = 0; i < 10; i++) // Read COFF Header
                            {
                                coffHeader[i] = bReader.ReadUInt16();
                            }

                            if (coffHeader[8] > 0) // Read Optional Header
                            {
                                architecture = bReader.ReadUInt16();
                            }
                        }
                    }
                }
            }

            switch (architecture)
            {
                case 0x20b:
                    return CpuArchitectures.x64;
                case 0x10b:
                    return ((coffHeader[9] & 0x100) == 0) ? CpuArchitectures.AnyCpu : CpuArchitectures.x86;
                default:
                    return CpuArchitectures.Unknown;
            }
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                        {
                            new PlatformTest
                                {
                                    ExpectedCpuArchitecture = CpuArchitectures.AnyCpu,
                                    TestName = "Any CPU Platform Check Example",
                                    AssemblyFilePath = @"C:\Assembly\MyExecutable.exe"
                                },
                                new PlatformTest
                                {
                                    ExpectedCpuArchitecture = CpuArchitectures.x86,
                                    TestName = "x86 Platform Check Example",
                                    AssemblyFilePath = @"C:\Assembly\MyExecutable.exe"
                                },
                                new PlatformTest
                                {
                                    ExpectedCpuArchitecture =  CpuArchitectures.x64,
                                    TestName = "x64 Platform Check Example",
                                    AssemblyFilePath = @"C:\Assembly\MyExecutable.exe"
                                }
                        };
        }
    }
}
