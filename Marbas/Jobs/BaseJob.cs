// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Marbas.Enums;
using Marbas.Interfaces;

namespace Marbas.Job
{
    public abstract class BaseJob : IJob
    {
        public JobStateEnum JobState { get; set; }

        public BaseJob()
        {
            JobState = JobStateEnum.InQueue;
        }

        public abstract Task Execute();
    }
}