﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MobileClient.Helpers
{
    // code adjusted from Thinktecture's client model (thinktecture.github.com)

    public static class EndpointAndTokenHelper
    {
        public async static Task<JObject> CallUserInfoEndpoint(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync(ExpenseTrackerConstants.IdSrvUserInfo);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JObject.Parse(json);
 
            }
            else
            {
                return null;
            }
        }


        public static JObject DecodeToken(string token)
        {
           
            var parts = token.Split('.');

            string partToConvert = parts[1];
            partToConvert = partToConvert.Replace('-', '+'); 
            partToConvert = partToConvert.Replace('_', '/');  
            switch (partToConvert.Length % 4)  
            {
                case 0:
                    break;  
                case 2: 
                    partToConvert += "==";
                    break;  
                case 3: 
                    partToConvert += "=";
                    break;  
                default:
                    break;
            }

            var partAsBytes = Convert.FromBase64String(partToConvert);             
            var partAsUTF8String = Encoding.UTF8.GetString(partAsBytes, 0, partAsBytes.Count());

            // Json .NET
           return JObject.Parse(partAsUTF8String);
 

       
        }

     
    }
}