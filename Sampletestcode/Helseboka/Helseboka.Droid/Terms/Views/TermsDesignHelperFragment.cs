using Android.OS;
using Android.Views;
using Android.Widget;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.DesignSystem
{
    public interface ITermsDesignHelperDelegate
    {
        void ToggleState(int position);
        void ToggleSwitchState(int position);
    }
   
    public class TermsDesignHelperFragment : BaseFragment
    {
        public ITermsDesignHelperDelegate Delegate;

        public void DesignReadMoreParagraphView(UniversalViewHolder holder, string mainText, string moreText)
        {
            var textView = holder.GetView<TextView>(Resource.Id.paragraphTextView);
            var showMoreBtn = holder.GetView<TextView>(Resource.Id.showMoreBtn);

            var text = mainText;
            if (moreText.Length > 0)
            {
                showMoreBtn.Text = Resources.GetString(Resource.String.terms_text_showless);
                text = text + "\n\n" + moreText;
            }
            else
            {
                showMoreBtn.Text = Resources.GetString(Resource.String.terms_text_showmore);
            }

            if (!showMoreBtn.HasOnClickListeners)
            {
                showMoreBtn.Click += (_1, _2) => ToggleState(holder.AdapterPosition);
            }
            textView.Text = text;
        }

        public void DesignParagraphView(UniversalViewHolder holder, string mainText)
        {
            var textView = holder.GetView<TextView>(Resource.Id.paragraphTextView);
            var showMoreBtn = holder.GetView<TextView>(Resource.Id.showMoreBtn);
            showMoreBtn.Visibility = ViewStates.Gone;
            textView.Text = mainText;
        }

        public void DesignTermsSwitchView(UniversalViewHolder holder, string text, bool selected)
        {
            var termsSwitchContainer = holder.GetView<RelativeLayout>(Resource.Id.termsSwitchContainer);
            var textView = holder.GetView<TextView>(Resource.Id.termsTextView);
            var termsSelection = holder.GetView<Switch>(Resource.Id.termsSelection);

            textView.Text = text;
            termsSelection.Checked = selected;
            
            if (!termsSelection.HasOnClickListeners)
            {
                termsSelection.Click += (_1, _2) => ToggleSwitchState(holder.AdapterPosition);
            }
        }

        private void ToggleState(int position)
        {
            if (Delegate != null)
            {
                Delegate.ToggleState(position);
            }
        }

        private void ToggleSwitchState(int position)
        {
            if (Delegate != null)
            {
                Delegate.ToggleSwitchState(position);
            }
        }
    }
}