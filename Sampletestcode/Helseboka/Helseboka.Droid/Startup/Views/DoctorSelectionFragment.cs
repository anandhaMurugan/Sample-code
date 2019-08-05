
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Startup.Interface;
using Helseboka.Droid.Common.Views;
using Android.Support.V7.Widget;
using Helseboka.Core.Common.Constant;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Droid.Common.Listners;
using Android.Graphics;
using Android.Support.V4.Content;
using Helseboka.Core.Resources.StringResources;

namespace Helseboka.Droid.Startup.Views
{
    public class DoctorSelectionFragment : BaseFragment, IUniversalAdapter
    {
        public IDoctorSelectionPresenter Presenter
        {
            get => presenter as IDoctorSelectionPresenter;
        }

        private System.Timers.Timer timer = new System.Timers.Timer(AppConstant.SearchDelay);
        private Doctor selectedDoctor;
        private bool isloading = false;
        private View SearchContainer;
        private RecyclerView searchListView;
        private EditText searchText;
        private TextView doctorDetailsLabel;
        private Button okButton;
        private TextView pageTitle;
        private TextView pageSubTitle;
        private TextView errorTextView;
        private TextView doctorInfoText;
        private List<Doctor> searchDoctorResult;


        public DoctorSelectionFragment(IDoctorSelectionPresenter doctorSelectionPresenter) 
        {
            this.presenter = doctorSelectionPresenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_doctor_selection, null);

            SearchContainer = view.FindViewById<View>(Resource.Id.search_container);
            searchListView = view.FindViewById<RecyclerView>(Resource.Id.searchListView);
            searchText = view.FindViewById<EditText>(Resource.Id.searchText);
            doctorDetailsLabel = view.FindViewById<TextView>(Resource.Id.doctorDetails);
            okButton = view.FindViewById<Button>(Resource.Id.okButton);
            pageTitle = view.FindViewById<TextView>(Resource.Id.pageTitle);
            pageSubTitle = view.FindViewById<TextView>(Resource.Id.pageSubTitle);
            errorTextView = view.FindViewById<TextView>(Resource.Id.errorTextView);
            doctorInfoText = view.FindViewById<TextView>(Resource.Id.doctorInfoText);

            doctorDetailsLabel.Click += DoctorDetailsLabel_Click;
            searchText.TextChanged += SearchText_TextChanged;
            okButton.Click += OkButton_Click;

            okButton.Enabled = false;
            timer.Elapsed += Timer_Elapsed;
            timer.Stop();

            searchText.Hint = AppResources.DoctorSearchPlaceholder;
            errorTextView.Visibility = ViewStates.Gone;
            doctorInfoText.Visibility = ViewStates.Invisible;

            var layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            searchListView.SetLayoutManager(layoutManager);
            searchListView.SetAdapter(new GenericRecyclerAdapter(this));
            var scrollListner = new RecyclerViewScrollListener(layoutManager);
            scrollListner.LoadMore += ScrollListner_LoadMore;
            searchListView.AddOnScrollListener(scrollListner);

            LoadUserInfo().Forget();

            return view;
        }
        

        void DoctorDetailsLabel_Click(object sender, EventArgs e)
        {
            ShowSearch();
        }

        void SearchText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(searchText.Text))
            {
                timer.Stop();
                timer.Start();
            }
        }

        void OkButton_Click(object sender, EventArgs e)
        {
            Presenter.SelectDoctor(selectedDoctor).Forget();
        }

        void ScrollListner_LoadMore(object sender, LoadMoreEventArgs e)
        {
            LoadMoreData().Forget();
        }


        private async Task LoadUserInfo()
        {
            errorTextView.Visibility = ViewStates.Gone;
            ShowLoader();
            var response = await Presenter.GetCurrentUser();
            HideLoader();
            doctorInfoText.Visibility = ViewStates.Gone;
            pageSubTitle.Text = AppResources.DoctorSelectionSubtitleDisabled;
            if (response.IsSuccess)
            {
                pageTitle.Text = $"{AppResources.Salutation} {response.Result.FirstName}";
                if (response.Result.AssignedDoctor != null)
                {
                    SelectDoctor(response.Result.AssignedDoctor);
                }
            }
            else
            {
                ProcessAPIError(response);
            }
        }

        private void ShowSearch()
        {
            doctorDetailsLabel.Enabled = false;
            ClearSearchList();
            SearchContainer.Visibility = ViewStates.Visible;
            ShowKeyboard(searchText);
        }

        private void ClearSearchList()
        {
            searchDoctorResult = new List<Doctor>();
            searchListView.GetAdapter().NotifyDataSetChanged();
        }

        private void ReloadSearchList()
        {
            searchListView.GetAdapter().NotifyDataSetChanged();
        }

        protected void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            Activity.RunOnUiThread(async () =>
            {
                ClearSearchList();
                if (!String.IsNullOrEmpty(searchText.Text))
                {
                    searchDoctorResult = await Presenter.SearchDoctor(searchText.Text);
                    ReloadSearchList();
                }
            });
        }

        private void SelectDoctor(Doctor doctor)
        {
            selectedDoctor = doctor;
            doctorDetailsLabel.Text = $"{selectedDoctor.FullName}\n{selectedDoctor.OfficeName.ToNameCase()}";
            doctorDetailsLabel.SetTextAppearance(Resource.Style.withdata_selectable_view_style);
            doctorDetailsLabel.Enabled = true;
            okButton.Enabled = true;

            //doctorInfoText.Visibility = ViewStates.Visible;
            //doctorInfoText.Text = AppResources.DoctorSelectionHelpText;
            pageSubTitle.Text = AppResources.DoctorSelectionSubtitleEnabled;

            if (doctor.Enabled)
            {
                errorTextView.Visibility = ViewStates.Gone;

                doctorInfoText.Text = doctor.Remarks;
                doctorInfoText.Visibility = ViewStates.Visible;
            }
            else
            {
                errorTextView.Text = doctor.Remarks;
                errorTextView.Visibility = ViewStates.Visible;

                doctorInfoText.Visibility = ViewStates.Gone;
            }
        }

        private async Task LoadMoreData()
        {
            if (Presenter.HasMoreData && !isloading)
            {
                isloading = true;
                var response = await Presenter.LoadMore();
                if (response != null)
                {
                    int currentItemCount = searchDoctorResult.Count;
                    searchDoctorResult.AddRange(response);
                    searchListView.GetAdapter().NotifyItemRangeInserted(currentItemCount - 1, response.Count);
                }
                isloading = false;
            }
        }

        private void HideSearch()
        {
            doctorDetailsLabel.Enabled = true;
            SearchContainer.Visibility = ViewStates.Gone;
            searchText.Text = String.Empty;
            HideKeyboard(searchText);
        }

        public int GetItemCount()
        {
            return searchDoctorResult != null ? searchDoctorResult.Count() : 0;
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(Activity);
            var view = inflater.Inflate(Resource.Layout.template_search_result, null);

            var viewMap = new Dictionary<int, View>();
            viewMap.Add(Resource.Id.searchResultLabel, view.FindViewById(Resource.Id.searchResultLabel));

            return new UniversalViewHolder(view, viewMap);
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            var data = searchDoctorResult[position];

            holder.GetView<TextView>(Resource.Id.searchResultLabel).Text = $"{data.FullName}, {data.OfficeName.ToNameCase()}";
        }

        public void OnItemClick(int position)
        {
            var data = searchDoctorResult[position];

            HideSearch();
            SelectDoctor(data);
        }
    }
}
