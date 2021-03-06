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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using ConfigurationTests.Attributes;
using System.Diagnostics;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.OS)]
    public class CallExecutableCheckReturnCodeTest : Test
    {
        [Description("Path of executable or batch file.")]
        [Category("Executable Properties")]
        [MandatoryField]
        public string Executable { get; set; }

        [Description("Command line parameters to pass into executable or batch file.")]
        [Category("Executable Properties")]        
        public string Parameters { get; set; }

        [Description("Return code from executable or batch file.")]
        [Category("Executable Properties")]
        [MandatoryField]
        public int ReturnCode { get; set; }

        public override void Run()
        {
            var startInfo = new ProcessStartInfo(Executable) {Arguments = Parameters};
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.ErrorDialog = false;

            var process = Process.Start(startInfo);

            if (process == null) throw new InvalidOperationException("Process Could not Execute in <CallExecutableCheckReturnCodeTest>");

            process.WaitForExit();

            AssertState.Equal(ReturnCode, process.ExitCode);            
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test> { new CallExecutableCheckReturnCodeTest
            {
                Executable = "c:\temp\test.bat",
                Parameters = "arg1 arg2",
                ReturnCode = 1
            } };
        }
    }
}