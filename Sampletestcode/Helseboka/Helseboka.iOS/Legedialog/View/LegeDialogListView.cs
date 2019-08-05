using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Core.Legedialog.Model;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.TableViewCell;
using Helseboka.iOS.Common.TableViewDelegates;
using Helseboka.iOS.Common.View;
using Helseboka.Core.Common.Extension;
using Helseboka.iOS.Common.Extension;
using Helseboka.Core.Common.EnumDefinitions;
using UIKit;
using System.Linq;
using Helseboka.Core.Resources.StringResources;

namespace Helseboka.iOS.Legedialog.View
{
	public partial class LegeDialogListView : BaseView
    {
        private const int cellBuffer = 5;
        private DialogListDataSource tableviewSource = new DialogListDataSource();
		public LegeDialogListView() {}

		public LegeDialogListView(IntPtr ptr) : base(ptr) {}

		public ILegedialogListPresenter Presenter
		{
			get => presenter as ILegedialogListPresenter;
			set => presenter = value;
		}

		public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var view = new HelpViewController(HelpFAQType.ChatHome, UIImage.FromBundle("Legedialog-home"), AppResources.ChatHomeHelpTitle);
            EmbedView(NoDataView, this, view);

            tableviewSource.DidScroll += Tableview_DidScroll;
            tableviewSource.DidSelect += Tableview_DidSelect;
            DataTableView.Source = tableviewSource;

            DataTableView.AddPullToRefresh(() => RefreshData());
            ShowDataTable();
            DataTableView.ShowLoader();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            RefreshData().Forget();
        }

        partial void NewDialog_Tapped(PrimaryActionButton sender)
        {
            CheckDoctorAndProceed(() =>
            {
                NavigateToNewThreadCreation().Forget();
            });
        }

        partial void Help_Tapped(UIButton sender)
        {
            new ModalHelpViewController(HelpFAQType.ChatHome).Show();
        }

        private async Task NavigateToNewThreadCreation()
        {
            ShowLoader();
            var response = await Presenter.DidTapOnNewDialog();
            HideLoader();
        }

        private void Tableview_DidScroll(object sender, UIScrollView scrollView)
        {
            if(DataTableView.IndexPathsForVisibleRows != null && DataTableView.IndexPathsForVisibleRows.Length > 0 && tableviewSource != null && tableviewSource.DataList != null && tableviewSource.DataList.Count > 0)
            {
                var lastVisibleRow = DataTableView.IndexPathsForVisibleRows.Last().Row;

                if (tableviewSource.DataList.Count - lastVisibleRow <= cellBuffer)
                {
                    LoadMoreData().Forget();
                }
            }
        }

        private void Tableview_DidSelect(object sender, MessageThread thread)
        {
            Presenter.DidSelectThread(thread);
        }

        private void ShowNoData()
        {
            HelpButton.Hidden = true;
            DataTableView.Hidden = true;
            NoDataView.Hidden = false;
            View.BringSubviewToFront(NoDataView);
        }

        private void ShowDataTable()
        {
            HelpButton.Hidden = false;
            DataTableView.Hidden = false;
            NoDataView.Hidden = true;
            View.BringSubviewToFront(DataTableView);
        }

        private async Task RefreshData()
        {
            DataTableView.ShowLoader();
            var response = await Presenter.GetThreads();
            DataTableView.HideLoader();
            if (response != null && response.Count > 0)
            {
                tableviewSource.Clear();
                tableviewSource.UpdateList(response);
                this.InvokeOnMainThread(() => DataTableView.ReloadData());
                ShowDataTable();
            }
            else
            {
                ShowNoData();
            }
        }

        private async Task LoadMoreData()
        {
            if (Presenter.HasMoreData && !DataTableView.IsLoading)
            {
                DataTableView.IsLoading = true;
                var response = await Presenter.LoadMore();
                tableviewSource.UpdateList(response);
                this.InvokeOnMainThread(() => DataTableView.ReloadData());
                DataTableView.IsLoading = false;
            }
        }
	}

    public class DialogListDataSource : BaseTableViewSource<MessageThread>
    {
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row < DataList.Count)
            {
                var cell = tableView.DequeueReusableCell("DialogCell") as DialogCell;
                var data = DataList[indexPath.Row];
                var title = data.Title;
                String message;
                String datestamp;
                switch (data.TypeOfPartner)
                {
                    case PartnerType.Doctor: message = $"{"Chat.Dialog.Title.Doctor".Translate()} {data.PartnerName.ToNameCase()}"; break;
                    case PartnerType.MedicalCenter: message = $"{"Chat.Dialog.Title.Office".Translate()} ({data.PartnerName.ToNameCase()})"; break;
                    default: message = data.PartnerName.ToNameCase(); break;
                }

                datestamp = data.LastUpdated.GetDayString();

                cell.Configure(title, message, datestamp, data.LastMessageStatus);

                return cell;
            }
            else
            {
                return new UITableViewCell();
            }
        }
    }
}

