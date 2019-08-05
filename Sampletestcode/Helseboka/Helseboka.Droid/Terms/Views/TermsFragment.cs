using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Terms.Interface;
using Helseboka.Core.Terms.Model;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Core.Common.Extension;
using Android.Support.V7.Widget;
using Helseboka.Droid.Common.Listners;
using Helseboka.Droid.DesignSystem;

namespace Helseboka.Droid.Startup.Views
{
    public class TermsFragment : TermsDesignHelperFragment, IMultipleItemUniversalAdapter, ITermsDesignHelperDelegate
    {
        RecyclerView termsListView;
        Button continueButton;
        List<TermsAndParagraphs> termsAndParagraphs;
        private bool isLastItemVisible = false;
        private List<int> acceptedIds;
        public bool[] isExpandedOrToggled;
        public List<int> isRequiredIndexes;
        TermsListModel termsResponse;

        public ITermsPresenter Presenter
        {
            get => presenter as ITermsPresenter;
        }
       
        public TermsFragment(ITermsPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_terms, null);

            termsListView = view.FindViewById<RecyclerView>(Resource.Id.termsListView);
            continueButton = view.FindViewById<Button>(Resource.Id.continueButton);

            continueButton.Enabled = false;
            Delegate = this;
            var layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            termsListView.SetLayoutManager(layoutManager);
            termsListView.SetAdapter(new GenericRecyclerAdapter(this));
            var scrollListener = new RecyclerViewScrollListener(layoutManager);
            termsListView.AddOnScrollListener(scrollListener);

            scrollListener.LastItemVisibleEvent += ScrollListener_LastItemVisibleEvent;
            continueButton.Click += ContinueButton_Clicked;

            return view;
        }

        private void ScrollListener_LastItemVisibleEvent(object sender, LastItemVisibleEventArgs e)
        {
            bool lastItemVisible = e.IsLastItemVisible;
            isLastItemVisible = lastItemVisible;
            if (lastItemVisible)
            {
                EnableOrDisableBtn();
                return;
            }
            continueButton.Enabled = false;
        }

        private void EnableOrDisableBtn()
        {
            if (isRequiredIndexes.Any(x => !isExpandedOrToggled[x]))
            {
                continueButton.Enabled = false;
            }
            else
            {
                continueButton.Enabled = true;
            }
        }

        private void ContinueButton_Clicked(object sender, EventArgs e)
        {
            ShowLoader();
            acceptedIds = new List<int>();

            for (int i = 0; i < termsAndParagraphs.Count; i++)
            {
                if (termsAndParagraphs[i].IsTerms && isExpandedOrToggled[i])
                {
                    acceptedIds.Add(termsAndParagraphs[i].Id);
                }
            }

            if (acceptedIds != null)
            {
                var response = Presenter.UpdateTerms(acceptedIds);
                if(response == null)
                {
                    continueButton.Enabled = true;
                }
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            RefreshData().Forget();
        }

        private async Task RefreshData()
        {
            termsResponse = await Presenter.RefreshTermsDatas();
            if (termsResponse != null)
            {
                termsAndParagraphs = termsResponse.TermsAndParagraphsList;
                UpdateListsAndArrays().Forget();              
                termsListView.GetAdapter().NotifyDataSetChanged();
                termsListView.ScrollToPosition(0);
            }
        }

        private async Task UpdateListsAndArrays()
        {
            isExpandedOrToggled = new bool[termsAndParagraphs.Count];
            isRequiredIndexes = new List<int>();
            int i = 0;
            while (termsAndParagraphs != null)
            {
                if (termsAndParagraphs[i].Required)
                {
                    isRequiredIndexes.Add(i);
                }
                isExpandedOrToggled[i] = !termsAndParagraphs[i].IsTerms ? false : termsAndParagraphs[i].Accepted;
                i++;
            }
        }

        public int GetItemViewType(int position)
        {
            return GetTypeFromList(position);
        }

        public int GetItemCount()
        {
            return termsAndParagraphs != null ? termsAndParagraphs.Count : 0;
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            if (termsAndParagraphs != null)
            {
                var data = termsAndParagraphs[position];

                if (holder.ItemViewType == 0)
                {
                    if (!isExpandedOrToggled[position])
                    {
                        DesignReadMoreParagraphView(holder, data.Text, "");
                    }
                    else
                    {
                        DesignReadMoreParagraphView(holder, data.Text, data.ReadMore);
                    }
                }
                else if (holder.ItemViewType == 1)
                {
                    DesignParagraphView(holder, data.Text);
                }
                else 
                {
                     DesignTermsSwitchView(holder, data.Text, isExpandedOrToggled[position]);
                }
            }
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(Activity);
            var viewMap = new Dictionary<int, View>();

            if (viewType == 0)
            {
                var view = inflater.Inflate(Resource.Layout.terms_paragraphs_template, null);
                viewMap.Add(Resource.Id.paragraphContainer, view.FindViewById(Resource.Id.paragraphContainer));
                viewMap.Add(Resource.Id.paragraphTextView, view.FindViewById(Resource.Id.paragraphTextView));
                viewMap.Add(Resource.Id.showMoreBtn, view.FindViewById(Resource.Id.showMoreBtn));

                return new UniversalViewHolder(view, viewMap);
            }
            else if(viewType == 1)
            {
                var view = inflater.Inflate(Resource.Layout.terms_paragraphs_template, null);
                viewMap.Add(Resource.Id.paragraphContainer, view.FindViewById(Resource.Id.paragraphContainer));
                viewMap.Add(Resource.Id.paragraphTextView, view.FindViewById(Resource.Id.paragraphTextView));
                viewMap.Add(Resource.Id.showMoreBtn, view.FindViewById(Resource.Id.showMoreBtn));
                return new UniversalViewHolder(view, viewMap);
            }
            else
            {
                var view = inflater.Inflate(Resource.Layout.terms_list_template, null);
                viewMap.Add(Resource.Id.termsSwitchContainer, view.FindViewById(Resource.Id.termsSwitchContainer));
                viewMap.Add(Resource.Id.termsTextView, view.FindViewById(Resource.Id.termsTextView));
                viewMap.Add(Resource.Id.termsSelection, view.FindViewById(Resource.Id.termsSelection));
                return new UniversalViewHolder(view, viewMap);
            }
        }

        public void OnItemClick(int position)
        {
            var data = termsAndParagraphs[position];
            if (!data.IsTerms && !string.IsNullOrEmpty(data.ReadMore))
            {
                ToggleState(position);
            }
        }

        public void ToggleState(int position)
        {      
            isExpandedOrToggled[position] = !isExpandedOrToggled[position];
            termsListView.GetAdapter().NotifyDataSetChanged();
        }

        public void ToggleSwitchState(int position)
        {
            isExpandedOrToggled[position] = !isExpandedOrToggled[position];
            termsListView.GetAdapter().NotifyDataSetChanged();
            if (isLastItemVisible)
            {
                EnableOrDisableBtn();
            }
        }

        private int GetTypeFromList(int position)
        {
            var data = termsAndParagraphs[position];
            if (!string.IsNullOrEmpty(data.ReadMore) && !data.IsTerms)
            {
                return 0;
            }
            else if (string.IsNullOrEmpty(data.ReadMore) && !data.IsTerms)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

    }
}