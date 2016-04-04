﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Restier.Core.Properties;

namespace Microsoft.Restier.Core
{
    /// <summary>
    /// Represents context under which an API operates.
    /// </summary>
    /// <remarks>
    /// An API context is an instantiation of an API configuration.
    /// </remarks>
    public class ApiContext
    {
        private readonly IServiceScope scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext" /> class.
        /// </summary>
        /// <param name="configuration">
        /// An API configuration.
        /// </param>
        public ApiContext(ApiConfiguration configuration)
        {
            Ensure.NotNull(configuration, "configuration");

            this.Configuration = configuration;
            this.scope = configuration.ServiceProvider
                .GetRequiredService<IServiceScopeFactory>().CreateScope();
        }

        /// <summary>
        /// Gets the API configuration.
        /// </summary>
        public ApiConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> which contains all services of this <see cref="ApiContext"/>.
        /// </summary>
        public IServiceProvider ServiceProvider
        {
            get { return this.scope.ServiceProvider; }
        }

        /// <summary>
        /// Gets a service instance.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// <returns>The service instance.</returns>
        public T GetApiService<T>() where T : class
        {
            return this.ServiceProvider.GetService<T>();
        }

        /// <summary>
        /// Gets all registered service instances.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// <returns>The ordered collection of service instances.</returns>
        public IEnumerable<T> GetApiServices<T>() where T : class
        {
            return this.ServiceProvider.GetServices<T>();
        }

        internal void DisposeScope()
        {
            this.scope.Dispose();
        }
    }
}
