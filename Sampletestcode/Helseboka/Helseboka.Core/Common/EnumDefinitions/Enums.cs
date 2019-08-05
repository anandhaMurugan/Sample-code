using System;

namespace Helseboka.Core.Common.EnumDefinitions
{
    public enum Day
    {
        Others,
        Yesterday,
        Today,
        Tomorrow
    }

    public enum LoginMode
    {
        Biometric,
        PIN,
        BankID
    }

    public enum MessageDirection
    {
        Others,
        Sent,
        Received
    }

    public enum ChatStatus
    {
        Sent,
        Delivered,
        Error,
        Received,
    }

    public enum TimeOfDay
    {
        Early,
        Midday,
        Late
    }

    public enum AppointmentStatus
    {
        Requested,
        Confirmed,
        Cancelled
    }

    public enum AlarmStatus
    {
        Pending,
        Completed
    }

    public enum ConfigTypes
    {
        Dev,
        Test,
        Staging,
        Prod
    }

    public enum BankIdConfigTypes
    {
       PreProd,
       Prod
    }

    public enum PlatformType
    {
        ANDROID,
        IOS,
        UNKNOWN
    }
}
