using System;
using Helseboka.Droid.Common.Interfaces;

namespace Helseboka.Droid.Common.Router
{
    public class BaseRouter
    {
        protected IActivity Activity { get; private set; }

        public BaseRouter(IActivity activity)
        {
            this.Activity = activity;
        }
    }
}
