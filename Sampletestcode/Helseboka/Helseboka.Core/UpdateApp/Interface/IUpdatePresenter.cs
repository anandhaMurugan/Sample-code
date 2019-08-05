
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helseboka.Core.Common.Interfaces;

namespace Helseboka.Core.Startup.UpdateVersion.Interface
{
    public interface IUpdatePresenter : IBasePresenter
    {
        void ProceedCloseOrNotNowClicked();
    }
}