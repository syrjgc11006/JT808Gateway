﻿using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Protocol.Extensions;
using JT808.Gateway.Abstractions;
using System;

namespace JT808.Gateway.Traffic
{
    public class JT808TrafficServiceHostedService : IHostedService
    {
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        private readonly IJT808Traffic  jT808Traffic;

        public JT808TrafficServiceHostedService(
            IJT808Traffic jT808Traffic,
            IJT808MsgConsumer jT808MsgConsumer)
        {
            this.jT808MsgConsumer = jT808MsgConsumer;
            this.jT808Traffic = jT808Traffic;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage((item)=> {
                //string str = item.Data.ToHexString();
                jT808Traffic.Increment(item.TerminalNo,DateTime.Now.ToString("yyyyMMdd"), item.Data.Length);
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
