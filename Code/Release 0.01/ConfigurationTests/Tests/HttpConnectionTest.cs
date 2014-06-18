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
using System;
using System.Collections.Generic;
using System.Net;

namespace ConfigurationTests.Tests
{
    public class HttpConnectionTest : Test
    {
        public string UrlToTest { get; set; }
        public string ExpectedResponse { get; set; }

        public override void Run()
        {
            var httpRequest = WebRequest.Create(UrlToTest);
            HttpWebResponse httpResponse;

            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            int status = (int)httpResponse.StatusCode;
            AssertState.Equal(ExpectedResponse, status.ToString(), false, String.Format("The Http response was {0}. The Expected response is {1}", status, ExpectedResponse));
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>{
                new HttpConnectionTest {UrlToTest="http://www.google.com", ExpectedResponse="200"}
            };
        }
    }
}