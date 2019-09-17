//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.Azure.Cosmos.Query
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Constants = Documents.Constants;

    // Note: We also return this to client when query execution is disallowed by Gateway
    internal sealed class PartitionedQueryExecutionInfo
    {
        public PartitionedQueryExecutionInfo()
        {
            this.Version = Constants.PartitionedQueryExecutionInfo.CurrentVersion;
        }

        [JsonProperty(Constants.Properties.PartitionedQueryExecutionInfoVersion)]
        public int Version
        {
            get;
            private set;
        }

        [JsonProperty(Constants.Properties.QueryInfo)]
        public QueryInfo QueryInfo
        {
            get;
            set;
        }

        [JsonProperty(Constants.Properties.QueryRanges)]
        public List<Documents.Routing.Range<string>> QueryRanges
        {
            get;
            set;
        }
    }
}
