﻿using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AzureFromTheTrenches.Commanding.Http.Implementation
{
    class UriCommandQueryBuilder : IUriCommandQueryBuilder
    {
        public string Query<TCommand>(Uri uri, TCommand command) where TCommand : class
        {
            // TODO: We need to optimise this with some caching and code emitting
            bool isFirstQueryParameter = string.IsNullOrWhiteSpace(uri.Query);
            StringBuilder sb = new StringBuilder(isFirstQueryParameter ? uri.Query : "");
            foreach (PropertyInfo propertyInfo in command.GetType().GetRuntimeProperties().OrderBy(x => x.Name).ToArray())
            {
                if (!isFirstQueryParameter)
                {
                    sb.Append("&");
                }
                else
                {
                    isFirstQueryParameter = false;
                }
                sb.Append(propertyInfo.Name);
                sb.Append("=");
                sb.Append(Uri.EscapeDataString(propertyInfo.GetValue(command).ToString()));
            }

            return sb.ToString();
        }
    }
}
