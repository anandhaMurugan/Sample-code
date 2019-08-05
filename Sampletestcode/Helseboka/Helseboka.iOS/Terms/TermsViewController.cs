using Foundation;
using Helseboka.Core.Terms.Interface;
using Helseboka.Core.Terms.Model;
using Helseboka.iOS.Common.View;
using System;
using System.Threading.Tasks;
using UIKit;
using Helseboka.Core.Common.Extension;
using CoreGraphics;
using Helseboka.iOS.Components.Cells;
using Helseboka.iOS.Constants;
using System.Collections.Generic;
using System.Linq;
using Helseboka.iOS.Terms;
using Helseboka.iOS.Common.Extension;

namespace Helseboka.iOS
{
    public interface ITermsDataSourceDelegate
    {
        void ContinueBtnLogics();
    }
    public partial class TermsViewController : BaseView, ITermsDataSourceDelegate
    {
        public static readonly String Identifier = "TermsViewController";
        private TermsDataSource termsDataSource = new TermsDataSource();
        private List<TermsAndParagraphs> totalList;
        private List<int> acceptedIds;

        public ITermsPresenter Presenter
        {
            get => presenter as ITermsPresenter;
            set => presenter = value;
        }
        public TermsViewController() { }
        public TermsViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UIColor semiTransparentColor = new UIColor(0, 0, 0, 0.5f);
            FloatingBottomView.BackgroundColor = semiTransparentColor;
            ContinueBtn.Enabled = false;
            termsDataSource.Delegate = this;
            TermsTableView.Source = termsDataSource;
            TermsTableView.RegisterNibForCellReuse(ReadMoreTextTableViewCell.Nib, ReadMoreTextTableViewCell.Key);
            TermsTableView.RegisterNibForCellReuse(TextTableViewCell.Nib, TextTableViewCell.Key);
            TermsTableView.RegisterNibForCellReuse(SwitchTableViewCell.Nib, SwitchTableViewCell.Key);
            TermsTableView.SeparatorColor = Colors.Grey600;
            ContinueBtn.SetTitle("Terms.ContinueButton.Text".Translate(), UIControlState.Normal);
            TermsTitle.Text = "Terms.Page.Title".Translate();
            TermsTableView.EstimatedRowHeight = 200f;
            
            termsDataSource.DidScroll += TermsDataSource_DidScroll;
            ContinueBtn.TouchUpInside += ContinueBtn_TouchUpInside;

            TermsTableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 0));
            var insets = new UIEdgeInsets(top: 0, left: 0, bottom: 80, right: 0);
            TermsTableView.ContentInset = insets;
            RefreshData().Forget();
        }

        private async Task RefreshData()
        {
            termsDataSource.Clear();
            var response = await Presenter.RefreshTermsDatas();
            if (response != null && response.TermsAndParagraphsList.Count > 0)
            {
                totalList = response.TermsAndParagraphsList;
                UpdateListsNdArrays().Forget();
                termsDataSource.UpdateList(response.TermsAndParagraphsList);
                this.InvokeOnMainThread(() => {
                    TermsTableView.ReloadData();
                    TermsTableView.ScrollToRow(NSIndexPath.FromRowSection(0, 0), UITableViewScrollPosition.Top, false);
                });
            }
        }
        private async Task UpdateListsNdArrays()
        {
            termsDataSource.IsExpandedOrToggled = new bool[totalList.Count];
            termsDataSource.IsRequiredIndexes = new List<int>();
            int i = 0;
            while (totalList != null)
            {
                if (totalList[i].Required)
                {
                    termsDataSource.IsRequiredIndexes.Add(i);
                }
                termsDataSource.IsExpandedOrToggled[i] = !totalList[i].IsTerms ? false : totalList[i].Accepted;
                i++;
            }
        }

        private void TermsDataSource_DidScroll(object sender, UIScrollView e)
        {
            ContinueBtnLogics();
        }

        public void ContinueBtnLogics()
        {
            if (TermsTableView.IndexPathsForVisibleRows.Length > 0)
            {
                int lastVisibleRow = TermsTableView.IndexPathsForVisibleRows.Last().Row;
                if (lastVisibleRow == totalList.Count - 1)
                {
                    EnableOrDisableBtn();
                    return;
                }
                ContinueBtn.Enabled = false;
            }
        }

        private void EnableOrDisableBtn()
        {
            if (termsDataSource.IsRequiredIndexes.Any(x => !termsDataSource.IsExpandedOrToggled[x]))
            {
                ContinueBtn.Enabled = false;
            }
            else
            {
                ContinueBtn.Enabled = true;
            }
        }

        private void ContinueBtn_TouchUpInside(object sender, EventArgs e)
        {
            ShowLoader();
            acceptedIds = new List<int>();

            for (int i = 0; i < totalList.Count; i++)
            {
                if (totalList[i].IsTerms && termsDataSource.IsExpandedOrToggled[i])
                {
                    acceptedIds.Add(totalList[i].Id);
                }
            }

            if (acceptedIds != null)
            {
                Presenter.UpdateTerms(acceptedIds);
            }
            else
            {
                HideLoader();
            }
        }
    }
}