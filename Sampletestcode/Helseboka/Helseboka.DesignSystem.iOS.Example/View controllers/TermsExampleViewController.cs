using System;
using CoreGraphics;
using Foundation;
using Helseboka.DesignSystem.iOS.Components.Cells;
using Helseboka.DesignSystem.iOS.Constants;
using UIKit;

namespace Helseboka.DesignSystem.iOS.Example.Viewcontrollers
{
    public partial class TermsExampleViewController : UITableViewController, IReadMoreTextTableViewCellDelegate
    {
        readonly string ShortText = "Helseboka saves your personal information in a secure manner.";
        readonly string MoreText = "All data is stored securely using state of the art encryption techniques.";

        bool[] isExpanded = { true, false, false };

        public TermsExampleViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            TableView.RegisterNibForCellReuse(ReadMoreTextTableViewCell.Nib, ReadMoreTextTableViewCell.Key);
            TableView.RegisterNibForCellReuse(TextTableViewCell.Nib, TextTableViewCell.Key);
            TableView.RegisterNibForCellReuse(SwitchTableViewCell.Nib, SwitchTableViewCell.Key);
            TableView.SeparatorColor = Colors.Grey600;

            // Set a zero sized view as footer to avoid having separator lines repeat down the screen
            TableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 0));
        }


        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return 4;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell;

            if (indexPath.Row == 0)
            {
                var readMoreCell = (ReadMoreTextTableViewCell)tableView.DequeueReusableCell(ReadMoreTextTableViewCell.Key, indexPath);
                if (!isExpanded[indexPath.Row])
                {
                    var image = UIImage.FromBundle("caret down").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                    readMoreCell.SetTexts(ShortText, "", "Show more", UIImage.FromBundle("caret down"));
                }
                else
                {
                    var image = UIImage.FromBundle("caret down").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                    readMoreCell.SetTexts(ShortText, MoreText, "Show Less", UIImage.FromBundle("caret up"));
                }
                readMoreCell.Delegate = this;
                cell = readMoreCell;
            }
            else if (indexPath.Row == 1)
            {
                var textCell = (TextTableViewCell)tableView.DequeueReusableCell(TextTableViewCell.Key, indexPath);
                textCell.SetText("A long text that will break multiple lines. This text should be shown in it's entirety, and not be clipped or truncated in any way");
                cell = textCell;
            }
            else
            {
                var switchCell = (SwitchTableViewCell)tableView.DequeueReusableCell(SwitchTableViewCell.Key, indexPath);
                switchCell.TextLabel.Text = "Some other text that will span multiple lines, and make the cell grow in the height direction.";
                cell = switchCell;
            }

            if (indexPath.Row < 2)
            {
                cell.SeparatorInset = new UIEdgeInsets(0, nfloat.MaxValue, 0, 0);
            }

            return cell;
        }

        public void ToggleState(ReadMoreTextTableViewCell cell)
        {
            var indexPath = TableView.IndexPathForCell(cell);
            isExpanded[indexPath.Row] = !isExpanded[indexPath.Row];
            TableView.ReloadData();
        }
    }
}

