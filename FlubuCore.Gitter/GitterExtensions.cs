using System;
using System.Collections.Generic;
using System.Text;
using FlubuCore.Context;
using FlubuCore.Context.FluentInterface;
using FlubuCore.Context.FluentInterface.Interfaces;
using FlubuCore.Gitter;
using FlubuCore.Infrastructure;

// ReSharper disable once CheckNamespace
namespace FlubuCore.Context.FluentInterface
{
    public static class GitterExtensions
    {
        public static GitterTask Gitter(this ITaskFluentInterface flubu, string message, string roomId, string token)
        {
            var taskInterface = (TaskFluentInterface)flubu;
            var taskContextInternal = (TaskContextInternal)taskInterface.Context;
            ////Get from ioc when available in flubu context.
            var httpClinetFactory = new HttpClientFactory();
            return new GitterTask(message, roomId, token, httpClinetFactory);
        }
    }
}
