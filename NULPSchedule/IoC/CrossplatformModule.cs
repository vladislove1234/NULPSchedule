using System;
using Autofac;
using NULPScgedule.Models.Schedule_Managers;
using NULPSchedule.Models.Interfaces;
using NULPSchedule.ViewModels;

namespace NULPSchedule.IoC
{
    public class CrossplatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ScheduleViewModel>().AsSelf();
            builder.RegisterType<ParserShceduleManager>().As<IScheduleManager>();
        }
    }
}
