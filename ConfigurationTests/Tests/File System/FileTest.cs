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
using ConfigurationTests.Attributes;
using System.ComponentModel;

namespace ConfigurationTests.Tests
{
    public abstract class FileTest : Test
    {
        private string _path = ".";

        [DefaultValue(".")]
        [Description("Directory of file")]
        [Category("File Properties")]
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        [MandatoryField]
        [Category("File Properties")]
        [Description("Name of file")]
        public string Filename { get; set; }

        protected string FullFilePath
        {
            get
            {
                return System.IO.Path.Combine(Path, Filename);
            }
        }
    }
}