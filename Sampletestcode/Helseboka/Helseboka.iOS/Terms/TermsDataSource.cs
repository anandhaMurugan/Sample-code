using Foundation;
using Helseboka.Core.Terms.Model;
using Helseboka.iOS.Common.TableViewDelegates;
using System;
using UIKit;
using Helseboka.iOS.Components.Cells;
using System.Collections.Generic;
using Helseboka.iOS.Common.Extension;

namespace Helseboka.iOS.Terms
{
    public class TermsDataSource : BaseTableViewSource<TermsAndParagraphs>, IReadMoreTextTableViewCellDelegate, ISwitchTableViewCellDelegate
    {

        public bool[] IsExpandedOrToggled { get; set; }
        public List<int> IsRequiredIndexes { get; set; }
        private UITableView TermsTableView;
        public ITermsDataSourceDelegate Delegate;

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return DataList != null ? DataList.Count : 0;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell;
            if (DataList != null)
            {
                if (indexPath.Row == 0)
                {
                    TermsTableView = tableView;
                }
                if (!DataList[indexPath.Row].IsTerms)
                {
                    if (!string.IsNullOrEmpty(DataList[indexPath.Row].ReadMore))
                    {
                        var readMoreCell = (ReadMoreTextTableViewCell)tableView.DequeueReusableCell(ReadMoreTextTableViewCell.Key, indexPath);
                        if (!IsExpandedOrToggled[indexPath.Row])
                        {
                            readMoreCell.SetTexts(DataList[indexPath.Row].Text, "", "Terms.ShowMore.Text".Translate(), UIImage.FromBundle("caret down"));
                        }
                        else
                        {
                            readMoreCell.SetTexts(DataList[indexPath.Row].Text, DataList[indexPath.Row].ReadMore, "Terms.ShowLess.Text".Translate(), UIImage.FromBundle("caret up"));
                        }
                        readMoreCell.Delegate = this;
                        cell = readMoreCell;
                    }
                    else
                    {
                        var textCell = (TextTableViewCell)tableView.DequeueReusableCell(TextTableViewCell.Key, indexPath);
                        textCell.SetText(DataList[indexPath.Row].Text);
                        cell = textCell;
                    }
                    cell.SeparatorInset = new UIEdgeInsets(0, nfloat.MaxValue, 0, 0);
                }
                else
                {
                    var switchCell = (SwitchTableViewCell)tableView.DequeueReusableCell(SwitchTableViewCell.Key, indexPath);
                    switchCell.SetTextAndSwitch(DataList[indexPath.Row].Text, IsExpandedOrToggled[indexPath.Row]);
                    switchCell.Delegate = this;
                    cell = switchCell;
                }
                cell.DirectionalLayoutMargins = new NSDirectionalEdgeInsets(top: 0, leading: 25, bottom: 0, trailing: 25);
               
                return cell;
            }
            return null;
        }

        public void ToggleState(ReadMoreTextTableViewCell cell)
        {
            var indexPath = TermsTableView.IndexPathForCell(cell);
            IsExpandedOrToggled[indexPath.Row] = !IsExpandedOrToggled[indexPath.Row];
            TermsTableView.ReloadData();
        }

        public void ToggleSwitchState(SwitchTableViewCell cell)
        {
            var indexPath = TermsTableView.IndexPathForCell(cell);
            IsExpandedOrToggled[indexPath.Row] = !IsExpandedOrToggled[indexPath.Row];
            if (Delegate != null)
            {
                Delegate.ContinueBtnLogics();
            }
        }
    }
}